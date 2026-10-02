import './App.css';
import {Button, HStack} from "@chakra-ui/react";
import {useQuery} from "@tanstack/react-query";

function App() {
  const query = useQuery({
    queryKey: [`weatherforecast`],
    queryFn: async () => {
      const response = await fetch(`/api/weatherforecast`);
      return await response.json();
    }
  });

  return (
    <>
      <HStack>
        <Button>Click me</Button>
        <Button>Click me</Button>
      </HStack>
      <div> {query.isLoading ? "Loading ... " : JSON.stringify(query.data)}</div>
    </>
  )
}

export default App;
