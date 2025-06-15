/* eslint-disable react-refresh/only-export-components */
import { LaunchScreen } from "@/components/launch-screen";
import {
  AUTH_STORAGE_KEY,
  Key,
  REFRESH_TOKEN_STORAGE_KEY,
} from "@/constants/consts";
import { reactQueryClient } from "@/libs/react-query-client";
import type { IUser } from "@/models";
import { sleep } from "@/utils/sleep";
import { useQuery } from "@tanstack/react-query";

import { createContext, useEffect, useState } from "react";
import type { PropsWithChildren } from "react";

type ISession = {
  isFetching: boolean;
  hasSession: boolean;
  applicationUser: IUser | null;
  signIn: (token: string) => void;
  signOut: () => void;
};

export const SessionContext = createContext<ISession>({
  isFetching: false,

  hasSession: false,
  applicationUser: null,
  signIn: Function,
  signOut: Function,
});

async function mockStoreAuthState(): Promise<IUser> {
  await sleep(1000);
  return {
    id: "id",
    email: "gabrielk6.mobile@gmail.com",
    name: "Gabriel dos Santos",
  } as IUser;
}

export function SessionProvider({ children }: PropsWithChildren) {
  const [signedIn, setSignedIn] = useState(() => {
    const jwt = localStorage.getItem(AUTH_STORAGE_KEY);
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_STORAGE_KEY);
    return !!jwt && !!refreshToken;
  });
  const hasSession = !!signedIn;

  const { isFetching, data, isError } = useQuery<IUser, Error>({
    queryKey: [Key.Profile],
    queryFn: mockStoreAuthState,
    enabled: hasSession,
    staleTime: Infinity,
  });

  function signIn(accessToken: string) {
    localStorage.setItem(AUTH_STORAGE_KEY, accessToken);
    setSignedIn(true);
  }

  function signOut() {
    localStorage.removeItem(AUTH_STORAGE_KEY);
    reactQueryClient.removeQueries({ queryKey: [Key.Profile] });
    setSignedIn(false);
  }

  useEffect(() => {
    if (isError) {
      signOut();
    }
  }, [isError]);

  return (
    <SessionContext.Provider
      value={{
        isFetching,
        applicationUser: data ?? null,
        hasSession,
        signIn,
        signOut,
      }}
    >
      <LaunchScreen isLoading={isFetching} />
      {!isFetching && children}
    </SessionContext.Provider>
  );
}
