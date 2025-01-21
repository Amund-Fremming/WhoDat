import { View, Text, Pressable } from 'react-native';
import { styles } from './ChooseBoardPageStyles';
import { PlayPages } from '../../types/GamePages';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useEffect, useState } from 'react';
import { useAuthProvider } from '@/src/Shared/providers/AuthProvider';
import { getAllCards } from '@/src/Shared/functions/CardClient';
import Result from '@/src/Shared/objects/Result';
import { ICardDto } from '@/src/Shared/types/CardTypes';
import MediumButton from '@/src/Shared/components/MediumButton/MediumButton';
import Card from './components/Card/Card';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { createBoardCards, leaveGame } from '../../GameHubClient';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import { usePreloadProvider } from '@/src/Shared/providers/PreloadProvider';

interface BoardPageProps {
  cardsToChoose: number;
}

export default function ChooseBoardPage({ cardsToChoose }: BoardPageProps) {
  const [allCards, setAllCards] = useState<ICardDto[]>([]);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [cardsForThisPage, setCardsForThisPage] = useState<ICardDto[]>([]);
  const [displayUserMessage, setDisplayUserMessage] = useState<boolean>(false);
  const [displayNext, setDisplayNext] = useState<boolean>(true);
  const [displayPrevious, setDisplayPrevious] = useState<boolean>(false);
  const [cardsPressed, setCardsPressed] = useState<number[]>([]);
  const { token } = useAuthProvider();
  const { setPage, connection, gameId } = useGameProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const { setDisplayTabBar } = useTabBarProvider();
  const { allGalleryCards } = usePreloadProvider();

  useEffect(() => {
    fetchPlayerCards();
  }, []);

  const handleCreateBoardcards = async (cardIds: number[]) => {
    if (connection) {
      var result = await createBoardCards(connection, gameId, cardIds);
      if (result.isError) {
        toggleInfoModal(true, result.message);
      }
    }
  };

  const fetchPlayerCards = async () => {
    if (allGalleryCards.length < 20) {
      setDisplayUserMessage(true);
    }

    if (allGalleryCards.length <= 20) {
      setDisplayNext(false);
    } else {
      setDisplayNext(true);
    }
    setAllCards(allGalleryCards!);

    const skip = (pageNumber - 1) * 20;
    const take = 20 * pageNumber;
    setCardsForThisPage(allGalleryCards!.slice(skip, take));
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

  const handleDonePressed = () => {
    if (cardsPressed.length != cardsToChoose) {
      toggleInfoModal(true, `Please choose  ${cardsToChoose} cards!`);
      return;
    }
    handleCreateBoardcards(cardsPressed);
  };

  const handleCardPressed = (cardId: number) => {
    if (
      cardsPressed.length == cardsToChoose &&
      cardsPressed.filter((item) => item == cardId).length == 0
    ) {
      toggleInfoModal(true, `You can only choose ${cardsToChoose} cards!`);
      return;
    }

    const isActive =
      cardsPressed.filter((id: number) => id == cardId).length > 0;

    if (!isActive) {
      setCardsPressed((prev) => [...prev, cardId]);
    } else {
      setCardsPressed((prev) => prev.filter((id) => id != cardId));
    }
  };

  const handleBackPressed = async () => {
    setPage(PlayPages.MAIN_PAGE);
    setDisplayTabBar('flex');
    if (connection) await leaveGame(connection, gameId, true);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>
        Chosen {cardsPressed.length}/{cardsToChoose}
      </Text>
      <Pressable style={styles.backIconWrapper} onPress={handleBackPressed}>
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.creamContainer}>
        <View style={styles.boardContainer}>
          {cardsForThisPage.map((card: ICardDto, index: number) => (
            <Card
              key={index}
              card={card}
              handleCardPressed={() => handleCardPressed(card.id)}
              isActive={
                cardsPressed.filter((id: number) => id == card.id).length > 0
              }
            />
          ))}
          {cardsForThisPage.length == 0 && displayUserMessage && (
            <Text style={styles.infoText}>
              You need to create cards in your gallery to be able to play!
            </Text>
          )}
        </View>
        <View style={styles.buttonWrapper}>
          {pageNumber == 1 && (
            <MediumButton
              text={'Done'}
              color={Colors.Green}
              inverted={false}
              onButtonPress={handleDonePressed}
            />
          )}
          {displayPrevious && (
            <MediumButton
              text={'Prev'}
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={handlePreviousPressed}
            />
          )}
          {displayNext && (
            <MediumButton
              text={'Next'}
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
