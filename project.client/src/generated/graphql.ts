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

export type Mutations = {
  __typename?: 'Mutations';
  authenticateUser: MutationResult;
  changeJobRole: MutationResult;
  changePassword: MutationResult;
  createAccount: UserProfileDto;
  logout: MutationResult;
  resetPassword: MutationResult;
  resetPasswordRequest: MutationResult;
};


export type MutationsAuthenticateUserArgs = {
  input: UserAuthenticationInput;
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


export type MutationsResetPasswordArgs = {
  input: ResetPasswordInput;
};


export type MutationsResetPasswordRequestArgs = {
  input: ResetPasswordRequestInput;
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

export type ResetPasswordInput = {
  confirmPassword: Scalars['String']['input'];
  encodedEmail: Scalars['String']['input'];
  encodedToken: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type ResetPasswordRequestInput = {
  email: Scalars['String']['input'];
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

export type UserAuthenticationInput = {
  email: Scalars['String']['input'];
  password: Scalars['String']['input'];
};

export type UserProfileDto = {
  __typename?: 'UserProfileDto';
  email: Scalars['String']['output'];
  employeeDtos: Array<EmployeeDto>;
  employeeSelectedRefId?: Maybe<Scalars['UUID']['output']>;
  id: Scalars['UUID']['output'];
  userName: Scalars['String']['output'];
};

export type Query1QueryVariables = Exact<{ [key: string]: never; }>;


export type Query1Query = { __typename?: 'Queries', userProfile: { __typename?: 'UserProfileDto', id: any, email: string, userName: string, employeeSelectedRefId?: any | null, employeeDtos: Array<{ __typename?: 'EmployeeDto', id: any, tenantId: any, name: string, badgeDtos: Array<BadgeType> }> } };


export const Query1Document = gql`
    query Query1 {
  userProfile {
    id
    email
    userName
    employeeSelectedRefId
    employeeDtos {
      id
      tenantId
      name
      badgeDtos
    }
  }
}
    `;

/**
 * __useQuery1Query__
 *
 * To run a query within a React component, call `useQuery1Query` and pass it any options that fit your needs.
 * When your component renders, `useQuery1Query` returns an object from Apollo Client that contains loading, error, and data properties
 * you can use to render your UI.
 *
 * @param baseOptions options that will be passed into the query, supported options are listed on: https://www.apollographql.com/docs/react/api/react-hooks/#options;
 *
 * @example
 * const { data, loading, error } = useQuery1Query({
 *   variables: {
 *   },
 * });
 */
export function useQuery1Query(baseOptions?: Apollo.QueryHookOptions<Query1Query, Query1QueryVariables>) {
        const options = {...defaultOptions, ...baseOptions}
        return Apollo.useQuery<Query1Query, Query1QueryVariables>(Query1Document, options);
      }
export function useQuery1LazyQuery(baseOptions?: Apollo.LazyQueryHookOptions<Query1Query, Query1QueryVariables>) {
          const options = {...defaultOptions, ...baseOptions}
          return Apollo.useLazyQuery<Query1Query, Query1QueryVariables>(Query1Document, options);
        }
export function useQuery1SuspenseQuery(baseOptions?: Apollo.SkipToken | Apollo.SuspenseQueryHookOptions<Query1Query, Query1QueryVariables>) {
          const options = baseOptions === Apollo.skipToken ? baseOptions : {...defaultOptions, ...baseOptions}
          return Apollo.useSuspenseQuery<Query1Query, Query1QueryVariables>(Query1Document, options);
        }
export type Query1QueryHookResult = ReturnType<typeof useQuery1Query>;
export type Query1LazyQueryHookResult = ReturnType<typeof useQuery1LazyQuery>;
export type Query1SuspenseQueryHookResult = ReturnType<typeof useQuery1SuspenseQuery>;
export type Query1QueryResult = Apollo.QueryResult<Query1Query, Query1QueryVariables>;