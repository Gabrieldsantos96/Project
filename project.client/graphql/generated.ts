import { gql } from '@apollo/client';
import * as Apollo from '@apollo/client';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
export type MakeEmpty<T extends { [key: string]: unknown }, K extends keyof T> = { [_ in K]?: never };
export type Incremental<T> = T | { [P in keyof T]?: P extends ' $fragmentName' | '__typename' ? T[P] : never };
const defaultOptions = {} as const;
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: { input: string; output: string; }
  String: { input: string; output: string; }
  Boolean: { input: boolean; output: boolean; }
  Int: { input: number; output: number; }
  Float: { input: number; output: number; }
  DateTime: { input: any; output: any; }
  UUID: { input: any; output: any; }
};

/** Defines when a policy shall be executed. */
export enum ApplyPolicy {
  /** After the resolver was executed. */
  AfterResolver = 'AFTER_RESOLVER',
  /** Before the resolver was executed. */
  BeforeResolver = 'BEFORE_RESOLVER',
  /** The policy is applied in the validation step before the execution. */
  Validation = 'VALIDATION'
}

export enum BadgeType {
  Analyst = 'Analyst',
  Developer = 'Developer',
  Manager = 'Manager'
}

export type ChangeJobRoleInput = {
  employeeId: Scalars['UUID']['input'];
};

export type ChangePasswordInput = {
  confirmPassword: Scalars['String']['input'];
  currentPassword: Scalars['String']['input'];
  newPassword: Scalars['String']['input'];
};

export type Company = {
  __typename?: 'Company';
  createdAt: Scalars['DateTime']['output'];
  createdBy?: Maybe<Scalars['String']['output']>;
  departments: Array<Department>;
  id: Scalars['Int']['output'];
  isTransient: Scalars['Boolean']['output'];
  name: Scalars['String']['output'];
  refId: Scalars['UUID']['output'];
  tenant: Tenant;
  tenantId: Scalars['Int']['output'];
  updatedAt?: Maybe<Scalars['DateTime']['output']>;
  updatedBy?: Maybe<Scalars['String']['output']>;
};

export type CreateAccountInput = {
  confirmPassword: Scalars['String']['input'];
  email: Scalars['String']['input'];
  name: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type Department = {
  __typename?: 'Department';
  company: Company;
  companyId: Scalars['Int']['output'];
  createdAt: Scalars['DateTime']['output'];
  createdBy?: Maybe<Scalars['String']['output']>;
  id: Scalars['Int']['output'];
  isTransient: Scalars['Boolean']['output'];
  name: Scalars['String']['output'];
  refId: Scalars['UUID']['output'];
  tenant: Tenant;
  tenantId: Scalars['Int']['output'];
  updatedAt?: Maybe<Scalars['DateTime']['output']>;
  updatedBy?: Maybe<Scalars['String']['output']>;
};

export type Employee = {
  __typename?: 'Employee';
  createdAt: Scalars['DateTime']['output'];
  createdBy?: Maybe<Scalars['String']['output']>;
  id: Scalars['Int']['output'];
  isTransient: Scalars['Boolean']['output'];
  jobName: Scalars['String']['output'];
  refId: Scalars['UUID']['output'];
  tenant: Tenant;
  tenantBadges: Array<TenantBadge>;
  tenantId: Scalars['Int']['output'];
  updatedAt?: Maybe<Scalars['DateTime']['output']>;
  updatedBy?: Maybe<Scalars['String']['output']>;
  user: ProjectUser;
  userId: Scalars['Int']['output'];
};

export type EmployeeDto = {
  __typename?: 'EmployeeDto';
  badgeDtos: Array<BadgeType>;
  id: Scalars['UUID']['output'];
  name: Scalars['String']['output'];
  tenantId: Scalars['UUID']['output'];
};

export type Entity = {
  __typename?: 'Entity';
  createdAt?: Maybe<Scalars['DateTime']['output']>;
  createdBy?: Maybe<Scalars['String']['output']>;
  id: Scalars['Int']['output'];
  isTransient: Scalars['Boolean']['output'];
  updatedAt?: Maybe<Scalars['DateTime']['output']>;
  updatedBy?: Maybe<Scalars['String']['output']>;
};

export type MutationResult = {
  __typename?: 'MutationResult';
  message: Scalars['String']['output'];
  statusCode: Scalars['Int']['output'];
  success: Scalars['Boolean']['output'];
};

export type MutationResultOfAuthenticationDto = {
  __typename?: 'MutationResultOfAuthenticationDto';
  data: Scalars['String']['output'];
  message: Scalars['String']['output'];
  statusCode: Scalars['Int']['output'];
  success: Scalars['Boolean']['output'];
};

export type MutationResultOfObject = {
  __typename?: 'MutationResultOfObject';
  data: Scalars['String']['output'];
  message: Scalars['String']['output'];
  statusCode: Scalars['Int']['output'];
  success: Scalars['Boolean']['output'];
};

export type Mutations = {
  __typename?: 'Mutations';
  changeJobRole: MutationResult;
  changePassword: MutationResult;
  createAccount: UserProfileDto;
  refreshToken: MutationResultOfAuthenticationDto;
  resetPassword: MutationResult;
  resetPasswordRequest: MutationResult;
  signin: MutationResultOfAuthenticationDto;
  signout: MutationResultOfObject;
};


export type MutationsChangeJobRoleArgs = {
  input: ChangeJobRoleInput;
};


export type MutationsChangePasswordArgs = {
  input: ChangePasswordInput;
};


export type MutationsCreateAccountArgs = {
  input: CreateAccountInput;
};


export type MutationsRefreshTokenArgs = {
  input: RefreshTokenInput;
};


export type MutationsResetPasswordArgs = {
  input: ResetPasswordInput;
};


export type MutationsResetPasswordRequestArgs = {
  input: ResetPasswordRequestInput;
};


export type MutationsSigninArgs = {
  input: SigninInput;
};


export type MutationsSignoutArgs = {
  input: RefreshTokenInput;
};

export type ProjectUser = {
  __typename?: 'ProjectUser';
  accessFailedCount: Scalars['Int']['output'];
  concurrencyStamp?: Maybe<Scalars['String']['output']>;
  createdAt: Scalars['DateTime']['output'];
  email?: Maybe<Scalars['String']['output']>;
  emailConfirmed: Scalars['Boolean']['output'];
  employeeSelectedId?: Maybe<Scalars['Int']['output']>;
  employees: Array<Employee>;
  id: Scalars['Int']['output'];
  lockoutEnabled: Scalars['Boolean']['output'];
  lockoutEnd?: Maybe<Scalars['DateTime']['output']>;
  name: Scalars['String']['output'];
  normalizedEmail?: Maybe<Scalars['String']['output']>;
  normalizedUserName?: Maybe<Scalars['String']['output']>;
  passwordHash?: Maybe<Scalars['String']['output']>;
  phoneNumber?: Maybe<Scalars['String']['output']>;
  phoneNumberConfirmed: Scalars['Boolean']['output'];
  refId: Scalars['UUID']['output'];
  securityStamp?: Maybe<Scalars['String']['output']>;
  twoFactorEnabled: Scalars['Boolean']['output'];
  updatedAt: Scalars['DateTime']['output'];
  userName?: Maybe<Scalars['String']['output']>;
};

export type Queries = {
  __typename?: 'Queries';
  userProfile: UserProfileDto;
};

export type RefreshTokenInput = {
  refreshToken: Scalars['String']['input'];
};

export type ResetPasswordInput = {
  confirmPassword: Scalars['String']['input'];
  encodedEmail: Scalars['String']['input'];
  encodedToken: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type ResetPasswordRequestInput = {
  email: Scalars['String']['input'];
};

export type SigninInput = {
  email: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type Tenant = {
  __typename?: 'Tenant';
  companies: Array<Company>;
  createdAt: Scalars['DateTime']['output'];
  createdBy?: Maybe<Scalars['String']['output']>;
  employees: Array<Employee>;
  id: Scalars['Int']['output'];
  isTransient: Scalars['Boolean']['output'];
  name: Scalars['String']['output'];
  refId: Scalars['UUID']['output'];
  tenantBadges: Array<TenantBadge>;
  updatedAt?: Maybe<Scalars['DateTime']['output']>;
  updatedBy?: Maybe<Scalars['String']['output']>;
};

export type TenantBadge = {
  __typename?: 'TenantBadge';
  badgeType: BadgeType;
  createdAt: Scalars['DateTime']['output'];
  createdBy?: Maybe<Scalars['String']['output']>;
  employees: Array<Employee>;
  id: Scalars['Int']['output'];
  isTransient: Scalars['Boolean']['output'];
  refId: Scalars['UUID']['output'];
  tenant: Tenant;
  tenantId: Scalars['Int']['output'];
  updatedAt?: Maybe<Scalars['DateTime']['output']>;
  updatedBy?: Maybe<Scalars['String']['output']>;
};

export type UserProfileDto = {
  __typename?: 'UserProfileDto';
  email: Scalars['String']['output'];
  employeeDtos: Array<EmployeeDto>;
  employeeSelectedRefId?: Maybe<Scalars['UUID']['output']>;
  id: Scalars['UUID']['output'];
  userName: Scalars['String']['output'];
};

export type RefreshTokenMutationVariables = Exact<{
  input: RefreshTokenInput;
}>;


export type RefreshTokenMutation = { __typename?: 'Mutations', refreshToken: { __typename?: 'MutationResultOfAuthenticationDto', data: string, success: boolean, message: string, statusCode: number } };

export type SigninMutationVariables = Exact<{
  input: SigninInput;
}>;


export type SigninMutation = { __typename?: 'Mutations', signin: { __typename?: 'MutationResultOfAuthenticationDto', data: string, success: boolean, message: string, statusCode: number } };

export type LogoutMutationVariables = Exact<{
  input: RefreshTokenInput;
}>;


export type LogoutMutation = { __typename?: 'Mutations', signout: { __typename?: 'MutationResultOfObject', data: string, success: boolean, message: string, statusCode: number } };

export type GetProfileQueryVariables = Exact<{ [key: string]: never; }>;


export type GetProfileQuery = { __typename?: 'Queries', userProfile: { __typename?: 'UserProfileDto', id: any, userName: string, email: string } };


export const RefreshTokenDocument = gql`
    mutation RefreshToken($input: RefreshTokenInput!) {
  refreshToken(input: $input) {
    data
    success
    message
    statusCode
  }
}
    `;
export type RefreshTokenMutationFn = Apollo.MutationFunction<RefreshTokenMutation, RefreshTokenMutationVariables>;

/**
 * __useRefreshTokenMutation__
 *
 * To run a mutation, you first call `useRefreshTokenMutation` within a React component and pass it any options that fit your needs.
 * When your component renders, `useRefreshTokenMutation` returns a tuple that includes:
 * - A mutate function that you can call at any time to execute the mutation
 * - An object with fields that represent the current status of the mutation's execution
 *
 * @param baseOptions options that will be passed into the mutation, supported options are listed on: https://www.apollographql.com/docs/react/api/react-hooks/#options-2;
 *
 * @example
 * const [refreshTokenMutation, { data, loading, error }] = useRefreshTokenMutation({
 *   variables: {
 *      input: // value for 'input'
 *   },
 * });
 */
export function useRefreshTokenMutation(baseOptions?: Apollo.MutationHookOptions<RefreshTokenMutation, RefreshTokenMutationVariables>) {
        const options = {...defaultOptions, ...baseOptions}
        return Apollo.useMutation<RefreshTokenMutation, RefreshTokenMutationVariables>(RefreshTokenDocument, options);
      }
export type RefreshTokenMutationHookResult = ReturnType<typeof useRefreshTokenMutation>;
export type RefreshTokenMutationResult = Apollo.MutationResult<RefreshTokenMutation>;
export type RefreshTokenMutationOptions = Apollo.BaseMutationOptions<RefreshTokenMutation, RefreshTokenMutationVariables>;
export const SigninDocument = gql`
    mutation Signin($input: SigninInput!) {
  signin(input: $input) {
    data
    success
    message
    statusCode
  }
}
    `;
export type SigninMutationFn = Apollo.MutationFunction<SigninMutation, SigninMutationVariables>;

/**
 * __useSigninMutation__
 *
 * To run a mutation, you first call `useSigninMutation` within a React component and pass it any options that fit your needs.
 * When your component renders, `useSigninMutation` returns a tuple that includes:
 * - A mutate function that you can call at any time to execute the mutation
 * - An object with fields that represent the current status of the mutation's execution
 *
 * @param baseOptions options that will be passed into the mutation, supported options are listed on: https://www.apollographql.com/docs/react/api/react-hooks/#options-2;
 *
 * @example
 * const [signinMutation, { data, loading, error }] = useSigninMutation({
 *   variables: {
 *      input: // value for 'input'
 *   },
 * });
 */
export function useSigninMutation(baseOptions?: Apollo.MutationHookOptions<SigninMutation, SigninMutationVariables>) {
        const options = {...defaultOptions, ...baseOptions}
        return Apollo.useMutation<SigninMutation, SigninMutationVariables>(SigninDocument, options);
      }
export type SigninMutationHookResult = ReturnType<typeof useSigninMutation>;
export type SigninMutationResult = Apollo.MutationResult<SigninMutation>;
export type SigninMutationOptions = Apollo.BaseMutationOptions<SigninMutation, SigninMutationVariables>;
export const LogoutDocument = gql`
    mutation Logout($input: RefreshTokenInput!) {
  signout(input: $input) {
    data
    success
    message
    statusCode
  }
}
    `;
export type LogoutMutationFn = Apollo.MutationFunction<LogoutMutation, LogoutMutationVariables>;

/**
 * __useLogoutMutation__
 *
 * To run a mutation, you first call `useLogoutMutation` within a React component and pass it any options that fit your needs.
 * When your component renders, `useLogoutMutation` returns a tuple that includes:
 * - A mutate function that you can call at any time to execute the mutation
 * - An object with fields that represent the current status of the mutation's execution
 *
 * @param baseOptions options that will be passed into the mutation, supported options are listed on: https://www.apollographql.com/docs/react/api/react-hooks/#options-2;
 *
 * @example
 * const [logoutMutation, { data, loading, error }] = useLogoutMutation({
 *   variables: {
 *      input: // value for 'input'
 *   },
 * });
 */
export function useLogoutMutation(baseOptions?: Apollo.MutationHookOptions<LogoutMutation, LogoutMutationVariables>) {
        const options = {...defaultOptions, ...baseOptions}
        return Apollo.useMutation<LogoutMutation, LogoutMutationVariables>(LogoutDocument, options);
      }
export type LogoutMutationHookResult = ReturnType<typeof useLogoutMutation>;
export type LogoutMutationResult = Apollo.MutationResult<LogoutMutation>;
export type LogoutMutationOptions = Apollo.BaseMutationOptions<LogoutMutation, LogoutMutationVariables>;
export const GetProfileDocument = gql`
    query GetProfile {
  userProfile {
    id
    userName
    email
  }
}
    `;

/**
 * __useGetProfileQuery__
 *
 * To run a query within a React component, call `useGetProfileQuery` and pass it any options that fit your needs.
 * When your component renders, `useGetProfileQuery` returns an object from Apollo Client that contains loading, error, and data properties
 * you can use to render your UI.
 *
 * @param baseOptions options that will be passed into the query, supported options are listed on: https://www.apollographql.com/docs/react/api/react-hooks/#options;
 *
 * @example
 * const { data, loading, error } = useGetProfileQuery({
 *   variables: {
 *   },
 * });
 */
export function useGetProfileQuery(baseOptions?: Apollo.QueryHookOptions<GetProfileQuery, GetProfileQueryVariables>) {
        const options = {...defaultOptions, ...baseOptions}
        return Apollo.useQuery<GetProfileQuery, GetProfileQueryVariables>(GetProfileDocument, options);
      }
export function useGetProfileLazyQuery(baseOptions?: Apollo.LazyQueryHookOptions<GetProfileQuery, GetProfileQueryVariables>) {
          const options = {...defaultOptions, ...baseOptions}
          return Apollo.useLazyQuery<GetProfileQuery, GetProfileQueryVariables>(GetProfileDocument, options);
        }
export function useGetProfileSuspenseQuery(baseOptions?: Apollo.SkipToken | Apollo.SuspenseQueryHookOptions<GetProfileQuery, GetProfileQueryVariables>) {
          const options = baseOptions === Apollo.skipToken ? baseOptions : {...defaultOptions, ...baseOptions}
          return Apollo.useSuspenseQuery<GetProfileQuery, GetProfileQueryVariables>(GetProfileDocument, options);
        }
export type GetProfileQueryHookResult = ReturnType<typeof useGetProfileQuery>;
export type GetProfileLazyQueryHookResult = ReturnType<typeof useGetProfileLazyQuery>;
export type GetProfileSuspenseQueryHookResult = ReturnType<typeof useGetProfileSuspenseQuery>;
export type GetProfileQueryResult = Apollo.QueryResult<GetProfileQuery, GetProfileQueryVariables>;