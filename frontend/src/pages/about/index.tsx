import {useQuery} from "@tanstack/react-query";
import {MyAppNav} from "../../components/shared/nav.tsx";

function Index() {
  const query = useQuery({
    queryKey: [`watchlist`],
    queryFn: async () => {
      const response = await fetch(`/api/watchlist`);
      return await response.json();
    }
  });

  return (
    <>
      <MyAppNav/>
      <div> {query.isLoading ? "Loading ... " : JSON.stringify(query.data)}</div>
    </>
  )
}

export default Index;
