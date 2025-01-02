import { View, Text, Pressable } from 'react-native';
import { styles } from './ChooseCardPageStyles';
import { PlayPages } from '../../types/GamePages';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useEffect, useState } from 'react';
import { useAuthProvider } from '@/src/Shared/providers/AuthProvider';
import { ICardDto } from '@/src/Shared/types/CardTypes';
import Card from './components/BoardCard/BoardCard';
import { getBoardWithBoardCards } from '../../GameClient';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { IBoardCard } from '../../types/BoardTypes';

export default function ChooseCardPage() {
  const [cardPressed, setCardPressed] = useState<number>(-1);
  const [cards, setCards] = useState<IBoardCard[]>([]);
  const { setPage, gameId } = useGameProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const { token } = useAuthProvider();

  useEffect(() => {
    fetchBoard();
  }, []);

  const fetchBoard = async () => {
    var result = await getBoardWithBoardCards(gameId, token);
    if (result.isError) {
      toggleInfoModal(true, result.message);
      return;
    }
    console.log(result.data?.boardCards);
    setCards(result.data?.boardCards!);
  };

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
          {cards.map((boardcard: IBoardCard, index: number) => (
            <Card
              key={index}
              boardcard={boardcard}
              handleCardPressed={() => setCardPressed(boardcard.ID)}
              isActive={boardcard.ID === cardPressed}
            />
          ))}
        </View>
      </View>
    </View>
  );
}
