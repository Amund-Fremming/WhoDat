import { View, Pressable, Text, TextInput } from "react-native";
import { styles } from "./JoinPageStyles";
import { PlayPages } from "../../GamePages";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import BigButton from "@/src/Shared/components/BigButton/BigButton";
import { joinGame } from "../../GameHubClient";

interface JoinPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  setGameId: React.Dispatch<React.SetStateAction<number>>;
  handleJoinGame: () => Promise<void>;
  handleError: (message: string) => void;
}

export default function JoinPage({
  setPage,
  handleJoinGame,
  setGameId,
  handleError,
}: JoinPageProps) {
  const handleJoinGamePressed = async () => {
    setPage(PlayPages.WAITING_PAGE);
    await handleJoinGame();
  };

  const handleInput = (input: string) => {
    try {
      var val = Number.parseInt(input);
      if (val > 10000) {
        handleError("Game ids has to be lower than 10 000.");
        return;
      }
      setGameId(val);
    } catch (error) {
      handleError("Only numeric values.");
    }
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
          <TextInput
            onChangeText={(input) => handleInput(input)}
            keyboardType="numeric"
            style={styles.textInput}
            placeholder="37293 ..."
          />
          <View style={styles.underline} />
        </View>
        <BigButton
          text="Join"
          color={Colors.BurgundyRed}
          inverted={false}
          onButtonPress={handleJoinGamePressed}
        />
      </View>
    </View>
  );
}
