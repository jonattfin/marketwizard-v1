import Loading from "../../../components/shared/loading";
import { Grid, GridItem } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import type { IndicePerformanceDataType } from "../../../components/shared/types";

import React, { Suspense, useContext } from "react";
import { CountryContext } from "../../../components/shared/country-context";
import { IndicesTable } from "../../../components/shared/indices-table";

const WorldMap = React.lazy(
  () => import("./world-map"),
);

export const useIndicesPerformance = () => {
  return useQuery<IndicePerformanceDataType>({
    queryKey: ["indices"],
    queryFn: async () => {
      const response = await fetch("/api/indices");
      return await response.json();
    },
  });
};

type IndicesPerformanceType = {
  onCountryChanged: (countryCode: string, checked: boolean) => void;
};

const IndicesPerformance = ({ onCountryChanged }: IndicesPerformanceType) => {
  const { isPending, error, data } = useIndicesPerformance();
  const countries = useContext(CountryContext);

  if (isPending) return <Loading />;
  if (error) return `Page ${error}`;

  const selectedIndices = data?.items
    ?.filter((item) => countries.includes(item.countryCode))
    .map((item) => {
      return { id: item.countryCode, value: item.regularMarketPrice };
    });

  return (
    <Grid
      templateColumns={{
        base: "1fr",
        md: "repeat(1, 1fr)",
        lg: "repeat(4, 1fr)",
      }}
      gap="6"
    >
      <GridItem colSpan={3}>
        <Suspense fallback={<Loading />}>
          <WorldMap data={selectedIndices}></WorldMap>
        </Suspense>
      </GridItem>
      <GridItem>
        <IndicesTable {...{ data, countries, onCountryChanged }}></IndicesTable>
      </GridItem>
    </Grid>
  );
};

export default IndicesPerformance;
