import { View, Text, Alert } from "react-native";
import { useEffect, useState } from "react";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import { ICard } from "@/src/Shared/domain/CardTypes";
import Card from "./components/Card/CardComponent";
import CardModal from "./components/CardModal/CardModal";
import { viewStyles, textStyles } from "./GalleryStyles";
import { AddCardComponent } from "./components/AddCard/AddCardComponent";
import AddCardModal from "./components/AddCardModal/AddCardModal";
import { deleteCard, getAllCards } from "@/src/Shared/functions/CardClient";
import { useAuthProvider } from "@/src/Shared/state/AuthProvider";
import MediumButton from "@/src/Shared/components/MediumButton/MediumButton";
import Result from "@/src/Shared/domain/Result";
import ErrorModal from "@/src/Shared/components/ErrorModal/ErrorModal";

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
  const [errorModalVisible, setErrorModalVisible] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");

  const handleError = (message: string) => {
    setErrorModalVisible(true);
    setErrorMessage(message);
  };

  const { token } = useAuthProvider();

  useEffect(() => {
    fetchPlayerCards();
  }, [addCardModalVisible]);

  const fetchPlayerCards = async () => {
    const result: Result<Array<ICard>> = await getAllCards(token);
    if (result.isError) {
      handleError(result.message);
    }

    const data = result.data;
    setAllCards(data!);

    const skip = (pageNumber - 1) * 20;
    const take = 20 * pageNumber;
    setCardsForThisPage(data!.slice(skip, take));
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
            handleError("Something went wrong.");
          }
        },
      },
    ]);
  };

  return (
    <>
      <ErrorModal
        errorModalVisible={errorModalVisible}
        setErrorModalVisible={setErrorModalVisible}
        message={errorMessage}
      />

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
          ...viewStyles.container,
          opacity: cardModalVisible ? 0.6 : 1,
        }}
      >
        <Text style={textStyles.header}>Gallery</Text>
        <View style={viewStyles.creamContainer}>
          <View style={viewStyles.boardContainer}>
            {cardsForThisPage.map((card: ICard) => (
              <Card
                key={card.cardID}
                card={card}
                onCardPress={() => handleCardPressed(card)}
              />
            ))}
            {cardsForThisPage.length < 20 && (
              <AddCardComponent
                onAddCardPress={() => setAddCardModalVisible(true)}
              />
            )}
          </View>
          <View style={viewStyles.buttonWrapper}>
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
