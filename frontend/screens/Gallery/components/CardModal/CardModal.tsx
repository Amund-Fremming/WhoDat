import { Modal, View, Image, Pressable } from "react-native";
import { styles, imageStyles } from "./CardModalStyles";
import BigButton from "@/components/BigButton/BigButton";
import { Colors } from "@/constants/Colors";
import StrokedText from "@/components/StokedText/StrokedText";
import { ICard } from "@/interfaces/ICard";
import FontAwesome from "@expo/vector-icons/FontAwesome";

interface CardModalProps {
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
  card: ICard | undefined;
}

export default function CardModal({
  modalVisible,
  setModalVisible,
  card,
}: CardModalProps) {
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
          <StrokedText text={"Monsen"} fontBaseSize={40} smallBorder={false} />
          <View style={styles.buttonWrapper}>
            <BigButton
              text="Edit"
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={() => console.log("editing...")}
            />
          </View>
        </View>
      </View>
    </Modal>
  );
}
