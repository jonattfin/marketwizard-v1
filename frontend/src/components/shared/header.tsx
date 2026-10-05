import {
  useDisclosure,
  Drawer,
  Portal,
  CloseButton,
  Icon,
  Box,
  Flex,
  Switch,
  HStack,
  VStack,
  Button,
} from "@chakra-ui/react";

import {LuMenu} from "react-icons/lu";
import {FaMoon, FaSun} from "react-icons/fa";
import {ThemeContextType} from "./theme-context";
import {Link} from "react-router";
import {Logo} from "./logo";

const links = [
  {name: "Home", href: "/"},
  // {name: "Investor sentiment", href: "/sentiment"},
  // {name: "Maps", href: "/maps"},
  // {name: "Lazy Portfolios", href: "/lazy-portfolios"},
  // // {name: "User Portfolios", href: "/portfolios"},
  // {name: "ETFs", href: "/etfs"},
  {name: "Watchlist", href: "/watchlist"},
];

const MenuLinks = ({isMobile = false}) => {
  const LinkComponent = isMobile ? VStack : HStack;

  return (
    <LinkComponent gap={isMobile ? 4 : 8} align="center">
      {links.map((link) => (
        <Link key={link.name} to={link.href}>
          {link.name}
        </Link>
      ))}
    </LinkComponent>
  );
};

const MobileDrawer = () => {
  const {open, onToggle} = useDisclosure();

  return (
    <Drawer.Root open={open} onOpenChange={onToggle} size="full">
      <Drawer.Trigger asChild>
        <Button variant="outline" size="sm">
          <Icon color="blue.600">
            <LuMenu/>
          </Icon>
        </Button>
      </Drawer.Trigger>
      <Portal>
        <Drawer.Backdrop/>
        <Drawer.Positioner>
          <Drawer.Content>
            <Drawer.Header>
              <Drawer.Title>
                <Logo/>
              </Drawer.Title>
            </Drawer.Header>
            <Drawer.Body>
              <MenuLinks isMobile/>
            </Drawer.Body>
            <Drawer.CloseTrigger asChild>
              <CloseButton size="md"/>
            </Drawer.CloseTrigger>
          </Drawer.Content>
        </Drawer.Positioner>
      </Portal>
    </Drawer.Root>
  );
};

export type HamburgerMenuType = {
  theme: string;
  setTheme: (theme: ThemeContextType) => void;
};

function HamburgerMenu({theme, setTheme}: Readonly<HamburgerMenuType>) {
  const renderThemeSwitch = () => {
    return (
      <Switch.Root
        colorPalette="orange"
        checked={theme === "dark"}
        onCheckedChange={(e) => {
          setTheme(e.checked ? "dark" : "light");
        }}
      >
        <Switch.HiddenInput/>
        <Switch.Control>
          <Switch.Thumb/>
          <Switch.Indicator fallback={<Icon as={FaMoon} color="gray.400"/>}>
            <Icon as={FaSun} color="yellow.400"/>
          </Switch.Indicator>
        </Switch.Control>
      </Switch.Root>
    );
  };

  return (
    <div>
      <Flex
        as="nav"
        align="center"
        justify="space-between"
        wrap="wrap"
        gap={{base: 8, lg: 16}}
        mx="auto"
      >
        {/* Desktop Menu */}
        <Box display={{base: "none", md: "block"}}>
          <MenuLinks/>
        </Box>

        {renderThemeSwitch()}

        {/* Mobile Drawer */}
        <Box display={{base: "block", md: "none"}}>
          <MobileDrawer/>
        </Box>
      </Flex>
    </div>
  );
}

export type HeaderMenuType = {
  theme: string;
  setTheme: (theme: ThemeContextType) => void;
};

export default function Header({theme, setTheme}: Readonly<HeaderMenuType>) {
  return (
    <>
      <Flex justify="space-between">
        <Logo/>
        <HamburgerMenu theme={theme} setTheme={setTheme}/>
      </Flex>
    </>
  );
}
