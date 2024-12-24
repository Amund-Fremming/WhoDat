import { View, Text, Pressable } from "react-native";
import { styles } from "./ChooseBoardPageStyles";
import { PlayPages } from "../../GamePages";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import { useEffect, useState } from "react";
import { useAuthProvider } from "@/src/Shared/state/AuthProvider";
import { getAllCards } from "@/src/Shared/functions/CardClient";
import Result from "@/src/Shared/domain/Result";
import { ICardDto } from "@/src/Shared/domain/CardTypes";
import MediumButton from "@/src/Shared/components/MediumButton/MediumButton";
import Card from "./components/Card/Card";

interface BoardPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  cardsToChoose: number;
  handleError: (message: string, redirect: boolean) => void;
}

export default function ChooseBoardPage({
  setPage,
  cardsToChoose,
  handleError
}: BoardPageProps) {

  const [allCards, setAllCards] = useState<ICardDto[]>([]);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [cardsForThisPage, setCardsForThisPage] = useState<ICardDto[]>([]);
  const [displayNext, setDisplayNext] = useState<boolean>(true);
  const [displayPrevious, setDisplayPrevious] = useState<boolean>(false);
  const [cardsPressed, setCardsPressed] = useState<number[]>([]);
  const { token } = useAuthProvider();

  useEffect(() => {
    fetchPlayerCards();
  }, []);

  const fetchPlayerCards = async () => {
    const result: Result<Array<ICardDto>> = await getAllCards(token);
    if (result.isError) {
      handleError(result.message, true);
    }

    const data = result.data;
    if(data!.length <= 20) {
      setDisplayNext(false); 
    } else {
      setDisplayNext(true);
    }
    setAllCards(data!);

    const skip = (pageNumber - 1) * 20;
    const take = 20 * pageNumber;
    setCardsForThisPage(data!.slice(skip, take));
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

  const handleCardPressed = (cardId: number) => {
    if(cardsPressed.length == cardsToChoose) {
      handleError(`You can only choose ${cardsToChoose} cards!`, false);
      return;
    }

    const isActive = cardsPressed.filter((id: number) => id == cardId).length > 0;

    if(!isActive) {
      setCardsPressed(prev => [...prev, cardId]); 
   } else {
      setCardsPressed(prev => prev.filter(id => id != cardId
      )); 
   }
  }

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Chosen {cardsPressed.length}/{cardsToChoose}</Text>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.creamContainer}>
      <View style={styles.boardContainer}>
            {cardsForThisPage.map((card: ICardDto, index: number) => (
              <Card
                key={index}
                card={card}
                handleCardPressed={() => handleCardPressed(card.id)}
                isActive={cardsPressed.filter((id: number) => id == card.id).length > 0}
              />
            ))}
            {cardsForThisPage.length == 0 && (
              <Text style={styles.infoText}>You need to create cards in your gallery to be able to play!</Text>
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
  );
}
