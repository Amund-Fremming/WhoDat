import { ICard } from "@/interfaces/ICard";
import { View, Image, Pressable } from "react-native";
import { styles, imageStyles } from "./CardStyles";
import StrokedText from "@/components/StokedText/StrokedText";

interface CardProps {
  card: ICard;
  onCardPress: () => void;
}

export default function Card({ card, onCardPress }: CardProps) {
  return (
    <Pressable style={styles.container} onPress={onCardPress}>
      <View style={styles.card}>
        <Image
          style={imageStyles.imageStyle}
          source={{
            uri: "https://www.wikihow.com/images/thumb/9/90/What_type_of_person_are_you_quiz_pic.png/1200px-What_type_of_person_are_you_quiz_pic.png",
          }}
        />
      </View>
      <StrokedText text={card.name} fontBaseSize={14} smallBorder={true} />
    </Pressable>
  );
}
