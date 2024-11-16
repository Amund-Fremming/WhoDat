import { ICard } from "@/src/domain/CardTypes";
import { View, Image, Pressable } from "react-native";
import { styles, imageStyles } from "./CardStyles";
import StrokedText from "@/src/shared/components/StokedText/StrokedText";

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
            uri: card.url,
          }}
        />
      </View>
      <StrokedText text={card.name} fontBaseSize={14} smallBorder={true} />
    </Pressable>
  );
}
