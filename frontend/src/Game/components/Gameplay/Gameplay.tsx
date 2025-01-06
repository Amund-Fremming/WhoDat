import { Image, Pressable, Text, View } from 'react-native';
import { imageStyles, styles } from './GameplayStyles';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { PlayPages } from '../../types/GamePages';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import { leaveGame } from '../../GameHubClient';
import { useEffect, useState } from 'react';
import FlipCard from './components/FlipCard/FlipCard';
import MediumButton from '@/src/Shared/components/MediumButton/MediumButton';

export default function Gameplay() {
  const [header, setHeader] = useState<string>('');
  const { setPage, connection, gameId, gameState, isHost, board } =
    useGameProvider();
  const { setDisplayTabBar } = useTabBarProvider();

  useEffect(() => {
    var stateP1Turn = [2, 5, 8, 9, 10, 11, 12];
    var stateP2Turn = [3, 6, 13, 14, 15, 16, 17];

    if (stateP1Turn.includes(gameState)) {
      setHeader(isHost ? 'Your turn' : 'Their turn');
    }
    if (stateP2Turn.includes(gameState)) {
      setHeader(isHost ? 'Their turn' : 'Your turn');
    }
  }, [gameState]);

  const handleBackPressed = async () => {
    // TODO: are you sure modal
    setPage(PlayPages.MAIN_PAGE);
    setDisplayTabBar('flex');
    if (connection) await leaveGame(connection, gameId);
  };

  const handleCardPressed = () => {
    //
  };

  const handleAskPressed = () => {
    //
  };

  const handleGuessPressed = () => {
    //
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>{header}</Text>
      <View style={styles.subHeaderWrapper}>
        <Text style={{ ...styles.header2, color: Colors.Green }}>20</Text>
        <Text style={{ ...styles.header2, color: Colors.Cream }}>vs</Text>
        <Text style={{ ...styles.header2, color: Colors.BurgundyRed }}>20</Text>
      </View>
      <Pressable style={styles.backIconWrapper} onPress={handleBackPressed}>
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.creamContainer}>
        <View style={styles.boardContainer}>
          {board?.boardCards?.map((bc) => (
            <FlipCard
              key={bc.id}
              onCardPress={handleCardPressed}
              boardcard={bc}
            />
          ))}
        </View>
        <View style={styles.controlPanel}>
          <View style={styles.chosenCardOuter}>
            <Image
              style={imageStyles.chosenCardInner}
              source={{
                uri: board?.chosenCard?.card.url,
              }}
            />
          </View>
          <View style={styles.controlButtonWrapper}>
            <MediumButton
              text="Ask"
              inverted={false}
              color={Colors.BurgundyRed}
              onButtonPress={handleAskPressed}
            />
            <MediumButton
              text="Guess"
              inverted={false}
              color={Colors.BurgundyRed}
              onButtonPress={handleGuessPressed}
            />
          </View>
        </View>
      </View>
    </View>
  );
}
