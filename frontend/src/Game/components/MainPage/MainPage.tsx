import { View, Text } from "react-native";
import { styles } from "./MainPageStyles";
import { PlayPages } from "../../GamePages";
import IconButton from "@/src/Shared/components/IconButton/IconButton";

interface MainPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function MainPage({ setPage }: MainPageProps) {
  return (
    <View style={styles.container}>
      <Text style={styles.header}>Time to cook!</Text>
      <IconButton
        text="Start a game"
        icon="host"
        onButtonPress={() => setPage(PlayPages.HOST_PAGE)}
      />
      <IconButton
        text="Join a friend"
        icon="join"
        onButtonPress={() => setPage(PlayPages.JOIN_PAGE)}
      />
    </View>
  );
}
