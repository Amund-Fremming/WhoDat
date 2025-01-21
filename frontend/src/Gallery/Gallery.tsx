import { View, Text, Alert } from 'react-native';
import { useEffect, useState } from 'react';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { ICardDto } from '@/src/Shared/types/CardTypes';
import Card from './components/Card/CardComponent';
import CardModal from './components/CardModal/CardModal';
import { viewStyles, textStyles } from './GalleryStyles';
import { AddCardComponent } from './components/AddCard/AddCardComponent';
import AddCardModal from './components/AddCardModal/AddCardModal';
import { deleteCard, getAllCards } from '@/src/Shared/functions/CardClient';
import { useAuthProvider } from '@/src/Shared/providers/AuthProvider';
import MediumButton from '@/src/Shared/components/MediumButton/MediumButton';
import { useInfoModalProvider } from '../Shared/providers/InfoModalProvider';
import { usePreloadProvider } from '../Shared/providers/PreloadProvider';

const defaultCard: ICardDto = {
  id: -1,
  name: 'Default',
  url: 'None',
};

export default function Gallery() {
  const [addCardModalVisible, setAddCardModalVisible] =
    useState<boolean>(false);
  const [cardModalVisible, setCardModalVisible] = useState<boolean>(false);
  const [cardPressed, setCardPressed] = useState<ICardDto>(defaultCard);
  const [cardsForThisPage, setCardsForThisPage] = useState<ICardDto[]>([]);
  const [pageNumber, setPageNumber] = useState<number>(1);
  const [displayPrevious, setDisplayPrevious] = useState<boolean>(false);
  const [displayNext, setDisplayNext] = useState<boolean>(false);
  const [justAddedCard, setJustAddedCard] = useState<ICardDto | undefined>(
    undefined
  );
  const { token } = useAuthProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const { allGalleryCards, setAllGalleryCards } = usePreloadProvider();

  useEffect(() => {
    fetchPlayerCards();
  }, []);

  useEffect(() => {
    if (justAddedCard) {
      setAllGalleryCards((prev) => [...prev, justAddedCard]);
      if (cardsForThisPage.length < 20) {
        setCardsForThisPage((prev) => [...prev, justAddedCard]);
      }
      setJustAddedCard(undefined);
    }
  }, [justAddedCard]);

  const fetchPlayerCards = async () => {
    var data = allGalleryCards;

    const skip = (pageNumber - 1) * 20;
    const take = 20 * pageNumber;
    setCardsForThisPage(data!.slice(skip, take));
    if (data!.length > 19) setDisplayNext(true);
  };

  const handleCardPressed = (card: ICardDto) => {
    setCardPressed(card);
    setCardModalVisible(true);
  };

  const handleNextPressed = () => {
    const skip = pageNumber * 20;
    const take = 20 * (pageNumber + 1);
    const cardsToDisplay = allGalleryCards.slice(skip, take);

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
    const cardsToDisplay = allGalleryCards.slice(skip, take);

    setCardsForThisPage(cardsToDisplay);
    setDisplayPrevious(pageNumber - 2 >= 1);
    setPageNumber(pageNumber - 1);
  };

  const handleDeleteCardPressed = async (card: ICardDto) => {
    Alert.alert('Are you sure?', `Do you want to delete ${card.name}`, [
      {
        text: 'No',
        style: 'cancel',
      },
      {
        text: 'Yes',
        onPress: async () => {
          setCardModalVisible(false);
          const result = await deleteCard(card.id, token);
          if (result.isError) {
            toggleInfoModal(true, result.message);
            return;
          }
          setAllGalleryCards((prev) =>
            prev.filter((prevCard: ICardDto) => prevCard.id != card.id)
          );
          setCardsForThisPage((prev) =>
            prev.filter((prevCard: ICardDto) => prevCard.id != card.id)
          );
        },
      },
    ]);
  };

  return (
    <View>
      <CardModal
        modalVisible={cardModalVisible}
        setModalVisible={setCardModalVisible}
        card={cardPressed}
        onDeleteCardPressed={async () => handleDeleteCardPressed(cardPressed)}
      />

      <AddCardModal
        modalVisible={addCardModalVisible}
        setModalVisible={setAddCardModalVisible}
        setJustAddedCard={setJustAddedCard}
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
            {cardsForThisPage.map((card: ICardDto, index: number) => (
              <Card
                key={index}
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
    </View>
  );
}
