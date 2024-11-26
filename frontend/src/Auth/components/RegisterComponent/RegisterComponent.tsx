import {
  View,
  Text,
  TextInput,
  Alert,
  Pressable,
  KeyboardAvoidingView,
  Platform,
} from "react-native";
import { styles } from "./RegisterComponentStyles";
import Feather from "@expo/vector-icons/Feather";
import { Colors } from "@/src/shared/assets/constants/Colors";
import BigButton from "@/src/shared/components/BigButton/BigButton";
import { useState } from "react";
import { validUsername } from "@/src/shared/functions/InputValitator";
import { IAuthResponse, IRegistrationRequest } from "@/src/Auth/AuthTypes";
import { registerPlayer } from "@/src/infrastructure/AuthClient";
import { useAuthProvider } from "@/src/shared/state/AuthProvider";

interface RegisterComponentProps {
  setView: React.Dispatch<React.SetStateAction<string>>;
}

export function RegisterComponent({ setView }: RegisterComponentProps) {
  const { setToken, setPlayerID, setUsername } = useAuthProvider();

  const [retypedPassword, setRetypedPassword] = useState<string>("");
  const [registrationRequest, setRegistrationRequest] =
    useState<IRegistrationRequest>({
      username: "",
      password: "",
    });

  const handleRegister = async () => {
    const validInput: boolean = handleInputValidationAndFeedback();
    if (!validInput) return;
    console.log("Registering user...");

    try {
      const response: IAuthResponse = await registerPlayer(registrationRequest);

      setToken(response.token);
      setPlayerID(response.playerID);
      setUsername(response.username);
    } catch {
      Alert.alert(
        "Invalid Registration!",
        "Something went wrong, try another username."
      );
    }
  };

  const handleInputValidationAndFeedback = (): boolean => {
    if (
      registrationRequest.username.length === 0 ||
      registrationRequest.password.length === 0
    ) {
      Alert.alert("Invalid Input!", "Username and password cannot be empty.");
      return false;
    }

    if (registrationRequest.password !== retypedPassword) {
      Alert.alert("Invalid Input!", "The passwords do not match.");
      return false;
    }

    if (
      !validUsername(registrationRequest.username) ||
      registrationRequest.username.length > 10
    ) {
      Alert.alert(
        "Invalid Input!",
        "Username can only consist of letters and numbers, with a max length of 10."
      );
      return false;
    }

    return true;
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === "ios" ? "padding" : "height"}
      style={styles.container}
    >
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
              placeholderTextColor={"gray"}
              onChangeText={(input: string) =>
                setRegistrationRequest({
                  ...registrationRequest,
                  username: input,
                })
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
              placeholderTextColor={"gray"}
              onChangeText={(input: string) =>
                setRegistrationRequest({
                  ...registrationRequest,
                  password: input,
                })
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
              placeholderTextColor={"gray"}
              onChangeText={(input: string) => setRetypedPassword(input)}
            />
          </View>
          <View style={styles.border}></View>
        </View>
        <View style={styles.loginAndRegisterNew}>
          <BigButton
            text="Register"
            color={Colors.BurgundyRed}
            inverted={false}
            onButtonPress={handleRegister}
          />
          <Pressable onPress={() => setView("LOGIN")}>
            <Text style={styles.registerNewText}>Go to Login</Text>
          </Pressable>
        </View>
      </View>
    </KeyboardAvoidingView>
  );
}
