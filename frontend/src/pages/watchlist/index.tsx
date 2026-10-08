import {useState} from "react";
import {useMutation, useQuery} from "@tanstack/react-query";
import {Breadcrumb, Button, Flex, HStack, IconButton, Input, Popover, Portal, Text} from "@chakra-ui/react";
import {LuCirclePlus, LuChevronLeft, LuChevronRight, LuChevronsLeft, LuChevronsRight} from "react-icons/lu";
import {AccordionWatchlist} from "./components/accordion-watchlist";
import {queryClient} from "../../components/shared/queryClient";
import {toaster} from "../../components/ui/toaster";
import type {WatchListPageType} from "../../components/shared/types";

function Index() {
  const [open, setOpen] = useState(false);
  const [newWatchlistName, setNewWatchlistName] = useState("");
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(5);

  const query = useQuery<WatchListPageType>({
    queryKey: [`watchlist`, pageNumber, pageSize],
    queryFn: async ({ signal }) => {
      const response = await fetch(`/api/watchlist?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
        signal,
      });
      if (!response.ok) {
        throw new Error("Failed to fetch watchlists");
      }
      return await response.json();
    }
  });

  const useCreateWatchlist = useMutation({
    mutationFn: async ({name, idempotencyKey}: {name: string; idempotencyKey: string}) => {
      const response = await fetch(`/api/watchlist`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "X-Idempotency-Key": idempotencyKey,
        },
        body: JSON.stringify({name}),
      });

      if (!response.ok) {
        throw new Error("Watchlist can't be created! Please try again later!");
      }

      return await response.json();
    },
    onError: () => {
      toaster.create({
        title: `Watchlist can't be created! Please try again later!`,
        type: "error",
      });
    },
  });

  const isCreating = useCreateWatchlist.isPending;

  const useUpdateWatchlist = useMutation({
    mutationFn: async ({id, name, idempotencyKey}: {id: string; name: string; idempotencyKey: string}) => {
      const response = await fetch(`/api/watchlist/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          "X-Idempotency-Key": idempotencyKey,
        },
        body: JSON.stringify({name}),
      });
      
      if (!response.ok) {
        throw new Error("Watchlist can't be updated! Please try again later!");
      }
      
      return await response.json();
    },
    onError: () => {
      toaster.create({
        title: `Watchlist can't be updated! Please try again later!`,
        type: "error",
      });
    },
  });

  const useDeleteWatchlist = useMutation({
    mutationFn: async ({id, idempotencyKey}: { id: string; idempotencyKey: string }) => {
      const response = await fetch(`/api/watchlist/${id}`, {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          "X-Idempotency-Key": idempotencyKey,
        },
      });
      
      if (!response.ok) {
        throw new Error("Watchlist can't be deleted! Please try again later!");
      }
    },
    onError: () => {
      toaster.create({
        title: `Watchlist can't be deleted! Please try again later!`,
        type: "error",
      });
    },
  });

  const handleCreate = async (name: string) => {
    const trimmed = name.trim();
    if (!trimmed || useCreateWatchlist.isPending) return;

    const idempotencyKey = crypto.randomUUID();
    await useCreateWatchlist.mutateAsync({
      name: trimmed,
      idempotencyKey,
    });
    setPageNumber(1);
    await queryClient.invalidateQueries({queryKey: ["watchlist"]});

    toaster.create({
      title: `Watchlist created successfully!`,
      type: "success",
    });
  };

  const handleUpdate = async (id: string, watchlistName: string) => {
    const trimmed = watchlistName.trim();
    if (!trimmed || useUpdateWatchlist.isPending) return;

    const idempotencyKey = crypto.randomUUID();
    await useUpdateWatchlist.mutateAsync({
      id,
      name: trimmed,
      idempotencyKey,
    });
    await queryClient.invalidateQueries({queryKey: ["watchlist"]});

    toaster.create({
      title: `Watchlist updated successfully!`,
      type: "success",
    });
  };

  const handleDelete = async (id: string) => {
    if (useDeleteWatchlist.isPending) return;

    const idempotencyKey = crypto.randomUUID();
    await useDeleteWatchlist.mutateAsync({
      id,
      idempotencyKey,
    });
    if ((query.data?.items?.length ?? 0) <= 1 && pageNumber > 1) {
      setPageNumber((prev) => prev - 1);
    }
    await queryClient.invalidateQueries({queryKey: ["watchlist"]});

    toaster.create({
      title: `Watchlist deleted successfully!`,
      type: "success",
    });
  };

  const watchlists = query.data?.items ?? [];
  const totalCount = query.data?.totalCount ?? 0;
  const totalPages = query.data?.totalPages ?? 0;
  const hasPreviousPage = query.data?.hasPreviousPage ?? pageNumber > 1;
  const hasNextPage = query.data?.hasNextPage ?? (totalPages > 0 && pageNumber < totalPages);

  return (
    <>
      <Flex justify="space-between" align="center">
        <Breadcrumb.Root>
          <Breadcrumb.List>
            <Breadcrumb.Item>
              <Breadcrumb.Link href="/">Home</Breadcrumb.Link>
            </Breadcrumb.Item>
            <Breadcrumb.Separator/>
            <Breadcrumb.Item>
              <Breadcrumb.CurrentLink data-testid={"watchlist-link"}>
                Watchlist
              </Breadcrumb.CurrentLink>
            </Breadcrumb.Item>
          </Breadcrumb.List>
        </Breadcrumb.Root>
        <div>&nbsp;</div>
        <Popover.Root open={open} onOpenChange={(e) => setOpen(e.open)}>
          <Popover.Trigger asChild>
            <Button size="sm" variant="outline">
              <LuCirclePlus />
              Create watchlist
            </Button>
          </Popover.Trigger>
          <Portal>
            <Popover.Positioner>
              <Popover.Content>
                <Popover.Arrow />
                <Popover.Body>
                  <Input
                    placeholder="Watchlist name"
                    size="sm"
                    value={newWatchlistName}
                    disabled={isCreating}
                    onChange={(e) => setNewWatchlistName(e.target.value)}
                    onKeyDown={async (e) => {
                      if (e.key === "Enter" && newWatchlistName.trim() && !isCreating) {
                        await handleCreate(newWatchlistName);
                        setNewWatchlistName("");
                        setOpen(false);
                      }
                    }}
                  />
                  <Button
                    mt="4"
                    size="sm"
                    variant="outline"
                    loading={isCreating}
                    disabled={isCreating || !newWatchlistName.trim()}
                    onClick={async () => {
                      if (!newWatchlistName.trim() || isCreating) return;
                      await handleCreate(newWatchlistName);
                      setNewWatchlistName("");
                      setOpen(false);
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
      <div>&nbsp;</div>
      <AccordionWatchlist
        {...{
          watchlists,
          handleUpdate,
          handleDelete,
        }}
      />
      <div>&nbsp;</div>
      {totalCount > 0 && (
        <Flex
          direction={{base: "column", sm: "row"}}
          justify="space-between"
          align="center"
          gap="4"
          py="2"
        >
          <Flex align="center" gap="4">
            <Text fontSize="sm" color="gray.500">
              Showing {(pageNumber - 1) * pageSize + 1} to{" "}
              {Math.min(pageNumber * pageSize, totalCount)} of {totalCount} watchlists
            </Text>
            <HStack gap="1">
              <Text fontSize="xs" color="gray.500">
                Per page:
              </Text>
              {[5, 10, 20].map((size) => (
                <Button
                  key={size}
                  size="2xs"
                  variant={pageSize === size ? "solid" : "outline"}
                  onClick={() => {
                    setPageSize(size);
                    setPageNumber(1);
                  }}
                >
                  {size}
                </Button>
              ))}
            </HStack>
          </Flex>
          <HStack gap="2">
            <IconButton
              size="sm"
              variant="outline"
              disabled={!hasPreviousPage}
              onClick={() => setPageNumber(1)}
              aria-label="First page"
            >
              <LuChevronsLeft />
            </IconButton>
            <IconButton
              size="sm"
              variant="outline"
              disabled={!hasPreviousPage}
              onClick={() => setPageNumber((prev) => Math.max(1, prev - 1))}
              aria-label="Previous page"
            >
              <LuChevronLeft />
            </IconButton>
            <Text fontSize="sm" px="2">
              Page {pageNumber} of {totalPages || 1}
            </Text>
            <IconButton
              size="sm"
              variant="outline"
              disabled={!hasNextPage}
              onClick={() => setPageNumber((prev) => Math.min(totalPages, prev + 1))}
              aria-label="Next page"
            >
              <LuChevronRight />
            </IconButton>
            <IconButton
              size="sm"
              variant="outline"
              disabled={!hasNextPage}
              onClick={() => setPageNumber(totalPages)}
              aria-label="Last page"
            >
              <LuChevronsRight />
            </IconButton>
          </HStack>
        </Flex>
      )}
      <div>&nbsp;</div>
    </>
  )
}

export default Index;
