import { ICard } from "@/interfaces/ICard";
import { View, Text } from "react-native";

interface CardProps {
  card: ICard;
  onCardPress: () => void;
}

export default function Card({}: CardProps) {
  return (
    <View>
      <Text>Card</Text>
    </View>
  );
}
