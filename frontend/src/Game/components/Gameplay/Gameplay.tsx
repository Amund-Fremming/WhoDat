import { Image, Pressable, Text, View } from 'react-native';
import { imageStyles, styles } from './GameplayStyles';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { PlayPages } from '../../types/GamePages';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import { guessBoardCard, leaveGame } from '../../GameHubClient';
import { useEffect, useState } from 'react';
import FlipCard from './components/FlipCard/FlipCard';
import MediumButton from '@/src/Shared/components/MediumButton/MediumButton';
import React from 'react';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';

export default function Gameplay() {
  const { setPage, connection, gameId, gameState, isHost, board } =
    useGameProvider();
  const { setDisplayTabBar } = useTabBarProvider();
  const { toggleInfoModal } = useInfoModalProvider();

  const [header, setHeader] = useState<string>('');
  const [thisPlayerTurn, setThisPlayerTurn] = useState<boolean>(isHost);
  const [guessMode, setGuessMode] = useState<boolean>(false);
  const [cardActive, setCardsActive] = useState<number[]>([]);
  const [cardToGuess, setCardToGuess] = useState<number>(-1);

  useEffect(() => {
    var stateP1Turn = [2, 5, 8, 9, 10, 11, 12];
    var stateP2Turn = [3, 6, 13, 14, 15, 16, 17];

    if (stateP1Turn.includes(gameState)) {
      setHeader(isHost ? 'Your turn' : 'Their turn');
      setThisPlayerTurn(isHost ? true : false);
    }
    if (stateP2Turn.includes(gameState)) {
      setHeader(isHost ? 'Their turn' : 'Your turn');
      setThisPlayerTurn(isHost ? false : true);
    }
  }, [gameState]);

  const handleBackPressed = async () => {
    // TODO: are you sure modal
    setPage(PlayPages.MAIN_PAGE);
    setDisplayTabBar('flex');
    if (connection) await leaveGame(connection, gameId);
  };

  const handleCardPressed = (boardcardId: number) => {
    if (!guessMode) {
      setCardsActive((prev) => [...prev, boardcardId]);
      return;
    }

    setCardToGuess(boardcardId);
  };

  const handleAskPressed = () => {
    //
  };

  const handleTakeGuessPressed = async () => {
    if (connection) {
      const result = await guessBoardCard(connection, gameId, cardToGuess);
      if (result.isError) {
        toggleInfoModal(true, result.message);
      }
    }
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
              onCardPress={() => handleCardPressed(bc.id)}
              cardToGuess={cardToGuess}
              boardcard={bc}
              guessMode={guessMode}
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
            {thisPlayerTurn && !guessMode && (
              <>
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
                  onButtonPress={() => setGuessMode(true)}
                />
              </>
            )}
            {thisPlayerTurn && guessMode && (
              <>
                <MediumButton
                  text="Cancel"
                  inverted={true}
                  color={Colors.BurgundyRed}
                  onButtonPress={() => setGuessMode(false)}
                />
                <MediumButton
                  text="Take guess"
                  inverted={false}
                  color={Colors.Green}
                  onButtonPress={handleTakeGuessPressed}
                />
              </>
            )}
          </View>
        </View>
      </View>
    </View>
  );
}
