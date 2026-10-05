import { Stack, Blockquote, Flex, Icon, Text } from "@chakra-ui/react";

import { Logo } from "./logo";
import { LuLinkedin, LuTwitter, LuYoutube } from "react-icons/lu";
import dayjs from "dayjs";
import {Link} from "react-router";


export default function Footer() {
  return (
    <Stack>
      <div>&nbsp;</div>
      <Flex gap="6" justify="space-evenly" wrap="wrap">
        <Stack>
          <Blockquote.Root colorPalette={"orange"}>
            <Blockquote.Content>Markets</Blockquote.Content>
          </Blockquote.Root>
          <div>&nbsp;</div>
          <div>
            <Link to={"#"}>US: NYSE and NASDAQ</Link>
          </div>
          <div>
            <Link to={"#"}>UK: FTSE</Link>
          </div>
          <div>
            <Link to={"#"}>Japan: NIKKEI</Link>
          </div>
          <div>
            <Link to={"#"}>Germany: DAX</Link>
          </div>
        </Stack>

        <Stack>
          <Blockquote.Root colorPalette={"orange"}>
            <Blockquote.Content>Guides</Blockquote.Content>
          </Blockquote.Root>
          <div>&nbsp;</div>
          <div>
            <Link to={"#"}>{"What is a lazy portfolio?"}</Link>
          </div>
          <div>
            <Link to={"#"}>{"How to invest?"}</Link>
          </div>
        </Stack>

        <Stack>
          <Blockquote.Root colorPalette={"orange"}>
            <Blockquote.Content>Market Wizard</Blockquote.Content>
          </Blockquote.Root>
          <div>&nbsp;</div>
          <div>
            <Link to={"#"}>Plans and pricing</Link>
          </div>
          <div>
            <Link to={"#"}>About us</Link>
          </div>
          <div>
            <Link to={"#"}>Our people</Link>
          </div>
          <div>
            <Link to={"#"}>Contact us</Link>
          </div>
        </Stack>
      </Flex>
      <div>&nbsp;</div>
      <Flex gap="6" justify="flex-start" wrap="wrap">
        <Stack>
          <Logo />
          <Stack direction={"row"}>
            <Link to={"#"}>
              <Icon fontSize="2xl" color="orange.300">
                <LuLinkedin />
              </Icon>
            </Link>
            <Link to={"#"}>
              <Icon fontSize="2xl" color="orange.300">
                <LuYoutube />
              </Icon>
            </Link>

            <Link to={"#"}>
              <Icon fontSize="2xl" color="orange.300">
                <LuTwitter />
              </Icon>
            </Link>
          </Stack>
          <div>
            <Text textStyle={"sm"} color={"gray.500"}>
              The data presented here is fictitious and intended solely for
              demonstration purposes. We are currently integrating multiple APIs
              to provide actual data in the future.
            </Text>
          </div>
        </Stack>
      </Flex>
      <div>&nbsp;</div>
      <Stack>
        <div>
          <div>DISCLAIMER</div>
          <div>&nbsp;</div>
          Market Wizard is not a financial advisor. The information provided on
          this website is for educational purposes only. We do not provide
          investment advice. Please consult a professional before making any
          financial decisions.
        </div>
        <div>
          <div>&nbsp;</div>© {dayjs().year()} Market Wizard
        </div>
      </Stack>
    </Stack>
  );
}
