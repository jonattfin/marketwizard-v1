import type {WatchListType} from "../../../components/shared/types";
import {Tooltip} from "../../../components/ui/tooltip";
import {
  AbsoluteCenter,
  Accordion,
  Box,
  Button,
  Grid,
  GridItem,
  Icon,
  IconButton,
  Input,
  Popover,
  Portal,
  Stack,
} from "@chakra-ui/react";
import {useState} from "react";
import {LuBrush, LuPencilOff, LuAtom} from "react-icons/lu";
import {WatchlistTable} from "./watchlist-table";

export type AccordionWatchlistProps = {
  watchlists: WatchListType[];
  handleUpdate: (id: string, watchlistName: string) => Promise<void>;
  handleDelete: (id: string) => Promise<void>;
};

export const AccordionWatchlist = ({
                                     watchlists,
                                     handleUpdate,
                                     handleDelete,
                                   }: Readonly<AccordionWatchlistProps>) => {
  const [watchlistName, setWatchlistName] = useState<string>("");
  const [popovers, setPopovers] = useState<string[]>([]);

  const handleOpen = (id: string, isOpen: boolean) => {
    setPopovers(isOpen ? [id] : []);
  };

  return (
    <Grid
      templateColumns={{
        base: "1fr",
        md: "repeat(1, 1fr)",
        lg: "repeat(2, 1fr)",
      }}
      gap="6"
    >
      <GridItem colSpan={2}>
        <Accordion.Root multiple>
          {watchlists.map((watchlist) => (
            <Accordion.Item key={watchlist.id} value={watchlist.id.toString()}>
              <Box position="relative" m={"1"}>
                <Accordion.ItemTrigger>
                  <Icon fontSize="lg" color={"orange.200"}>
                    <LuAtom/>
                  </Icon>
                  {`${watchlist.name} | items: [${watchlist.items.length}]`}
                  <Accordion.ItemIndicator/>
                </Accordion.ItemTrigger>
                <AbsoluteCenter axis="vertical" insetEnd="0">
                  <Stack direction={"row"} gap="2">
                    <Popover.Root
                      open={popovers.includes(watchlist.id)}
                      onOpenChange={(e) => handleOpen(watchlist.id, e.open)}
                    >
                      <Popover.Trigger asChild>
                        <IconButton
                          size={"sm"}
                          variant={"outline"}
                          color={"green.300"}
                          onClick={() => setWatchlistName(watchlist.name)}
                        >
                          <Tooltip content="Update watchlist">
                            <LuBrush/>
                          </Tooltip>
                        </IconButton>
                      </Popover.Trigger>
                      <Portal>
                        <Popover.Positioner>
                          <Popover.Content>
                            <Popover.Arrow/>
                            <Popover.Body>
                              <Input
                                placeholder="Watchlist name"
                                size="sm"
                                value={watchlistName}
                                onChange={(e) =>
                                  setWatchlistName(e.target.value)
                                }
                              />
                              <Button
                                mt="4"
                                size="sm"
                                variant="outline"
                                onClick={async () => {
                                  await handleUpdate(
                                    watchlist.id,
                                    watchlistName,
                                  );
                                  setPopovers([]);
                                }}
                              >
                                Update
                              </Button>
                            </Popover.Body>
                          </Popover.Content>
                        </Popover.Positioner>
                      </Portal>
                    </Popover.Root>
                    <IconButton
                      size={"sm"}
                      variant={"outline"}
                      color={"red.300"}
                      onClick={async () => {
                        await handleDelete(watchlist.id);
                      }}
                    >
                      <Tooltip content="Delete watchlist">
                        <LuPencilOff/>
                      </Tooltip>
                    </IconButton>
                  </Stack>
                </AbsoluteCenter>
              </Box>
              <Accordion.ItemContent>
                <Accordion.ItemBody>
                  <WatchlistTable
                    {...{
                      watchlist,
                    }}
                  />
                  <div>&nbsp;</div>
                </Accordion.ItemBody>
              </Accordion.ItemContent>
            </Accordion.Item>
          ))}
        </Accordion.Root>
      </GridItem>
    </Grid>
  );
};
