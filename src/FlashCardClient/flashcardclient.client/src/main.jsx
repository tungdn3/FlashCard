import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import 'bootstrap-icons/font/bootstrap-icons.css';
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Root, { loader as decksLoader, action as rootAction } from "./routes/root";
import ErrorPage from "./error-page";
import AuthProvider from "./context/AuthContext";
import Index from "./routes";
import Deck, { loader as deckLoader, action as deckAction } from "./routes/deck";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
    errorElement: <ErrorPage />,
    loader: decksLoader,
    action: rootAction,
    children: [
      {
        errorElement: <ErrorPage />,
        children: [
          { index: true, element: <Index /> },
          {
            path: "decks/:deckId",
            element: <Deck />,
            loader: deckLoader,
            action: deckAction,
          },
        ],
      },
    ],
  },
]);

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  </StrictMode>
);
