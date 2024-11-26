import {
  View,
  Text,
  TextInput,
  Pressable,
  KeyboardAvoidingView,
  Platform,
} from "react-native";
import { styles } from "./LoginComponentStyles";
import Feather from "@expo/vector-icons/Feather";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import BigButton from "@/src/Shared/components/BigButton/BigButton";
import { IAuthResponse, ILoginRequest } from "@/src/Auth/AuthTypes";
import { useState } from "react";
import { useAuthProvider } from "@/src/Shared/state/AuthProvider";
import { loginPlayer } from "../../AuthClient";
import { Result } from "@/src/Shared/domain/Result";
import ErrorModal from "@/src/Shared/components/ErrorModal/ErrorModal";

interface LoginComponentProps {
  setView: React.Dispatch<React.SetStateAction<string>>;
}

export function LoginComponent({ setView }: LoginComponentProps) {
  const { setToken, setPlayerID, setUsername } = useAuthProvider();
  const [errorModalVisible, setErrorModalVisible] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");

  const handleError = (message: string) => {
    setErrorModalVisible(true);
    setErrorMessage(message);
  };

  const [loginRequest, setLoginRequest] = useState<ILoginRequest>({
    username: "",
    password: "",
  });

  const handleLogin = async () => {
    const result: Result<IAuthResponse> = await loginPlayer(loginRequest);
    if (result.isError) {
      handleError(result.message);
      return;
    }

    const response: IAuthResponse | null = result.data;
    setToken(response!.token);
    setPlayerID(response!.playerID);
    setUsername(response!.username);
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === "ios" ? "padding" : "height"}
      style={styles.container}
    >
      <ErrorModal
        errorModalVisible={errorModalVisible}
        setErrorModalVisible={setErrorModalVisible}
        message={errorMessage}
      />

      <Text style={styles.header}>Login</Text>
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
                setLoginRequest({ ...loginRequest, username: input })
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
                setLoginRequest({ ...loginRequest, password: input })
              }
            />
          </View>
          <View style={styles.border}></View>
        </View>
        <View style={styles.loginAndRegisterNew}>
          <BigButton
            text="Login"
            color={Colors.BurgundyRed}
            inverted={false}
            onButtonPress={handleLogin}
          />
          <Pressable onPress={() => setView("REGISTER")}>
            <Text style={styles.registerNewText}>Register new player</Text>
          </Pressable>
        </View>
      </View>
    </KeyboardAvoidingView>
  );
}
