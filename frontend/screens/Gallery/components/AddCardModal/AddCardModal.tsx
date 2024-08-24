import { Modal, View, Image, Pressable, TextInput } from "react-native";
import { styles, imageStyles } from "../AddCardModal/AddCardModalStyles";
import BigButton from "@/components/BigButton/BigButton";
import { Colors } from "@/constants/Colors";
import FontAwesome from "@expo/vector-icons/FontAwesome";
import { useState } from "react";
import { ICardInputDto } from "@/interfaces/GalleryTypes";
import { addCardToGallery } from "@/api/GalleryApi";
import { useAuthProvider } from "@/providers/AuthProvider";

interface AddCardModalProps {
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
}

export default function AddCardModal({
  modalVisible,
  setModalVisible,
}: AddCardModalProps) {
  const [nameInput, setNameInput] = useState<string>("");

  const { token } = useAuthProvider();

  const handleNameInput = (input: string) => {
    if (input.length > 9) {
      // Use util function with regex and length that also shows users alert
      // on what went wrong
      return;
    }

    input = input.toLowerCase();
    setNameInput(input.charAt(0).toUpperCase() + input.slice(1));
  };

  const handleAddCard = () => {
    try {
      addCardToGallery(galleryId, cardDto, token);
    } catch (error) {
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
            <Image
              style={imageStyles.imageStyle}
              source={{
                uri: "https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg",
              }}
            />
          </View>
          <TextInput
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
