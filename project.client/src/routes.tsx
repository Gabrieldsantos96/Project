import { createBrowserRouter } from "react-router-dom";
import { RootLayout } from "@/components/root-layout";
import { Home } from "./views/home";

const router = createBrowserRouter([
  {
    path: "/",
    element: <RootLayout />,
    children: [
      {
        path: "/",
        element: <Home />,
      },
    ],
  },
  {
    path: "*",
    element: <div />,
  },
]);

export { router };
