import {
  AUTH_STORAGE_KEY,
  REFRESH_TOKEN_STORAGE_KEY,
} from "@/constants/consts";
import {
  ApolloClient,
  InMemoryCache,
  createHttpLink,
  from,
} from "@apollo/client";
import { onError } from "@apollo/client/link/error";

import { MUTATION_REFRESH_TOKEN } from "@/graphql/mutations/refresh-token";
import { setContext } from "@apollo/client/link/context";

function generateRefreshTokenLinkOnUnauthError(props: {
  key: string;
  callbackFn: () => Promise<void>;
}) {
  return [
    onError(({ graphQLErrors, operation, forward }) => {
      if (!graphQLErrors) return;

      for (const { path, extensions } of graphQLErrors) {
        if (extensions?.code !== "UNAUTHENTICATED" || !path) continue;
        if (path.includes(props.key)) break;

        const { getContext, setContext } = operation;
        const context = getContext();

        setContext({
          ...context,
          headers: {
            ...context?.headers,
            _needsRefresh: true,
          },
        });

        return forward(operation);
      }
    }),
    setContext(async (_, previousContext) => {
      if (previousContext?.headers?._needsRefresh) {
        await props.callbackFn();
      }

      return previousContext;
    }),
  ];
}

const uri = "https://localhost:7130/bff/graphql";

const httpLink = createHttpLink({ uri });

const authLink = setContext((_, previousContext) => {
  const token = localStorage.getItem(AUTH_STORAGE_KEY);
  return {
    ...previousContext,
    headers: {
      ...previousContext?.headers,
      authorization: token ? `Bearer ${token}` : "",
    },
  };
});

async function getRefreshToken() {
  try {
    const currentRefreshToken = localStorage.getItem(REFRESH_TOKEN_STORAGE_KEY);
    if (!currentRefreshToken) {
      throw new Error("No refresh token available");
    }

    const { data: result } = await apolloClient.mutate({
      mutation: MUTATION_REFRESH_TOKEN,
      variables: { refreshToken: currentRefreshToken },
    });

    if (!result.success) {
      throw new Error("Failed to refresh token");
    }

    const { accessToken, refreshToken } = JSON.parse(result.data);
    localStorage.setItem(AUTH_STORAGE_KEY, accessToken);
    localStorage.setItem(REFRESH_TOKEN_STORAGE_KEY, refreshToken);
  } catch {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    localStorage.removeItem(REFRESH_TOKEN_STORAGE_KEY);
  }
}

export const apolloClient = new ApolloClient({
  link: from([
    ...generateRefreshTokenLinkOnUnauthError({
      key: REFRESH_TOKEN_STORAGE_KEY,
      callbackFn: getRefreshToken,
    }),
    authLink,
    httpLink,
  ]),
  cache: new InMemoryCache(),
});
