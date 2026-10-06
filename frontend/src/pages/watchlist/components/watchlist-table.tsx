import type {WatchListType} from "../../../components/shared/types";
import {
  Button,
  ButtonGroup,
  Flex,
  IconButton,
  Input,
  Popover,
  Portal,
  Stack,
  Table,
} from "@chakra-ui/react";
import {useState} from "react";
import {LuPencilOff, LuCirclePlus} from "react-icons/lu";
import {Tooltip} from "../../../components/ui/tooltip";
import {toaster} from "../../../components/ui/toaster";
import {useMutation} from "@tanstack/react-query";
import {queryClient} from "../../../components/shared/queryClient";

export type WatchlistTableType = {
  watchlist: WatchListType;
};

export const WatchlistTable = ({watchlist}: WatchlistTableType) => {
  const [ticker, setTicker] = useState<string>("");
  const [open, setOpen] = useState(false);

  const useCreateWatchlistItem = useMutation({
    mutationFn: async (watchlistItem: {
      watchlistId: string;
      ticker: string;
    }) => {
      const response = await fetch("/api/watchlist-item", {
        method: "POST",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify(watchlistItem),
      });
      
      if (!response.ok) {
        throw new Error("Watchlist item can't be created! Please try again later!");
      }
      
      return await response.json();
    },
  });

  const useDeleteWatchlistItem = useMutation({
    mutationFn: async (watchlist: { id: string; itemId: string }) => {
      const response = await fetch("/api/watchlist-item", {
        method: "DELETE",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify(watchlist),
      });
      
      if (!response.ok) {
        throw new Error("Watchlist item can't be deleted! Please try again later!");
      }
    },
  });

  const handleCreate = async (watchlistId: string, ticker: string) => {
    try {
      await useCreateWatchlistItem.mutateAsync({
        watchlistId,
        ticker,
      });
      await queryClient.invalidateQueries({queryKey: ["watchlist"]});

      setTicker("");
      setOpen(false);

      toaster.create({
        title: `Watchlist item created successfully!`,
        type: "success",
      });
    } catch (e) {
      toaster.create({
        title: `Watchlist item creation failed!`,
        type: "success",
      });

      console.error(e);
    }
  };

  const handleDelete = async (id: string, itemId: string) => {
    try {
      await useDeleteWatchlistItem.mutateAsync({
        id,
        itemId,
      });
      await queryClient.invalidateQueries({queryKey: ["watchlist"]});

      toaster.create({
        title: `Watchlist item deleted successfully!`,
        type: "success",
      });
    } catch (e) {
      toaster.create({
        title: `Watchlist item delete failed!`,
        type: "success",
      });

      console.error(e);
    }
  };

  return (
    <Stack>
      <Table.Root striped>
        <Table.Header>
          <Table.Row>
            <Table.ColumnHeader>Name</Table.ColumnHeader>
            <Table.ColumnHeader>Ticker</Table.ColumnHeader>
            <Table.ColumnHeader textAlign="end"></Table.ColumnHeader>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {watchlist.items.map((item) => (
            <Table.Row key={item.id}>
              <Table.Cell>{item.name}</Table.Cell>
              <Table.Cell>{item.ticker}</Table.Cell>
              <Table.Cell textAlign="end">
                <Flex gap="4" justify="flex-end">
                  <ButtonGroup size="sm" variant="outline">
                    <IconButton
                      size={"sm"}
                      variant={"outline"}
                      color={"red.300"}
                      onClick={async () => {
                        await handleDelete(watchlist.id, item.id);
                      }}
                    >
                      <Tooltip content="Delete item">
                        <LuPencilOff/>
                      </Tooltip>
                    </IconButton>
                  </ButtonGroup>
                </Flex>
              </Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table.Root>
      <div>&nbsp;</div>
      <Flex>
        <Popover.Root open={open} onOpenChange={(e) => setOpen(e.open)}>
          <Popover.Trigger asChild>
            <Button size="sm" variant="outline">
              <LuCirclePlus/>
              Add item
            </Button>
          </Popover.Trigger>
          <Portal>
            <Popover.Positioner>
              <Popover.Content>
                <Popover.Arrow/>
                <Popover.Body>
                  <Input
                    placeholder="Asset name"
                    size="sm"
                    value={ticker}
                    onChange={(e) => setTicker(e.target.value)}
                  />
                  <Button
                    mt="4"
                    size="sm"
                    variant="outline"
                    onClick={async () => {
                      await handleCreate(watchlist.id, ticker);
                    }}
                  >
                    Create
                  </Button>
                </Popover.Body>
              </Popover.Content>
            </Popover.Positioner>
          </Portal>
        </Popover.Root>
      </Flex>
    </Stack>
  );
};
