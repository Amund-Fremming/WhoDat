import { View, Pressable, Text, TextInput } from "react-native";
import styles from "./JoinPageStyles";
import { PlayPages } from "../../PlayPages";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/shared/assets/constants/Colors";
import BigButton from "@/src/shared/components/BigButton/BigButton";

interface JoinPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function JoinPage({ setPage }: JoinPageProps) {
  const handleJoinGame = () => {
    // TODO
    setPage(PlayPages.WAITING_PAGE);
  };

  return (
    <View style={styles.container}>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <Text style={styles.header}>Type a friends id</Text>
      <View style={styles.box}>
        <View style={styles.inputWrapper}>
          <TextInput style={styles.textInput} placeholder="37293 ..." />
          <View style={styles.underline} />
        </View>
        <BigButton
          text="Join"
          color={Colors.BurgundyRed}
          inverted={false}
          onButtonPress={handleJoinGame}
        />
      </View>
    </View>
  );
}
