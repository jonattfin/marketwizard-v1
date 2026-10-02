import { useContext } from "react";
import { AppThemeContext } from "./theme-context";

export const useNivoTheme = () => {
  const appTheme = useContext(AppThemeContext);
  if (appTheme === "dark") {
    return {
      tooltip: {
        container: {
          background: "#000000",
        },
      },
    };
  }

  return {
    tooltip: {
      container: {
        background: "#ffffff",
      },
    },
  };
};
