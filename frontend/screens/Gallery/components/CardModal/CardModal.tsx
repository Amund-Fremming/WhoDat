import { Modal, View, Image, Pressable } from "react-native";
import { styles, imageStyles } from "./CardModalStyles";
import BigButton from "@/components/BigButton/BigButton";
import { Colors } from "@/constants/Colors";
import StrokedText from "@/components/StokedText/StrokedText";
import { ICard } from "@/interfaces/CardTypes";
import FontAwesome from "@expo/vector-icons/FontAwesome";
import { useEffect, useState } from "react";
import { MaterialCommunityIcons } from "@expo/vector-icons";

interface CardModalProps {
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
  card: ICard;
  onDeleteCardPressed: () => void;
}

export default function CardModal({
  modalVisible,
  setModalVisible,
  card,
  onDeleteCardPressed,
}: CardModalProps) {
  const [editMode, setEditMode] = useState<boolean>(false);

  useEffect(() => {
    setEditMode(false);
  }, [modalVisible]);

  const handleEditCardPressed = () => {
    setEditMode(false);
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
            {editMode && (
              <Pressable
                style={styles.deleteButton}
                onPress={onDeleteCardPressed}
              >
                <MaterialCommunityIcons
                  name="close-circle"
                  size={45}
                  color={Colors.Red}
                />
              </Pressable>
            )}
            <Image
              style={imageStyles.imageStyle}
              source={{
                uri: card.url,
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
              <StrokedText
                text={card.name}
                fontBaseSize={40}
                smallBorder={false}
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
