import {createRoot} from 'react-dom/client';
import './index.css';
import HomePage from './pages/home';
import AboutPage from './pages/about';
import {Provider} from "./components/ui/provider.tsx";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {ReactQueryDevtools} from "@tanstack/react-query-devtools";
import {createBrowserRouter} from "react-router";
import {RouterProvider} from "react-router/dom";

const queryClient = new QueryClient()

const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <Provider>
        <QueryClientProvider client={queryClient}>
          <HomePage/>
          <ReactQueryDevtools initialIsOpen={false}/>
        </QueryClientProvider>
      </Provider>
    ),
  },
  {
    path: "/about",
    element: (
      <Provider>
        <QueryClientProvider client={queryClient}>
          <AboutPage/>
          <ReactQueryDevtools initialIsOpen={false}/>
        </QueryClientProvider>
      </Provider>
    ),
  },
]);

const root = document.getElementById("root");

createRoot(root!).render(
  <RouterProvider router={router}/>
);
