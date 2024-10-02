import {
  Modal,
  View,
  Image,
  Pressable,
  TextInput,
  Alert,
  Text,
} from "react-native";
import { styles, imageStyles } from "../AddCardModal/AddCardModalStyles";
import BigButton from "@/components/BigButton/BigButton";
import { Colors } from "@/constants/Colors";
import FontAwesome from "@expo/vector-icons/FontAwesome";
import { useState } from "react";
import { useAuthProvider } from "@/providers/AuthProvider";
import { addCard } from "@/api/CardApi";
import { validText } from "@/util/InputValitator";
import { pickImage } from "@/util/ImagePicker";

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

  const handleNameInput = () => {
    if (nameInput.length > 9 || !validText(nameInput)) {
      setNameInput("");
      Alert.alert(
        "Input not valid",
        "Name can only be letters and 8 characters long"
      );
      return;
    }

    setNameInput(
      nameInput.charAt(0).toUpperCase() + nameInput.slice(1).toLowerCase()
    );
  };

  const handleUploadImage = async () => {
    try {
      const uri: any = await pickImage();
      if (uri === "EXIT") return;

      setImageUri(uri);
    } catch (Exception) {
      console.error("Image picker failed");
    }
  };

  const handleAddCard = async () => {
    try {
      handleNameInput();

      const blobResponse = await fetch(imageUri);
      const blob: Blob = await blobResponse.blob();

      const formData = new FormData();

      formData.append("Name", nameInput);
      formData.append("Image", blob, "image.jpg");

      console.log(blob.type);

      await addCard(formData, token);
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
            <Pressable style={styles.uploadButton} onPress={handleUploadImage}>
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
              onButtonPress={handleAddCard}
            />
          </View>
        </View>
      </View>
    </Modal>
  );
}
