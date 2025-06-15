// graphql/queries/get-profile.ts
import { gql, QueryHookOptions, useQuery } from "@apollo/client"
import { GetProfileQuery, GetProfileQueryVariables } from "../generated"

export const QUERY_GET_PROFILE = gql`
  query GetProfile {
    userProfile {
      id
      userName
      email
      deviceIds
    }
  }
`

export function useGetProfileQuery(
  options?: QueryHookOptions<GetProfileQuery, GetProfileQueryVariables>
) {
  return useQuery<GetProfileQuery, GetProfileQueryVariables>(
    QUERY_GET_PROFILE,
    options
  )
}
