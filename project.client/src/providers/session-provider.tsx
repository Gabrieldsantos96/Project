import { createContext, useCallback, useEffect, useState } from "react";
import type { IUser } from "@/src/models";
import { LaunchScreen } from "@/src/components/launch-screen";
import { Key } from "@/src/constants/consts";
import { useQuery } from "@tanstack/react-query";
import { reactQueryClient } from "../libs/react-query-client";

interface ISessionContext {
  signedIn: boolean;
  user: IUser | null;
  signin(accessToken: string): void;
  signout(): void;
}

export const SessionContext = createContext({} as ISessionContext);

export function SessionProvider({ children }: { children: React.ReactNode }) {
  const [signedIn, setSignedIn] = useState<boolean>(() => {
    const storedAccessToken = localStorage.getItem(Key.AccessToken);

    return !!storedAccessToken;
  });

  const { isError, isFetching, isSuccess, data } = useQuery({
    queryKey: [Key.Profile],
    queryFn: () => {},
    enabled: signedIn,
    staleTime: Infinity,
  });

  const signin = useCallback((accessToken: string) => {
    localStorage.setItem(Key.AccessToken, accessToken);

    setSignedIn(true);
  }, []);

  const signout = useCallback(() => {
    localStorage.removeItem(Key.AccessToken);
    reactQueryClient.removeQueries({ queryKey: [Key.Profile] });

    setSignedIn(false);
  }, []);

  useEffect(() => {
    if (isError) {
      alert("Sessão expirou");
      signout();
    }
  }, [isError, signout]);

  return (
    <SessionContext.Provider
      value={{
        signedIn: isSuccess && signedIn,
        user: data ?? null,
        signin,
        signout,
      }}
    >
      <LaunchScreen isLoading={isFetching} />

      {!isFetching && children}
    </SessionContext.Provider>
  );
}
