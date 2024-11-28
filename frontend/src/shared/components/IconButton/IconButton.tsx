import { Pressable, View, Text } from "react-native";
import styles from "./IconButtonStyles";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import { Feather } from "@expo/vector-icons";

interface IconButtonProps {
  text: string;
  icon: string;
  onButtonPress: () => void;
}

export default function IconButton({
  text,
  icon,
  onButtonPress,
}: IconButtonProps) {
  return (
    <Pressable style={styles.container} onPress={onButtonPress}>
      <View style={styles.wrapper}>
        <View style={styles.iconWidth}>
          {icon === "host" && (
            <Feather name="play-circle" size={40} color={Colors.DarkGray} />
          )}
          {icon === "join" && (
            <Feather name="truck" size={40} color={Colors.DarkGray} />
          )}
          {icon === "user" && (
            <Feather name="user" size={40} color={Colors.DarkGray} />
          )}
          {icon === "users" && (
            <Feather name="users" size={40} color={Colors.DarkGray} />
          )}
        </View>
        <Text style={styles.text}>{text}</Text>
      </View>
    </Pressable>
  );
}
