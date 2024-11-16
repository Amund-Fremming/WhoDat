import { View, Text, Alert } from "react-native";
import { useEffect, useState } from "react";
import { Colors } from "@/src/shared/assets/constants/Colors";
import { ICard } from "@/src/domain/CardTypes";
import Card from "./components/Card/Card";
import CardModal from "./components/CardModal/CardModal";
import styles from "./GalleryStyles";
import { AddCard } from "./components/AddCard/AddCard";
import AddCardModal from "./components/AddCardModal/AddCardModal";
import { deleteCard, getAllCards } from "@/src/infrastructure/CardClient";
import { useAuthProvider } from "@/src/shared/AuthProvider";
import MediumButton from "@/src/shared/components/MediumButton/MediumButton";

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
  const [allCards, setAllCards] = useState<ICard[]>([]);
  const [cardsForThisPage, setCardsForThisPage] = useState<ICard[]>([]);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [displayPrevious, setDisplayPrevious] = useState<boolean>(false);
  const [displayNext, setDisplayNext] = useState<boolean>(true);

  const { token } = useAuthProvider();

  useEffect(() => {
    fetchPlayerCards();
  }, [addCardModalVisible]);

  const fetchPlayerCards = async () => {
    try {
      const data = await getAllCards(token);
      setAllCards(data);

      const skip = (pageNumber - 1) * 20;
      const take = 20 * pageNumber;
      setCardsForThisPage(data.slice(skip, take));
    } catch (error) {
      console.error("Fetching allCards failed " + error);
    }
  };

  const handleCardPressed = (card: ICard) => {
    setCardPressed(card);
    setCardModalVisible(true);
  };

  const handleNextPressed = () => {
    const skip = pageNumber * 20;
    const take = 20 * (pageNumber + 1);
    const cardsToDisplay = allCards.slice(skip, take);

    if (cardsToDisplay.length < 20) {
      setDisplayNext(false);
    }

    setCardsForThisPage(cardsToDisplay);
    setPageNumber(pageNumber + 1);
    setDisplayPrevious(true);
  };

  const handlePreviousPressed = () => {
    setDisplayNext(true);
    const skip = (pageNumber - 2) * 20;
    const take = 20 * (pageNumber - 1);
    const cardsToDisplay = allCards.slice(skip, take);

    setCardsForThisPage(cardsToDisplay);
    setDisplayPrevious(pageNumber - 2 >= 1);
    setPageNumber(pageNumber - 1);
  };

  const handleDeleteCardPressed = async (card: ICard) => {
    Alert.alert("Are you sure?", `Do you want to delete ${card.name}`, [
      {
        text: "No",
        onPress: () => console.log("No Pressed"),
        style: "cancel",
      },
      {
        text: "Yes",
        onPress: async () => {
          setCardModalVisible(false);
          setAllCards([
            ...allCards.filter(
              (prevCard: ICard) => prevCard.cardID != card.cardID
            ),
          ]);

          try {
            await deleteCard(card.cardID, token);
          } catch (error) {
            // TODO - better handling
            Alert.alert("Something went wrong!", "Try again later");
          }
        },
      },
    ]);
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
            {cardsForThisPage.map((card: ICard) => (
              <Card
                key={card.cardID}
                card={card}
                onCardPress={() => handleCardPressed(card)}
              />
            ))}
            {cardsForThisPage.length < 20 && (
              <AddCard onAddCardPress={() => setAddCardModalVisible(true)} />
            )}
          </View>
          <View style={styles.buttonWrapper}>
            {displayPrevious && (
              <MediumButton
                text={"Prev"}
                color={Colors.BurgundyRed}
                inverted={false}
                onButtonPress={handlePreviousPressed}
              />
            )}
            {displayNext && (
              <MediumButton
                text={"Next"}
                color={Colors.BurgundyRed}
                inverted={false}
                onButtonPress={handleNextPressed}
              />
            )}
          </View>
        </View>
      </View>
    </>
  );
}
