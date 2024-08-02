import { View, Text, TextInput } from "react-native";
import { styles } from "./RegisterStyles";
import Feather from "@expo/vector-icons/Feather";
import { Colors } from "@/constants/Colors";
import BigButton from "@/components/BigButton/BigButton";

export function Register() {
  const handleRegister = () => {
    // Todo
    console.log("Registering user...");
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Register</Text>
      <View style={styles.card}>
        <View style={styles.inputContainer}>
          <View style={styles.iconAndInput}>
            <Feather name="user" size={35} color={Colors.DarkGray} />
            <TextInput placeholder="Username" />
          </View>
          <View style={styles.border}></View>
        </View>
        <View style={styles.inputContainer}>
          <View style={styles.iconAndInput}>
            <Feather name="lock" size={35} color={Colors.DarkGray} />
            <TextInput placeholder="Password" />
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
