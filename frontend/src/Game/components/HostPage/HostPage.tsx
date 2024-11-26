import { View, Text, Pressable } from "react-native";
import styles from "./HostPageStyles";
import { PlayPages } from "../../GamePages";
import IconButton from "../../../Shared/components/IconButton/IconButton";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import { State } from "../../types/GameTypes";

interface HostPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  setStateToCreate: React.Dispatch<React.SetStateAction<State>>;
}

export default function HostPage({ setPage, setStateToCreate }: HostPageProps) {
  const handleBothChoosing = () => {
    setPage(PlayPages.WAITING_PAGE);
    setStateToCreate(State.BOTH_CHOSING_CARDS);
  };

  const handleHostChoosing = () => {
    setPage(PlayPages.WAITING_PAGE);
    setStateToCreate(State.ONLY_HOST_CHOSING_CARDS);
  };

  return (
    <View style={styles.container}>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.textWrapper}>
        <Text style={styles.header}>Choose</Text>
        <Text style={styles.header}>the cards</Text>
      </View>
      <IconButton
        text="Your cards"
        icon="user"
        onButtonPress={handleHostChoosing}
      />
      <IconButton
        text="Split 50/50"
        icon="users"
        onButtonPress={handleBothChoosing}
      />
    </View>
  );
}
