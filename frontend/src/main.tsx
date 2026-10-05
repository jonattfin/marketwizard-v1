import {createRoot} from 'react-dom/client';
import './index.css';
import HomePage from './pages/home';
import WatchPage from './pages/watchlist';
import Layout from './layout';

import {createBrowserRouter} from "react-router";
import {RouterProvider} from "react-router/dom";

const router = createBrowserRouter([
  {
    path: "/",
    element: (<Layout children={<HomePage/>}/>),
  },
  {
    path: "/watchlist",
    element: (<Layout children={<WatchPage/>}/>),
  },
]);

const root = document.getElementById("root");

createRoot(root!).render(
  <RouterProvider router={router}/>
);
