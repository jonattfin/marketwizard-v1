import {NavLink} from "react-router";
import {Center, Tabs} from "@chakra-ui/react"
import {LuFolder, LuUser} from "react-icons/lu"

export function MyAppNav() {
  return (
    <nav>
      <Center>
        <Tabs.Root defaultValue="members">
          <Tabs.List>
            <Tabs.Trigger value="members">
              <LuUser/>
              Members
            </Tabs.Trigger>
            <Tabs.Trigger value="projects">
              <LuFolder/>
              Projects
            </Tabs.Trigger>
          </Tabs.List>
          <Tabs.Content value="members">
            <NavLink to="/" end>
              Home
            </NavLink>
          </Tabs.Content>
          <Tabs.Content value="projects">
            <NavLink to="/about" end>
              About
            </NavLink>
          </Tabs.Content>
        </Tabs.Root>
      </Center>
    </nav>
  );
}
