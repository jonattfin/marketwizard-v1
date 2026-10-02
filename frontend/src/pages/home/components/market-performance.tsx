import {
  Grid,
  GridItem,
  Icon,
  Stack,
  Tabs,
  Splitter,
  useBreakpointValue,
} from "@chakra-ui/react";
import {
  TopGainers,
  TopIndustries,
  TopLosers,
  WorstIndusties,
} from "./top-gainers";
import { LuZap, LuZapOff } from "react-icons/lu";

const MarketPerformance = () => {
  const orientation = useBreakpointValue<"horizontal" | "vertical">({
    base: "vertical",
    md: "horizontal",
  });

  const minH = orientation === "horizontal" ? 60 : 650;

  return (
    <Grid gap="6">
      <GridItem>
        <Splitter.Root
          panels={[{ id: "a" }, { id: "b" }]}
          orientation={orientation}
          minH={minH}
        >
          <Splitter.Panel id="a">
            <Tabs.Root lazyMount unmountOnExit defaultValue="tab-1">
              <Tabs.List>
                <Tabs.Trigger value="tab-1">
                  <Stack direction={"row"}>
                    <Icon fontSize="lg" color="orange.300">
                      <LuZap />
                    </Icon>
                    Top Gainers
                  </Stack>
                </Tabs.Trigger>
                <Tabs.Trigger value="tab-2">
                  <Stack direction={"row"}>
                    <Icon fontSize="lg" color="red.300">
                      <LuZapOff />
                    </Icon>
                    Top Losers
                  </Stack>
                </Tabs.Trigger>
              </Tabs.List>
              <Tabs.Content value="tab-1">
                <Tabs.Root lazyMount unmountOnExit defaultValue="tab-1">
                  <Tabs.List>
                    <Tabs.Trigger value="tab-1">7D</Tabs.Trigger>
                    <Tabs.Trigger value="tab-2">1M</Tabs.Trigger>
                    <Tabs.Trigger value="tab-3">3M</Tabs.Trigger>
                    <Tabs.Trigger value="tab-4">1Y</Tabs.Trigger>
                    <Tabs.Trigger value="tab-5">3Y</Tabs.Trigger>
                  </Tabs.List>
                  <Tabs.Content value="tab-1">
                    <TopGainers period={1} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-2">
                    <TopGainers period={2} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-3">
                    <TopGainers period={3} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-4">
                    <TopGainers period={4} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-5">
                    <TopGainers period={5} />
                  </Tabs.Content>
                </Tabs.Root>
              </Tabs.Content>
              <Tabs.Content value="tab-2">
                <Tabs.Root lazyMount unmountOnExit defaultValue="tab-1">
                  <Tabs.List>
                    <Tabs.Trigger value="tab-1">7D</Tabs.Trigger>
                    <Tabs.Trigger value="tab-2">1M</Tabs.Trigger>
                    <Tabs.Trigger value="tab-3">3M</Tabs.Trigger>
                    <Tabs.Trigger value="tab-4">1Y</Tabs.Trigger>
                    <Tabs.Trigger value="tab-5">3Y</Tabs.Trigger>
                  </Tabs.List>
                  <Tabs.Content value="tab-1">
                    <TopLosers period={1} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-2">
                    <TopLosers period={2} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-3">
                    <TopLosers period={3} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-4">
                    <TopLosers period={4} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-5">
                    <TopLosers period={5} />
                  </Tabs.Content>
                </Tabs.Root>
              </Tabs.Content>
            </Tabs.Root>
          </Splitter.Panel>
          <Splitter.ResizeTrigger id="a:b" />
          <Splitter.Panel id="b">
            <Tabs.Root lazyMount unmountOnExit defaultValue="tab-1">
              <Tabs.List>
                <Tabs.Trigger value="tab-1">
                  <Stack direction={"row"}>
                    <Icon fontSize="lg" color="orange.300">
                      <LuZap />
                    </Icon>
                    Top Industries
                  </Stack>
                </Tabs.Trigger>
                <Tabs.Trigger value="tab-2">
                  <Stack direction={"row"}>
                    <Icon fontSize="lg" color="red.300">
                      <LuZapOff />
                    </Icon>
                    Worst Industries
                  </Stack>
                </Tabs.Trigger>
              </Tabs.List>
              <Tabs.Content value="tab-1">
                <Tabs.Root lazyMount unmountOnExit defaultValue="tab-1">
                  <Tabs.List>
                    <Tabs.Trigger value="tab-1">7D</Tabs.Trigger>
                    <Tabs.Trigger value="tab-2">1M</Tabs.Trigger>
                    <Tabs.Trigger value="tab-3">3M</Tabs.Trigger>
                    <Tabs.Trigger value="tab-4">1Y</Tabs.Trigger>
                    <Tabs.Trigger value="tab-5">3Y</Tabs.Trigger>
                  </Tabs.List>
                  <Tabs.Content value="tab-1">
                    <TopIndustries period={1} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-2">
                    <TopIndustries period={2} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-3">
                    <TopIndustries period={3} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-4">
                    <TopIndustries period={4} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-5">
                    <TopIndustries period={5} />
                  </Tabs.Content>
                </Tabs.Root>
              </Tabs.Content>
              <Tabs.Content value="tab-2">
                <Tabs.Root lazyMount unmountOnExit defaultValue="tab-1">
                  <Tabs.List>
                    <Tabs.Trigger value="tab-1">7D</Tabs.Trigger>
                    <Tabs.Trigger value="tab-2">1M</Tabs.Trigger>
                    <Tabs.Trigger value="tab-3">3M</Tabs.Trigger>
                    <Tabs.Trigger value="tab-4">1Y</Tabs.Trigger>
                    <Tabs.Trigger value="tab-5">3Y</Tabs.Trigger>
                  </Tabs.List>
                  <Tabs.Content value="tab-1">
                    <WorstIndusties period={1} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-2">
                    <WorstIndusties period={2} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-3">
                    <WorstIndusties period={3} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-4">
                    <WorstIndusties period={4} />
                  </Tabs.Content>
                  <Tabs.Content value="tab-5">
                    <WorstIndusties period={5} />
                  </Tabs.Content>
                </Tabs.Root>
              </Tabs.Content>
            </Tabs.Root>
          </Splitter.Panel>
        </Splitter.Root>
      </GridItem>
    </Grid>
  );
};

export default MarketPerformance;
