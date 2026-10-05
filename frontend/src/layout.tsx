import {Provider} from "./components/ui/provider.tsx";
import {QueryClientProvider} from "@tanstack/react-query";
import {ReactQueryDevtools} from "@tanstack/react-query-devtools";

import {Center, Flex, Separator, Theme} from "@chakra-ui/react";
import Header from "./components/shared/header.tsx";
import {AppThemeContext, ThemeContextType} from "./components/shared/theme-context.ts";
import React, {useState} from "react";
import {ThemeContext} from "@emotion/react";
import Footer from "./components/shared/footer.tsx";

import {queryClient} from "./components/shared/queryClient";

export default function Layout({children}: { children: React.ReactNode }) {
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
                    <div>&nbsp;</div>
                    <Header
                      theme={theme}
                      setTheme={(theme) => setTheme(theme)}
                    />
                    <div>&nbsp;</div>
                    <Separator/>
                    {children}
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
