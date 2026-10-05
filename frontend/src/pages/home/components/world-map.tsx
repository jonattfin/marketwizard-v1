import {useNivoTheme} from "../../../components/shared/nivoTheme";

type WorldMapType = {
  id: string;
  value: number;
};

const WorldMap = ({data = []}: { data?: WorldMapType[] }) => {
  const nivoTheme = useNivoTheme();

  return (
    <div style={{height: 400}}>
     
    </div>
  );
};

export default WorldMap;
