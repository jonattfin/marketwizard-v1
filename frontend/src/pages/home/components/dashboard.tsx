import { Accordion, Flex, Grid, GridItem, Icon } from "@chakra-ui/react";
import { LuActivity, LuAxis3D, LuScale, LuNewspaper } from "react-icons/lu";
import type {ReactNode} from "react";

import MarketPerformance from "./market-performance";
import IndicesPerformance from "./indices-performance";
import SectorPerformance from "./sector-performance";
import News from "./news";

import { useCountries } from "../../../components/shared/useCountries";
import { CountryContext } from "../../../components/shared/country-context";

export default function Dashboard() {
  const { countries, onCountryChanged } = useCountries();

  const leftItems = getLeftItems();
  const rightItems = getRightItems();

  return (
    <CountryContext.Provider value={countries}>
      <IndicesPerformance onCountryChanged={onCountryChanged} />

      <div>&nbsp;</div>
      <Grid
        templateColumns={{
          base: "1fr",
          md: "repeat(1, 1fr)",
          lg: "repeat(4, 1fr)",
        }}
        gap="6"
      >
        <GridItem colSpan={3}>
          <Accordion.Root
            multiple
            defaultValue={leftItems.map((item) => item.value)}
            size={"md"}
          >
            {leftItems.map((item) => (
              <Accordion.Item key={item.value} value={item.value}>
                <Accordion.ItemTrigger>
                  <Icon fontSize="lg" color="orange.300">
                    {item.icon}
                  </Icon>
                  {item.title}
                  <Accordion.ItemIndicator />
                </Accordion.ItemTrigger>
                <Accordion.ItemContent>
                  <Accordion.ItemBody>
                    {item.content}
                    <div>&nbsp;</div>
                  </Accordion.ItemBody>
                </Accordion.ItemContent>
              </Accordion.Item>
            ))}
          </Accordion.Root>
        </GridItem>

        <GridItem>
          <Flex gap="4" direction="column">
            <Accordion.Root
              multiple
              defaultValue={rightItems.map((item) => item.value)}
              size={"md"}
            >
              {rightItems.map((item) => (
                <Accordion.Item key={item.value} value={item.value}>
                  <Accordion.ItemTrigger>
                    <Icon fontSize="lg" color="orange.300">
                      {item.icon}
                    </Icon>
                    {item.title}
                    <Accordion.ItemIndicator />
                  </Accordion.ItemTrigger>
                  <Accordion.ItemContent>
                    <Accordion.ItemBody>
                      {item.content}
                      <div>&nbsp;</div>
                    </Accordion.ItemBody>
                  </Accordion.ItemContent>
                </Accordion.Item>
              ))}
            </Accordion.Root>
          </Flex>
        </GridItem>
      </Grid>
      </CountryContext.Provider>
  );
}

type ItemType = {
  value: string;
  icon: ReactNode;
  title: string;
  content: ReactNode;
}


function getLeftItems(): ItemType[] {
  return [
    {
      value: "performance-by-sector",
      icon: <LuAxis3D />,
      title: "Today's performance by sector",
      content: <SectorPerformance />,
    },
    {
      value: "top-news",
      icon: <LuActivity />,
      title: "Today's top news",
      content: <News />,
    },
    {
      value: "top-gainers-industries",
      icon: <LuScale />,
      title: "Market performance",
      content: <MarketPerformance/>,
    },
  ];
}

function getRightItems(): ItemType[] {
  return [
    // {
    //   value: "tree-maps",
    //   icon: <LuFolderSearch/>,
    //   title: "Map",
    //   content: <MarketTreemap height={300}/>
    // },
    {
      value: "bloomberg-news",
      icon: <LuNewspaper />,
      title: "Bloomberg news",
      content: <>&nbsp;</>,
    },
  ];
}
