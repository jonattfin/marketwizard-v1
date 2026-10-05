import {createRoot} from 'react-dom/client';
import './index.css';
import HomePage from './pages/home';
import {Provider} from "./components/ui/provider.tsx";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {ReactQueryDevtools} from "@tanstack/react-query-devtools";
import {createBrowserRouter} from "react-router";
import {RouterProvider} from "react-router/dom";
import {Center, Flex, Separator, Theme} from "@chakra-ui/react";
import Header from "./components/shared/header.tsx";
import {AppThemeContext, ThemeContextType} from "./components/shared/theme-context.ts";
import {useState} from "react";
import {ThemeContext} from "@emotion/react";
import Footer from "./components/shared/footer.tsx";

const queryClient = new QueryClient()

function Layout() {
  const [theme, setTheme] = useState<ThemeContextType>("light");

  return (
    <Provider>
      <AppThemeContext.Provider value={theme}>
        <ThemeContext value={theme}>
          <Theme appearance={theme}>
            <Flex
              gap="10"
              direction="column"
              justify="flex-start"
              minHeight={"100vh"}
            >
              <Center>
                <Flex width={"80%"} direction={"column"}>
                  <QueryClientProvider client={queryClient}>
                    <Header
                      theme={theme}
                      setTheme={(theme) => setTheme(theme)}
                    />
                    <Separator/>
                    <HomePage/>
                    <ReactQueryDevtools initialIsOpen={false}/>
                  </QueryClientProvider>
                  <div>&nbsp;</div>
                  <Separator/>
                  <Footer/>
                </Flex>

              </Center>
            </Flex>

          </Theme>
        </ThemeContext>
      </AppThemeContext.Provider>
    </Provider>
  )
}

const router = createBrowserRouter([
  {
    path: "/",
    element: (<Layout/>),
  },
]);

const root = document.getElementById("root");

createRoot(root!).render(
  <RouterProvider router={router}/>
);
