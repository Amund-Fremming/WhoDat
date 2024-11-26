import { Pressable } from "react-native";
import styles from "./ErrorModalStyles";

interface ErrorModalProps {
    message: string;
    onCloseAction: () => void;
}

export default function ErrorModal ({message, onCloseAction} : ErrorModalProps) {
    return(
        <Pressable style={styles.container}>
        </Pressable>
    );
}