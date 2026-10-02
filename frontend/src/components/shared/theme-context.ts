import { createContext } from "react";

export type ThemeContextType = "light" | "dark";

export const AppThemeContext = createContext<ThemeContextType>("light");
