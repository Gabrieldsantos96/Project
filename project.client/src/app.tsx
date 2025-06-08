import { RouterProvider } from "react-router-dom";
import { Providers } from "@/src/providers/providers";
import { router } from "@/src/routes";

export default function App() {
  return (
    <Providers>
      <RouterProvider router={router} />
    </Providers>
  );
}
