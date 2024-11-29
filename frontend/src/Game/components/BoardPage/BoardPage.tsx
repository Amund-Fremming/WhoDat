import { View, Text } from "react-native";
import { styles } from "./BoardPageStyles";
import { PlayPages } from "../../GamePages";

interface BoardPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function BoardPage({ setPage }: BoardPageProps) {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>BoardPage</Text>
    </View>
  );
}
