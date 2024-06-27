import { ICard } from "@/interfaces/ICard";
import { View, Text, Image } from "react-native";
import styles from "./CardStyles";
import StrokedText from "@/components/StokedText/StrokedText";

interface CardProps {
  card: ICard | null;
  onCardPress: () => void;
}

export default function Card({ card, onCardPress }: CardProps) {
  return (
    <View style={styles.container}>
      <View style={styles.outerRim}>
        <View style={styles.innerRim}>
          {/*
          <Image
            style={styles.imageStyle}
            source={{
              uri: "https://www.wikihow.com/images/thumb/9/90/What_type_of_person_are_you_quiz_pic.png/1200px-What_type_of_person_are_you_quiz_pic.png",
            }}
          />
        */}
        </View>
      </View>
      <StrokedText text={"Card"} />
    </View>
  );
}
