import { View, Text } from "react-native";
import { useEffect, useState } from "react";
import { Colors } from "@/constants/Colors";
import { ICard, ICardInputDto } from "@/interfaces/GalleryTypes";
import BigButton from "@/components/BigButton/BigButton";
import Card from "./components/Card/Card";
import CardModal from "./components/CardModal/CardModal";
import styles from "./GalleryStyles";
import { AddCard } from "./components/AddCard/AddCard";
import AddCardModal from "./components/AddCardModal/AddCardModal";
import { GetAllPlayerCards, addCardToGallery } from "@/api/GalleryApi";
import { useAuthProvider } from "@/providers/AuthProvider";

const defaultCard: ICard = {
  cardId: -1,
  galleryID: -1,
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
  }, []);

  const fetchPlayerCards = async () => {
    try {
      const data = await GetAllPlayerCards(token);
      setCards(data);
    } catch (error) {
      console.error("Fetching cards failed");
    }
  };

  const handleCardPressed = (card: ICard) => {
    setCardPressed(card);
    setCardModalVisible(true);
  };

  const handleDeleteCardPressed = (card: ICard) => {
    // Delete card from db
    setCardModalVisible(false);
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
        onDeleteCardPressed={() => handleDeleteCardPressed(cardPressed)}
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
                key={card.cardId}
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
