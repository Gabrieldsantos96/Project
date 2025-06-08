import { createBrowserRouter } from "react-router-dom";
import { RootLayout } from "@/src/components/root-layout";

const router = createBrowserRouter([
  {
    path: "/",
    element: <RootLayout />,
    children: [
      {
        path: "/",
        element: <div />,
      },
    ],
  },
  {
    path: "*",
    element: <div />,
  },
]);

export { router };
