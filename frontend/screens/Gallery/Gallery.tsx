import { View, Text } from "react-native";
import { useState } from "react";

import { Colors } from "@/constants/Colors";
import styles from "./GalleryStyles";
import { ICard } from "@/interfaces/ICard";

import BigButton from "@/components/BigButton/BigButton";
import Card from "./components/Card/Card";
import CardModal from "./components/CardModal/CardModal";

export default function Gallery() {
  const [cardModalVisible, setCardModalVisible] = useState<boolean>(false);
  const [cardPressed, setCardPressed] = useState<ICard | undefined>(undefined);
  const [cards, setCards] = useState<ICard[]>([]);

  const handleCardPressed = (card: ICard) => {
    setCardPressed(card);
    setCardModalVisible(true);
  };

  return (
    <>
      <CardModal
        modalVisible={cardModalVisible}
        setModalVisible={setCardModalVisible}
        card={cardPressed}
      />

      <View style={styles.container}>
        <Text style={styles.header}>Gallery</Text>
        <View style={styles.creamContainer}>
          <View style={styles.boardContainer}>
            {cards.map((card: ICard) => (
              <Card
                key={card.cardId}
                card={null}
                onCardPress={() => handleCardPressed(card)}
              />
            ))}
          </View>
          <BigButton
            text="Edit"
            color={Colors.BurgundyRed}
            inverted={false}
            onButtonPress={() => setCardModalVisible(true)}
          />
        </View>
      </View>
    </>
  );
}
