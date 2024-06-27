import { Modal, View, Image } from "react-native";
import { styles, imageStyles } from "./CardModalStyles";
import BigButton from "@/components/BigButton/BigButton";
import { Colors } from "@/constants/Colors";
import StrokedText from "@/components/StokedText/StrokedText";
import { ICard } from "@/interfaces/ICard";

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
          <View style={styles.card}>
            <Image
              style={imageStyles.imageStyle}
              source={{
                uri: "https://www.wikihow.com/images/thumb/9/90/What_type_of_person_are_you_quiz_pic.png/1200px-What_type_of_person_are_you_quiz_pic.png",
              }}
            />
          </View>
          <StrokedText text={"Monsen"} fontBaseSize={40} stokeWidth={4} />
          <BigButton
            text="Edit"
            color={Colors.BurgundyRed}
            inverted={false}
            onButtonPress={() => console.log("editing...")}
          />
        </View>
      </View>
    </Modal>
  );
}
