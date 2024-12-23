import { View, Text, Pressable } from "react-native";
import { styles } from "./ChooseBoardPageStyles";
import { PlayPages } from "../../GamePages";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import { useEffect, useState } from "react";
import { useAuthProvider } from "@/src/Shared/state/AuthProvider";
import { getAllCards } from "@/src/Shared/functions/CardClient";
import Result from "@/src/Shared/domain/Result";
import { ICard } from "@/src/Shared/domain/CardTypes";
import MediumButton from "@/src/Shared/components/MediumButton/MediumButton";
import Card from "./components/Card/Card";

interface BoardPageProps {
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  cardsToChoose: number;
}

export default function ChooseBoardPage({
  setPage,
  cardsToChoose
}: BoardPageProps) {

  const [errorModalVisible, setErrorModalVisible] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [allCards, setAllCards] = useState<ICard[]>([]);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [cardsForThisPage, setCardsForThisPage] = useState<ICard[]>([]);
  const [displayNext, setDisplayNext] = useState<boolean>(true);
  const [displayPrevious, setDisplayPrevious] = useState<boolean>(false);
  const [cardsChosen, setCardsChosen] = useState<number>(0);
  const [cardsPressed, setCardsPressed] = useState<Set<number>>(new Set());
  const { token } = useAuthProvider();

  const handleError = (message: string) => {
    setErrorModalVisible(true);
    setErrorMessage(message);
  };

  useEffect(() => {
    fetchPlayerCards();
  }, []);

  const fetchPlayerCards = async () => {
    const result: Result<Array<ICard>> = await getAllCards(token);
    if (result.isError) {
      handleError(result.message);
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

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Chosen {cardsChosen}/{cardsToChoose}</Text>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.creamContainer}>
      <View style={styles.boardContainer}>
            {cardsForThisPage.map((card: ICard, index: number) => (
              <Card
                key={index}
                setCardsChoosen={setCardsChosen}
                card={card}
                setCardsPressed={setCardsPressed}
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
