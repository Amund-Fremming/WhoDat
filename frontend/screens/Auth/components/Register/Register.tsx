import { View, Text, TextInput } from "react-native";
import { styles } from "./RegisterStyles";
import Feather from "@expo/vector-icons/Feather";
import { Colors } from "@/constants/Colors";
import BigButton from "@/components/BigButton/BigButton";
import { useState } from "react";
import { IPlayerDto } from "@/interfaces/PlayerTypes";

export function Register() {
  const [retypedPassword, setRetypedPassword] = useState<string>("");
  const [player, setPlayer] = useState<IPlayerDto>({
    username: "",
    password: "",
  });

  const handleRegister = () => {
    // Todo
    // - validate input
    // - try register with api call
    // - loader ??
    // - if error
    //      - feedback
    // - else setToken and redirect should be automatic
    console.log("Registering user...");
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
