import { Modal, View, Image, Pressable, TextInput, Text } from "react-native";
import { styles, imageStyles } from "./AddCardModalStyles";
import BigButton from "@/src/Shared/components/BigButton/BigButton";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import FontAwesome from "@expo/vector-icons/FontAwesome";
import { useState } from "react";
import { useAuthProvider } from "@/src/Shared/state/AuthProvider";
import { addCard } from "@/src/Shared/functions/CardClient";
import { validText } from "@/src/Shared/functions/InputValitator";
import { pickImage } from "@/src/Shared/functions/ImagePicker";
import Result from "@/src/Shared/domain/Result";
import { moderateScale } from "@/src/Shared/assets/constants/Dimentions";

interface AddCardModalProps {
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
  handleError: (message: string) => void;
}

export default function AddCardModal({
  modalVisible,
  setModalVisible,
  handleError
}: AddCardModalProps) {
  const [nameInput, setNameInput] = useState<string>("");
  const [imageUri, setImageUri] = useState<any>(
    "https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg"
  );
  const { token } = useAuthProvider();

  const handleNameInput = (name: string): boolean => {
    if (name.length <= 0) {
      handleError("Name cannot be empty.");
      return false;
    }

    if (name.length > 9 || !validText(name)) {
      setModalVisible(!modalVisible)
      handleError("Name must be text only and under 9 letters long");
      return false;
    }

    setNameInput(
      nameInput.charAt(0).toUpperCase() + nameInput.slice(1).toLowerCase()
    );
    return true;
  };

  const handleImageInput = async () => {
    try {
      const result = await pickImage();
      if(result === "EXIT") return;
      setImageUri(result);
    } catch (Exception) {
      handleError("Image picker failed.");
    }
  };

  const uploadCard = async () => {
    const namePresent = handleNameInput(nameInput);
    if (!namePresent) return;

    var result: Result<boolean> = await addCard(imageUri, nameInput, token);
    if (result.isError) {
      handleError(result.message);
      return;
    }

    setModalVisible(false);
    setNameInput("");
    setImageUri(
      "https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg"
    );
  };

  return (
    <>
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
    </>
  );
}
