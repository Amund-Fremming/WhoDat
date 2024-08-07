import {
  View,
  Text,
  TextInput,
  Pressable,
  Alert,
  KeyboardAvoidingView,
  Platform,
} from "react-native";
import { styles } from "./LoginStyles";
import Feather from "@expo/vector-icons/Feather";
import { Colors } from "@/constants/Colors";
import BigButton from "@/components/BigButton/BigButton";
import { IAuthResponse, ILoginRequest } from "@/interfaces/AuthTypes";
import { useState } from "react";
import { useAuthProvider } from "@/providers/AuthProvider";
import { loginPlayer } from "@/api/AuthApi";

interface LoginProps {
  setView: React.Dispatch<React.SetStateAction<string>>;
}

export function Login({ setView }: LoginProps) {
  const { setToken, setPlayerID, setUsername } = useAuthProvider();

  const [loginRequest, setLoginRequest] = useState<ILoginRequest>({
    username: "",
    password: "",
  });

  const handleLogin = async () => {
    try {
      const response: IAuthResponse = await loginPlayer(loginRequest);

      console.log(response.username);

      setToken(response.token);
      setPlayerID(response.playerID);
      setUsername(response.username);
    } catch (error) {
      Alert.alert("Invalid Login!", "The username or password was wrong!");
    }
  };

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === "ios" ? "padding" : "height"}
      style={styles.container}
    >
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
