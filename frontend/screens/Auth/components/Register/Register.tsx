import { View, Text, TextInput, Alert } from "react-native";
import { styles } from "./RegisterStyles";
import Feather from "@expo/vector-icons/Feather";
import { Colors } from "@/constants/Colors";
import BigButton from "@/components/BigButton/BigButton";
import { useState } from "react";
import { validUsername } from "@/util/InputValitator";
import { IAuthResponse, IRegistrationRequest } from "@/interfaces/AuthTypes";
import { registerPlayer } from "@/api/AuthApi";
import { useAuthProvider } from "@/providers/AuthProvider";

export function Register() {
  const { token, setToken, playerID, setPlayerID, username, setUsername } =
    useAuthProvider();

  const [retypedPassword, setRetypedPassword] = useState<string>("");
  const [player, setPlayer] = useState<IRegistrationRequest>({
    username: "",
    password: "",
  });

  const handleRegister = async () => {
    const validInput: boolean = handleInputValidationAndFeedback();
    if (!validInput) return;
    console.log("Registering user...");

    try {
      const response: IAuthResponse = await registerPlayer(player);

      setToken(response.token);
      setPlayerID(response.playerID);
      setUsername(response.username);
    } catch {
      Alert.alert(
        "Invalid Registration!",
        "Something went wrong, try another username."
      );
    }

    // remove under this line when everything works
    console.log("TOKEN: " + token);
    console.log("PLAYERID: " + playerID);
    console.log("USERNAME: " + username);
  };

  const handleInputValidationAndFeedback = (): boolean => {
    if (player.username.length === 0 || player.password.length === 0) {
      Alert.alert("Invalid Input!", "Username and password cannot be empty.");
      return false;
    }

    if (player.password !== retypedPassword) {
      Alert.alert("Invalid Input!", "The passwords do not match.");
      return false;
    }

    if (!validUsername(player.username) || player.username.length > 10) {
      Alert.alert(
        "Invalid Input!",
        "Username can only consist of letters and numbers, with a max length of 10."
      );
      return false;
    }

    return true;
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Register</Text>
      <View style={styles.card}>
        <View style={styles.inputContainer}>
          <View style={styles.iconAndInput}>
            <Feather
              style={styles.icon}
              name="user"
              size={35}
              color={Colors.DarkGray}
            />
            <TextInput
              style={styles.textInput}
              placeholder="Username"
              onChangeText={(input: string) =>
                setPlayer({ ...player, username: input })
              }
            />
          </View>
          <View style={styles.border}></View>
        </View>
        <View style={styles.inputContainer}>
          <View style={styles.iconAndInput}>
            <Feather
              style={styles.icon}
              name="lock"
              size={35}
              color={Colors.DarkGray}
            />
            <TextInput
              style={styles.textInput}
              placeholder="Password"
              onChangeText={(input: string) =>
                setPlayer({ ...player, password: input })
              }
            />
          </View>
          <View style={styles.border}></View>
        </View>
        <View style={styles.inputContainer}>
          <View style={styles.iconAndInput}>
            <Feather
              style={styles.icon}
              name="lock"
              size={35}
              color={Colors.DarkGray}
            />
            <TextInput
              style={styles.textInput}
              placeholder="Retype password"
              onChangeText={(input: string) => setRetypedPassword(input)}
            />
          </View>
          <View style={styles.border}></View>
        </View>
        <BigButton
          text="Register"
          color={Colors.BurgundyRed}
          inverted={false}
          onButtonPress={handleRegister}
        />
      </View>
    </View>
  );
}
