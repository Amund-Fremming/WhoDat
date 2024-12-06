import { Modal, Text, View } from "react-native";
import { styles } from "./ErrorModalStyles";
import MediumButton from "../MediumButton/MediumButton";
import { Colors } from "../../assets/constants/Colors";

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
    <Modal visible={errorModalVisible} animationType="fade" transparent={true}>
      <View style={styles.container}>
        <View style={styles.modal}>
          <Text style={styles.header}>Ooops</Text>
          <Text style={styles.message}>{message}</Text>
          <View style={styles.absoluteButton}>
            <MediumButton
              text="close"
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={() => setErrorModalVisible(!errorModalVisible)}
            />
          </View>
        </View>
      </View>
    </Modal>
  );
}
