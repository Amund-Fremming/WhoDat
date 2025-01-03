import { View, Text, Pressable } from 'react-native';
import { styles } from './ChooseCardPageStyles';
import { PlayPages } from '../../types/GamePages';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useEffect, useState } from 'react';
import { useAuthProvider } from '@/src/Shared/providers/AuthProvider';
import Card from './components/BoardCard/BoardCard';
import { getBoardWithBoardCards } from '../../GameClient';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { IBoardCard } from '../../types/BoardTypes';
import BigButton from '@/src/Shared/components/BigButton/BigButton';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import { chooseBoardCard, leaveGame } from '../../GameHubClient';

export default function ChooseCardPage() {
  const [cardPressed, setCardPressed] = useState<number>(-1);
  const [cards, setCards] = useState<IBoardCard[]>([]);
  const { setPage, gameId, connection, setBoard, board } = useGameProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const { token } = useAuthProvider();
  const { setDisplayTabBar } = useTabBarProvider();

  useEffect(() => {
    fetchBoard();
  }, []);

  const fetchBoard = async () => {
    var result = await getBoardWithBoardCards(gameId, token);
    if (result.isError) {
      toggleInfoModal(true, result.message);
      return;
    }
    setBoard(result.data!);
    setCards(result.data?.boardCards!);
  };

  const handleChooseCard = async () => {
    if (cardPressed === -1) {
      toggleInfoModal(false, 'Please choose a card.');
      return;
    }

    if (connection && board)
      await chooseBoardCard(connection, gameId, board.id, cardPressed);
  };

  const handleBackPressed = async () => {
    setPage(PlayPages.MAIN_PAGE);
    setDisplayTabBar('flex');
    if (connection) await leaveGame(connection, gameId);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Chose a card</Text>
      <Pressable style={styles.backIconWrapper} onPress={handleBackPressed}>
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.creamContainer}>
        <View style={styles.boardContainer}>
          {cards.map((boardcard: IBoardCard, index: number) => (
            <Card
              key={index}
              boardcard={boardcard}
              handleCardPressed={() => setCardPressed(boardcard.id)}
              isActive={boardcard.id === cardPressed}
            />
          ))}
        </View>
        <View style={styles.buttonWrapper}>
          <BigButton
            text="Choose"
            color={Colors.BurgundyRed}
            inverted={false}
            onButtonPress={handleChooseCard}
          />
        </View>
      </View>
    </View>
  );
}
