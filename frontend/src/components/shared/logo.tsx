import {Heading, IconButton, Mark, useHighlight} from "@chakra-ui/react";
import {SiCoinmarketcap} from "react-icons/si";

import {Fragment} from "react";
import {Link} from "react-router";

export const Logo = () => {
  const chunks = useHighlight({
    text: "Market Wizard",
    query: ["Wizard"],
  });

  return (
    <Link to={"/"}>
      <IconButton variant={"ghost"} colorPalette={"border"}>
        <SiCoinmarketcap/>
        <Heading size="xl" maxW="20ch">
          {chunks.map((chunk, _) => {
            return chunk.match ? (
              <Mark
                key={chunk.text}
                css={{
                  fontStyle: "italic",
                  color: "red.400",
                  position: "relative",
                }}
              >
                {chunk.text}
              </Mark>
            ) : (
              <Fragment key={chunk.text}>{chunk.text}</Fragment>
            );
          })}
        </Heading>
      </IconButton>
    </Link>
  );
};
