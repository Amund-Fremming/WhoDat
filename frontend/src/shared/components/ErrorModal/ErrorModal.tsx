import { Modal, Pressable, Text } from "react-native";
import { styles } from "./ErrorModalStyles";

interface ErrorModalProps {
  message: string;
  errorModalVisible: boolean;
  setErrorModalVisible: (condition: boolean) => void;
}

export default function ErrorModal({
  message,
  errorModalVisible,
  setErrorModalVisible,
}: ErrorModalProps) {
  return (
    <Modal style={styles.container} visible={errorModalVisible}>
      <Text>{message}</Text>
      <Pressable
        onPress={() => setErrorModalVisible(!errorModalVisible)}
      ></Pressable>
    </Modal>
  );
}
