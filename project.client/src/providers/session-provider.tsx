import { createContext, useCallback, useEffect, useState } from "react";
import type { IUser } from "@/models";
import { LaunchScreen } from "@/components/launch-screen";
import { Key } from "@/constants/consts";
import { useQuery } from "@tanstack/react-query";
import { reactQueryClient } from "../libs/react-query-client";

interface ISessionContext {
  signedIn: boolean;
  user: IUser | null;
  signin(): void;
  signout(): void;
}

export const SessionContext = createContext({} as ISessionContext);

const fetchUserProfile = async (): Promise<IUser> => {
  const response = await fetch("/api/auth/me", {
    credentials: "include",
  });
  if (!response.ok) {
    throw new Error("Sessão inválida");
  }
  return response.json();
};

export function SessionProvider({ children }: { children: React.ReactNode }) {
  const [signedIn, setSignedIn] = useState<boolean>(false);

  const { isError, isFetching, isSuccess, data } = useQuery({
    queryKey: [Key.Profile],
    queryFn: fetchUserProfile,
    enabled: true,
    staleTime: Infinity,
    retry: false,
  });

  const signin = useCallback(() => {
    window.location.href = "/api/auth/login";
  }, []);

  const signout = useCallback(async () => {
    try {
      await fetch("/api/auth/logout", {
        method: "POST",
        credentials: "include",
      });
      reactQueryClient.removeQueries({ queryKey: [Key.Profile] });
      setSignedIn(false);
    } catch (error) {
      console.error("Erro ao fazer logout:", error);
    }
  }, []);

  useEffect(() => {
    if (isSuccess) {
      setSignedIn(true);
      return;
    }
    if (isError) {
      alert("Sessão expirou ou inválida");
      setSignedIn(false);
      reactQueryClient.removeQueries({ queryKey: [Key.Profile] });
    }
  }, [isSuccess, isError]);

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
