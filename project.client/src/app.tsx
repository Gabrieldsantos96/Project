import { RouterProvider } from "react-router-dom";
import { Providers } from "@/providers/providers";
import { router } from "@/routes";

export default function App() {
  return (
    <Providers>
      <RouterProvider router={router} />
    </Providers>
  );
}
