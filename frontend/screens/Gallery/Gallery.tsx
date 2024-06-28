import { View, Text, Pressable } from "react-native";
import { useEffect, useState } from "react";

import { Colors } from "@/constants/Colors";
import styles from "./GalleryStyles";
import { ICard } from "@/interfaces/ICard";

import BigButton from "@/components/BigButton/BigButton";
import Card from "./components/Card/Card";
import CardModal from "./components/CardModal/CardModal";

import { mockCards } from "@/constants/Mockdata";
import { opacity } from "react-native-reanimated/lib/typescript/reanimated2/Colors";

const defaultCard: ICard = {
  cardId: -1,
  galleryID: -1,
  name: "Default",
  url: "None",
};

export default function Gallery() {
  const [editMode, setEditMode] = useState<boolean>(false);
  const [cardModalVisible, setCardModalVisible] = useState<boolean>(false);
  const [cardPressed, setCardPressed] = useState<ICard>(defaultCard);
  const [cards, setCards] = useState<ICard[]>([]);

  useEffect(() => {
    // Fetch cards
    setCards(mockCards);
  }, []);

  const handleCardPressed = (card: ICard) => {
    setCardPressed(card);
    setCardModalVisible(true);
  };

  const handleDeleteCardPressed = (card: ICard) => {
    // Delete card from db
    setCards([
      ...cards.filter((prevCard: ICard) => prevCard.cardId != card.cardId),
    ]);
  };

  return (
    <>
      <CardModal
        modalVisible={cardModalVisible}
        setModalVisible={setCardModalVisible}
        card={cardPressed}
      />

      <View
        style={{
          ...styles.container,
          opacity: cardModalVisible ? 0.6 : 1,
        }}
      >
        <Text style={styles.header}>Gallery</Text>
        <View style={styles.creamContainer}>
          <View style={styles.boardContainer}>
            {!editMode &&
              cards.map((card: ICard) => (
                <Card
                  key={card.cardId}
                  card={card}
                  onCardPress={() => handleCardPressed(card)}
                />
              ))}
            {editMode &&
              cards.map((card: ICard) => (
                <Pressable onPress={() => handleDeleteCardPressed(card)}>
                  <Card
                    key={card.cardId}
                    card={card}
                    onCardPress={() => handleCardPressed(card)}
                  />
                </Pressable>
              ))}
          </View>
          <BigButton
            text={editMode ? "Save" : "Edit"}
            color={Colors.BurgundyRed}
            inverted={editMode ? true : false}
            onButtonPress={() => setEditMode(!editMode)}
          />
        </View>
      </View>
    </>
  );
}
