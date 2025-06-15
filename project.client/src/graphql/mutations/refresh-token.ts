import { gql } from "@apollo/client";

export const MUTATION_REFRESH_TOKEN = gql`
  mutation RefreshToken($input: RefreshTokenInput!) {
    refreshToken(input: $input) {
      data
      success
      message
      statusCode
    }
  }
`;

// export function useRefreshTokenMutation(
//   options?: MutationHookOptions<
//     RefreshTokenMutation,
//     RefreshTokenMutationVariables
//   >
// ) {
//   return useMutation<RefreshTokenMutation, RefreshTokenMutationVariables>(
//     MUTATION_REFRESH_TOKEN,
//     options
//   )
// }
