import { Icon, Table } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { useContext } from "react";
import { LuInfo } from "react-icons/lu";

// import { Tooltip } from "@/components/ui/tooltip";

import { lastUpdatedAt } from "../../../components/shared/helpers";
import Loading from "../../../components/shared/loading";
import type { TopNewsDataType } from "../../../components/shared/types";
import { CountryContext } from "../../../components/shared/country-context";

const useTopNews = () => {
  const countries = useContext(CountryContext);

  return useQuery<TopNewsDataType>({
    queryKey: ["top-news", countries],
    queryFn: async () => {
      const response = await fetch(
        `api/top-news?countries=${encodeURIComponent(JSON.stringify(countries))}`,
      );
      return await response.json();
    },
  });
};

const News = () => {
  const { isPending, error, data } = useTopNews();

  if (isPending) return <Loading />;
  if (error) return `Page ${error}`;

  return (
    <>
      <Table.Root size={"sm"}>
        <Table.Header>
          <Table.Row>
            <Table.ColumnHeader>Headline</Table.ColumnHeader>
            <Table.ColumnHeader>Source</Table.ColumnHeader>
            {/*<Table.ColumnHeader textAlign="end">More info</Table.ColumnHeader>*/}
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {data?.items?.map((item, index) => (
            <Table.Row key={item.id}>
              <Table.Cell>
                {`${index + 1}. ${item.text}`}{" "}
                {/*<Tooltip content={item.description}>*/}
                  <Icon fontSize="sm">
                    <LuInfo />
                  </Icon>
                {/*</Tooltip>*/}
              </Table.Cell>
              <Table.Cell>{item.source}</Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table.Root>
      {lastUpdatedAt(data?.date)}
    </>
  );
};

export default News;
