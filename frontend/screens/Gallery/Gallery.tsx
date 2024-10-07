import { View, Text, Alert } from "react-native";
import { useEffect, useState } from "react";
import { Colors } from "@/constants/Colors";
import { ICard } from "@/interfaces/CardTypes";
import BigButton from "@/components/BigButton/BigButton";
import Card from "./components/Card/Card";
import CardModal from "./components/CardModal/CardModal";
import styles from "./GalleryStyles";
import { AddCard } from "./components/AddCard/AddCard";
import AddCardModal from "./components/AddCardModal/AddCardModal";
import { deleteCard, getAllCards } from "@/api/CardApi";
import { useAuthProvider } from "@/providers/AuthProvider";

const defaultCard: ICard = {
  cardID: -1,
  name: "Default",
  url: "None",
};

export default function Gallery() {
  const [addCardModalVisible, setAddCardModalVisible] =
    useState<boolean>(false);
  const [cardModalVisible, setCardModalVisible] = useState<boolean>(false);
  const [cardPressed, setCardPressed] = useState<ICard>(defaultCard);
  const [cards, setCards] = useState<ICard[]>([]);

  const { token } = useAuthProvider();

  useEffect(() => {
    fetchPlayerCards();
  }, [addCardModalVisible]);

  const fetchPlayerCards = async () => {
    try {
      const data = await getAllCards(token);
      setCards(data);
    } catch (error) {
      console.error("Fetching cards failed " + error);
    }
  };

  const handleCardPressed = (card: ICard) => {
    setCardPressed(card);
    setCardModalVisible(true);
  };

  const handleDeleteCardPressed = async (card: ICard) => {
    setCardModalVisible(false);
    setCards([
      ...cards.filter((prevCard: ICard) => prevCard.cardID != card.cardID),
    ]);

    try {
      await deleteCard(card.cardID, token);
    } catch (error) {
      // TODO - better handling
      Alert.alert("Something went wrong!", "Try again later");
    }
  };

  return (
    <>
      <CardModal
        modalVisible={cardModalVisible}
        setModalVisible={setCardModalVisible}
        card={cardPressed}
        onDeleteCardPressed={async () => handleDeleteCardPressed(cardPressed)}
      />

      <AddCardModal
        modalVisible={addCardModalVisible}
        setModalVisible={setAddCardModalVisible}
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
            {cards.map((card: ICard) => (
              <Card
                key={card.cardID}
                card={card}
                onCardPress={() => handleCardPressed(card)}
              />
            ))}
            {cards.length < 20 && (
              <AddCard onAddCardPress={() => setAddCardModalVisible(true)} />
            )}
          </View>
          <View style={styles.buttonWrapper}>
            <BigButton
              text={"Next"}
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={() => console.log("next pressed")}
            />
          </View>
        </View>
      </View>
    </>
  );
}
