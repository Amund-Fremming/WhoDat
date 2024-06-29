import { View, Text, Pressable } from "react-native";
import styles from "./HostPageStyles";
import { PlayPages } from "../../PlayPages";
import IconButton from "../GenericComponents/IconButton/IconButton";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/constants/Colors";

interface HostPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function HostPage({ setPage }: HostPageProps) {
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
        onButtonPress={() => setPage(PlayPages.WAITING_PAGE)}
      />
      <IconButton
        text="Split 50/50"
        icon="users"
        onButtonPress={() => setPage(PlayPages.WAITING_PAGE)}
      />
    </View>
  );
}
