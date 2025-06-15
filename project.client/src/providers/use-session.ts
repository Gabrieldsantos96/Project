import { useContext } from "react";
import { SessionContext } from "./session-provider";

export function useSession() {
  const authProvider = useContext(SessionContext);

  return authProvider;
}
