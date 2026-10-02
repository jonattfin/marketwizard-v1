"use client";

import { FormatNumber, Switch, Table, Tag } from "@chakra-ui/react";
import type { IndicePerformanceDataType } from "./types";
import { HiCheck, HiX } from "react-icons/hi";
import { lastUpdatedAt } from "./helpers";

export type IndiceTableType = {
  data: IndicePerformanceDataType | undefined;
  countries: string[];
  onCountryChanged: (countryCode: string, checked: boolean) => void;
};

export const IndicesTable = ({
  data,
  countries,
  onCountryChanged,
}: Readonly<IndiceTableType>) => {
  return (
    <>
      <Table.Root size="sm" interactive>
        <Table.Header>
          <Table.Row>
            <Table.ColumnHeader>Indice</Table.ColumnHeader>
            <Table.ColumnHeader>Chg%</Table.ColumnHeader>
            <Table.ColumnHeader>Points</Table.ColumnHeader>
            <Table.ColumnHeader textAlign="end">Off/On</Table.ColumnHeader>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {data?.items?.map((item) => (
            <Table.Row key={item.id}>
              <Table.Cell>{item.name}</Table.Cell>
              <Table.Cell>
                <Tag.Root size="md" variant={"subtle"} colorPalette={item.regularMarketChangePercent > 0 ? "green" : "orange"}>
                  <Tag.Label><FormatNumber
                    value={item.regularMarketChangePercent}
                    style="percent"
                    maximumFractionDigits={2}
                    minimumFractionDigits={2}
                  /></Tag.Label>
                </Tag.Root>
              </Table.Cell>
              <Table.Cell>
                <FormatNumber value={item.regularMarketPrice} />
              </Table.Cell>
              <Table.Cell textAlign="end">
                <Switch.Root
                  size={"sm"}
                  variant={"raised"}
                  checked={countries.includes(item.countryCode)}
                  onCheckedChange={(e) =>
                    onCountryChanged(item.countryCode, e.checked)
                  }
                >
                  <Switch.HiddenInput />
                  <Switch.Control>
                    <Switch.Thumb>
                      <Switch.ThumbIndicator fallback={<HiX color="black" />}>
                        <HiCheck />
                      </Switch.ThumbIndicator>
                    </Switch.Thumb>
                  </Switch.Control>
                </Switch.Root>
              </Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table.Root>
      {lastUpdatedAt(data?.date)}
    </>
  );
};
