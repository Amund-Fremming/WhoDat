import { Modal, View, Image, Pressable, TextInput } from "react-native";
import { styles, imageStyles } from "./CardModalStyles";
import BigButton from "@/components/BigButton/BigButton";
import { Colors } from "@/constants/Colors";
import StrokedText from "@/components/StokedText/StrokedText";
import { ICard } from "@/interfaces/ICard";
import FontAwesome from "@expo/vector-icons/FontAwesome";
import { useEffect, useState } from "react";

interface CardModalProps {
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
  card: ICard;
}

export default function CardModal({
  modalVisible,
  setModalVisible,
  card,
}: CardModalProps) {
  const [editMode, setEditMode] = useState<boolean>(false);
  const [newNameInput, setNewNameInput] = useState<string>("");

  useEffect(() => {
    setNewNameInput("");
    setEditMode(false);
  }, [modalVisible]);

  const handleEditCardPressed = () => {
    // TODO
    setEditMode(false);
  };

  const handleNewNameInput = (input: string) => {
    if (input.length > 9) {
      // Use util function with regex and length that also shows users alert
      // on what went wrong
      return;
    }
    input = input.toLowerCase();
    setNewNameInput(input.charAt(0).toUpperCase() + input.slice(1));
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
                uri: "https://www.wikihow.com/images/thumb/9/90/What_type_of_person_are_you_quiz_pic.png/1200px-What_type_of_person_are_you_quiz_pic.png",
              }}
            />
          </View>

          {!editMode && (
            <>
              <StrokedText
                text={card.name}
                fontBaseSize={40}
                smallBorder={false}
              />
              <View style={styles.buttonWrapper}>
                <BigButton
                  text="Edit"
                  color={Colors.BurgundyRed}
                  inverted={false}
                  onButtonPress={() => setEditMode(true)}
                />
              </View>
            </>
          )}

          {editMode && (
            <>
              <TextInput
                onChangeText={(input: string) => handleNewNameInput(input)}
                style={styles.newNameInput}
                value={newNameInput}
                placeholder={card.name}
              />
              <View style={styles.buttonWrapper}>
                <BigButton
                  text="Save"
                  color={Colors.BurgundyRed}
                  inverted={true}
                  onButtonPress={() => handleEditCardPressed()}
                />
              </View>
            </>
          )}
        </View>
      </View>
    </Modal>
  );
}
