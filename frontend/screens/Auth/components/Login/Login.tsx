import { View, Text, TextInput, Pressable } from "react-native";
import { styles } from "./LoginStyles";
import Feather from "@expo/vector-icons/Feather";
import { Colors } from "@/constants/Colors";
import BigButton from "@/components/BigButton/BigButton";
import { ILoginRequest } from "@/interfaces/AuthTypes";
import { useState } from "react";
import { useAuthProvider } from "@/providers/AuthProvider";

interface LoginProps {
  setView: React.Dispatch<React.SetStateAction<string>>;
}

export function Login({ setView }: LoginProps) {
  const { setToken, setPlayerID, setUsername } = useAuthProvider();

  const [loginRequest, setLoginRequest] = useState<ILoginRequest>({
    username: "",
    password: "",
  });

  const handleLogin = () => {
    // TODO
  };

  return (
    <View style={styles.container}>
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
    </View>
  );
}
