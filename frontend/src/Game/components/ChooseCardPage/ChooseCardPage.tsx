import { View, Text, Pressable } from 'react-native';
import { styles } from './ChooseCardPageStyles';
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
import { getBoardWithBoardCards } from '../../GameClient';

interface CardPageProps {
  handleError: (message: string, redirect: boolean) => void;
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
  fetchBoard: () => {};
}

export default function ChooseCardPage({
  handleError,
  setPage,
  fetchBoard,
}: CardPageProps) {
  const [cardPressed, setCardPressed] = useState<number>(-1);
  const [cards, setCards] = useState<ICardDto[]>([]);

  useEffect(() => {
    fetchBoard();
  }, []);

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Chose a card</Text>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.creamContainer}>
        <View style={styles.boardContainer}>
          {cards.map((card: ICardDto, index: number) => (
            <Card
              key={index}
              card={card}
              handleCardPressed={() => setCardPressed(card.id)}
              isActive={card.id === cardPressed}
            />
          ))}
        </View>
      </View>
    </View>
  );
}
