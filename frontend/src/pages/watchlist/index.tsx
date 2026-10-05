import {useMutation, useQuery} from "@tanstack/react-query";
import {Breadcrumb} from "@chakra-ui/react";
import {AccordionWatchlist} from "./components/accordion-watchlist";
import {queryClient} from "../../components/shared/queryClient";
import {toaster} from "../../components/ui/toaster";

function Index() {
  const query = useQuery({
    queryKey: [`watchlist`],
    queryFn: async () => {
      const response = await fetch(`/api/watchlist`);
      return await response.json();
    }
  });

  const useUpdateWatchlist = useMutation({
    mutationFn: async (watchlist: { id: string; name: string }) => {
      const response = await fetch("/api/watchlist", {
        method: "PUT",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify(watchlist),
      });
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
    mutationFn: async (watchlist: { id: string }) => {
      const response = await fetch("/api/watchlist", {
        method: "DELETE",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify(watchlist),
      });
      return await response.json();
    },
    onError: () => {
      toaster.create({
        title: `Watchlist can't be deleted! Please try again later!`,
        type: "error",
      });
    },
  });

  const handleUpdate = async (id: string, watchlistName: string) => {
    await useUpdateWatchlist.mutateAsync({
      id,
      name: watchlistName,
    });
    await queryClient.invalidateQueries({queryKey: ["watchlist"]});

    toaster.create({
      title: `Watchlist updated successfully!`,
      type: "success",
    });
  };

  const handleDelete = async (id: string) => {
    await useDeleteWatchlist.mutateAsync({
      id,
    });
    await queryClient.invalidateQueries({queryKey: ["watchlist"]});

    toaster.create({
      title: `Watchlist deleted successfully!`,
      type: "success",
    });
  };

  return (
    <>
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
      <AccordionWatchlist
        {...{
          watchlists: query.data ?? [],
          handleUpdate,
          handleDelete,
        }}
      />
      <div>&nbsp;</div>
    </>
  )
}

export default Index;
