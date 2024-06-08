import { Text, Pressable } from "react-native";
import styles from "./BigButtonStyles";
import { Colors } from "@/constants/Colors";

interface BigButtonProps {
    text: string;
    color: string;
    inverted: boolean;
    onButtonPress: () => void;
}

export default function BigButton({
    text,
    color,
    inverted,
    onButtonPress,
}: BigButtonProps) {
    return (
        <Pressable
            onPress={onButtonPress}
            style={{ ...styles.button, backgroundColor: inverted ? Colors.Cream}}
        >
            <Text>{text}</Text>
        </Pressable>
    );
}
