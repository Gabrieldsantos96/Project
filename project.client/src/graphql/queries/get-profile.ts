// graphql/queries/get-profile.ts
import { gql } from "@apollo/client";

export const QUERY_GET_PROFILE = gql`
  query GetProfile {
    userProfile {
      id
      userName
      email
    }
  }
`;

// export function useGetProfileQuery(
//   options?: QueryHookOptions<GetProfileQuery, GetProfileQueryVariables>
// ) {
//   return useQuery<GetProfileQuery, GetProfileQueryVariables>(
//     QUERY_GET_PROFILE,
//     options
//   )
// }
