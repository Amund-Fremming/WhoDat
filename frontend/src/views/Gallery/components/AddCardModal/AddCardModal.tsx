import {
  Modal,
  View,
  Image,
  Pressable,
  TextInput,
  Alert,
  Text,
} from "react-native";
import { styles, imageStyles } from "./AddCardModalStyles";
import BigButton from "@/src/shared/components/BigButton/BigButton";
import { Colors } from "@/src/shared/assets/constants/Colors";
import FontAwesome from "@expo/vector-icons/FontAwesome";
import { useState } from "react";
import { useAuthProvider } from "@/src/shared/AuthProvider";
import { addCard } from "@/src/infrastructure/CardClient";
import { validText } from "@/src/shared/InputValitator";
import { pickImage } from "@/src/services/GalleryService/ImagePicker";

interface AddCardModalProps {
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
}

export default function AddCardModal({
  modalVisible,
  setModalVisible,
}: AddCardModalProps) {
  const [nameInput, setNameInput] = useState<string>("");
  const [imageUri, setImageUri] = useState<any>(
    "https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg"
  );

  const { token } = useAuthProvider();

  const handleNameInput = (name: string): boolean => {
    if (name.length > 9 || !validText(name)) {
      Alert.alert(
        "Input not valid",
        "Name can only be letters and 8 characters long"
      );
      return false;
    }

    setNameInput(
      nameInput.charAt(0).toUpperCase() + nameInput.slice(1).toLowerCase()
    );
    return true;
  };

  const handleImageInput = async () => {
    try {
      const result: any = await pickImage();
      setImageUri(result);
    } catch (Exception) {
      console.error("Image picker failed");
    }
  };

  const uploadCard = async () => {
    try {
      const namePresent = handleNameInput(nameInput);
      if (!namePresent) return;

      await addCard(imageUri, nameInput, token);
      setModalVisible(false);
      setNameInput("");
      setImageUri(
        "https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg"
      );
    } catch (Exception) {
      // TODO
      console.error("Adding card failed");
    }
  };

  return (
    <Modal visible={modalVisible} animationType="fade" transparent={true}>
      <View style={styles.container}>
        <View style={styles.cardModal}>
          <Pressable
            style={styles.closeButton}
            onPress={() => setModalVisible(false)}
          >
            <FontAwesome name="close" size={36} color={Colors.DarkGray} />
          </Pressable>
          <View style={styles.card}>
            <Pressable style={styles.uploadButton} onPress={handleImageInput}>
              <Text style={styles.uploadText}>upload</Text>
            </Pressable>
            <Image
              style={imageStyles.imageStyle}
              source={{
                uri: imageUri,
              }}
            />
          </View>
          <TextInput
            value={nameInput}
            onChangeText={(input: string) => setNameInput(input)}
            style={styles.inputText}
            placeholder="Name ..."
            placeholderTextColor={Colors.Gray}
          />
          <View style={styles.border} />
          <View style={styles.buttonWrapper}>
            <BigButton
              text="Add Card"
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={uploadCard}
            />
          </View>
        </View>
      </View>
    </Modal>
  );
}
