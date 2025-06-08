import type { PropsWithChildren } from "react";
import { SessionProvider } from "./session-provider";
import { QueryClientProvider } from "@tanstack/react-query";
import { reactQueryClient } from "@/src/libs/react-query-client";

export function Providers({ children }: PropsWithChildren) {
  return (
    <QueryClientProvider client={reactQueryClient}>
      <SessionProvider>{children}</SessionProvider>
    </QueryClientProvider>
  );
}
