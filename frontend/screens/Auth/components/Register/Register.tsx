import { View, Text, TextInput, Alert, Pressable } from "react-native";
import { styles } from "./RegisterStyles";
import Feather from "@expo/vector-icons/Feather";
import { Colors } from "@/constants/Colors";
import BigButton from "@/components/BigButton/BigButton";
import { useState } from "react";
import { validUsername } from "@/util/InputValitator";
import { IAuthResponse, IRegistrationRequest } from "@/interfaces/AuthTypes";
import { registerPlayer } from "@/api/AuthApi";
import { useAuthProvider } from "@/providers/AuthProvider";

interface RegisterProps {
  setView: React.Dispatch<React.SetStateAction<string>>;
}

export function Register({ setView }: RegisterProps) {
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
      console.log(response.username);

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
    </View>
  );
}
