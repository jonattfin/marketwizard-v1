import {
  Flex,
  FormatNumber,
  Icon,
  Splitter,
  Table,
  Tag,
  useBreakpointValue,
} from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { SectorPerformanceDataType, SectorType } from "../../../components/shared/types";
import Loading from "../../../components/shared/loading";
import { drop, take } from "es-toolkit";
import { useContext, useMemo } from "react";
import {
  LuActivity,
  LuAxis3D,
  LuBanknote,
  LuArchive,
  LuAtom,
  LuAperture,
  LuApple,
  LuAnvil,
  LuAntenna,
  LuAward,
  LuAmbulance,
  LuBolt,
} from "react-icons/lu";
import { CountryContext } from "../../../components/shared/country-context";
import { lastUpdatedAt } from "../../../components/shared/helpers";

const useSectorPerformance = () => {
  const countries = useContext(CountryContext);

  return useQuery<SectorPerformanceDataType>({
    queryKey: ["sector-performance", countries],
    queryFn: async () => {
      const response = await fetch(
        `/api/sector-performance?countries=${encodeURIComponent(JSON.stringify(countries))}`,
      );
      return await response.json();
    },
  });
};

const SectorPerformanceTable = () => {
  const { isPending, error, data } = useSectorPerformance();

  const orientation = useBreakpointValue<"horizontal" | "vertical">({
    base: "vertical",
    md: "horizontal",
  });

  const minH = orientation === "horizontal" ? 60 : 700;

  const arrays = useMemo(() => {
    const midpoint = Math.ceil((data?.items?.length || 0) / 2);
    const leftArray = take(data?.items || [], midpoint);
    const rightArray = drop(data?.items || [], midpoint);

    return { leftArray, rightArray };
  }, [data]);

  if (isPending) return <Loading />;
  if (error) return `Page ${error}`;

  return (
    <>
      <Flex gap="4" justify={"space-evenly"}>
        <Splitter.Root
          panels={[{ id: "a" }, { id: "b" }]}
          orientation={orientation}
          minH={minH}
        >
          <Splitter.Panel id="a">
            <SectorPerformance items={arrays.leftArray} date={data?.date} />
          </Splitter.Panel>
          <Splitter.ResizeTrigger id="a:b" />
          <Splitter.Panel id="b">
            <SectorPerformance items={arrays.rightArray} date={data?.date} />
          </Splitter.Panel>
        </Splitter.Root>
      </Flex>
      {lastUpdatedAt(data?.date)}
    </>
  );
};

const SectorPerformance = ({ items }: SectorPerformanceDataType) => {
  return (
    <Table.Root>
      <Table.Header>
        <Table.Row>
          <Table.ColumnHeader>Sector</Table.ColumnHeader>
          <Table.ColumnHeader textAlign="end">Change</Table.ColumnHeader>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        {items.map((item) => (
          <Table.Row key={item.type}>
            <Table.Cell>
              <Icon fontSize="lg" color="fg.subtle">
                {renderSectorIcon(item.type)}
              </Icon>
              {` ${item.type}`}
            </Table.Cell>
            <Table.Cell textAlign="end">
              <Tag.Root
                size="md"
                variant={"subtle"}
                colorPalette={item.change > 0 ? "green" : "orange"}
              >
                <Tag.Label>
                  <FormatNumber
                    value={item.change}
                    style="percent"
                    maximumFractionDigits={2}
                    minimumFractionDigits={2}
                  />
                </Tag.Label>
              </Tag.Root>
            </Table.Cell>
          </Table.Row>
        ))}
      </Table.Body>
    </Table.Root>
  );
};

const iconsMap = {
  [SectorType.Telecom]: <LuAntenna />,
  [SectorType.Energy]: <LuBanknote />,
  [SectorType.Financials]: <LuAxis3D />,
  [SectorType.Industrials]: <LuArchive />,
  [SectorType.Materials]: <LuAtom />,
  [SectorType.Utilities]: <LuAperture />,
  [SectorType.BasicMaterials]: <LuAnvil />,
  [SectorType.ConsumerGoods]: <LuActivity />,
  [SectorType.HealthCare]: <LuAmbulance />,
  [SectorType.ConsumerServices]: <LuAward />,
  [SectorType.CustomerStaples]: <LuApple />,
  [SectorType.Technology]: <LuBolt />,
};

function renderSectorIcon(sector: SectorType) {
  return iconsMap[sector] ?? <div>&nbsp;</div>;
}

export default SectorPerformanceTable;
