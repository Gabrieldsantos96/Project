import { gql } from "@apollo/client";

export const MUTATION_SIGN_IN = gql`
  mutation SignIn($input: SignInInput!) {
    signIn(input: $input) {
      data
      success
      message
      statusCode
    }
  }
`;

// export function useSignInMutation(
//   options?: MutationHookOptions<SignInMutation, SignInMutationVariables>
// ) {
//   return useMutation<SignInMutation, SignInMutationVariables>(
//     MUTATION_SIGN_IN,
//     options
//   )
// }
