import { gql } from "@apollo/client";

export const MUTATION_LOGOUT = gql`
  mutation Logout($input: RefreshTokenInput!) {
    logout(input: $input) {
      data
      success
      message
      statusCode
    }
  }
`;

// export function useLogoutMutation(
//   options?: MutationHookOptions<LogoutMutation, LogoutMutationVariables>
// ) {
//   return useMutation<LogoutMutation, LogoutMutationVariables>(
//     MUTATION_LOGOUT,
//     options
//   )
// }
