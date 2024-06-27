import { ICard } from "@/interfaces/ICard";
import { View, Image } from "react-native";
import { styles, imageStyles } from "./CardStyles";
import StrokedText from "@/components/StokedText/StrokedText";

interface CardProps {
  card: ICard | null;
  onCardPress: () => void;
}

export default function Card({ card, onCardPress }: CardProps) {
  const names = [
    "Emily",
    "Brandon",
    "Victoria",
    "Michael",
    "Samantha",
    "Jonathan",
    "Isabella",
    "Benjamin",
    "Nicholas",
    "Mmmmmmmm",
    "Mmm",
  ];

  const getRandomName = (): string => {
    const name: string | undefined = names.at(Math.random() * names.length);
    if (name) return name;
    else return "Card";
  };

  return (
    <View style={styles.container}>
      <View style={styles.card}>
        <Image
          style={imageStyles.imageStyle}
          source={{
            uri: "https://www.wikihow.com/images/thumb/9/90/What_type_of_person_are_you_quiz_pic.png/1200px-What_type_of_person_are_you_quiz_pic.png",
          }}
        />
      </View>
      <StrokedText
        text={getRandomName()}
        fontBaseSize={14}
        smallBorder={true}
      />
    </View>
  );
}
