import { GraphQLClient as Client } from "graphql-request";
import type { RequestDocument, Variables } from "graphql-request";

export class GraphQLClient {
  private client: Client;

  constructor(baseUrl: string) {
    this.client = new Client(baseUrl, {
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
    });
  }

  async query<TData>(document: RequestDocument): Promise<TData> {
    return this.client.request<TData>(document);
  }

  async mutation<TData = any, TVariables extends Variables = Variables>(
    document: RequestDocument,
    variables?: TVariables
  ): Promise<TData> {
    return this.client.request<TData>(document, variables);
  }
}

export const graphQLClient = new GraphQLClient("bla");
