import './App.css';
import {useQuery} from "@tanstack/react-query";
import {MyAppNav} from "../../components/shared/nav.tsx";

function Index() {
  const query = useQuery({
    queryKey: [`weatherforecast`],
    queryFn: async () => {
      const response = await fetch(`/api/weatherforecast`);
      return await response.json();
    }
  });

  return (
    <>
      Home
      <MyAppNav/>
      <div> {query.isLoading ? "Loading ... " : JSON.stringify(query.data)}</div>
    </>
  )
}

export default Index;
