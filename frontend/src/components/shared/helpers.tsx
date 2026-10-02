import dayjs from "dayjs";
import {Text, Flex, Badge} from "@chakra-ui/react";

export const lastUpdatedAt = (date?: Date) => {
  const updatedAt = dayjs(date).format("MMM D, YYYY h:mm A");

  return (
    <Flex justify="flex-end">
      <Text textStyle={"sm"} color={"gray.500"}>
        <Badge variant={"subtle"} size={"xs"}>
          {`Updated at: ${updatedAt}`}
        </Badge>
      </Text>
    </Flex>
  );
};

export const COUNTRY_CODES = [
  "AUS",
  "CHN",
  "DEU",
  "ESP",
  "FRA",
  "GBR",
  "IND",
  "JPN",
  "KOR",
  "USA",
];
