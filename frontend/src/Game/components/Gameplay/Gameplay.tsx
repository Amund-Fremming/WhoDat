import { Pressable, Text, View } from 'react-native';
import { Image } from 'expo-image';
import { imageStyles, styles } from './GameplayStyles';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { PlayPages } from '../../types/GamePages';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import {
  finishTurn,
  guessBoardCard,
  leaveGame,
  updateBoardCardsActivity,
} from '../../GameHubClient';
import { useEffect, useState } from 'react';
import FlipCard from './components/FlipCard/FlipCard';
import MediumButton from '@/src/Shared/components/MediumButton/MediumButton';
import React from 'react';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import ActionModal from './components/ActionModal/ActionModal';
import { IBoardCardUpdate } from '../../types/BoardTypes';
import { useGameplayProvider } from '@/src/Shared/providers/GameplayProvider';
import { GameState } from '../../types/GameTypes';

const blurhash =
  '|rF?hV%2WCj[ayj[a|j[az_NaeWBj@ayfRayfQfQM{M|azj[azf6fQfQfQIpWXofj[ayj[j[fQayWCoeoeaya}j[ayfQa{oLj?j[WVj[ayayj[fQoff7azayj[ayj[j[ayofayayayj[fQj[ayayj[ayfjj[j[ayjuayj[';

export default function Gameplay() {
  const {
    setPage,
    connection,
    gameId,
    gameState,
    isHost,
    board,
    oponentCardsLeft,
  } = useGameProvider();
  const { setDisplayTabBar } = useTabBarProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const { setAskModalVisible } = useGameplayProvider();

  const [header, setHeader] = useState<string>('');
  const [thisPlayerTurn, setThisPlayerTurn] = useState<boolean>(isHost);
  const [playerTurnFinished, setPlayerTurnFinished] = useState<boolean>(false);
  const [guessMode, setGuessMode] = useState<boolean>(false);
  const [actionModalVisible, setActionModalVisible] = useState<boolean>(false);
  const [gameFinsihed, setGameFinished] = useState<boolean>(false);
  const [activeCards, setActiveCards] = useState<number[]>([]);
  const [cardToGuess, setCardToGuess] = useState<number>(-1);

  useEffect(() => {
    if (board && board.boardCards)
      setActiveCards(board.boardCards.map((bc) => bc.id));
    console.log('hsd');
  }, []);

  useEffect(() => {
    var p1TurnStates = [
      GameState.P1_TURN_STARTED,
      GameState.P1_WAITING_ASK_REPLY,
    ];
    var p2TurnStates = [
      GameState.P2_TURN_STARTED,
      GameState.P2_WAITING_ASK_REPLY,
    ];
    var finishedTurnStates = [
      GameState.P1_ASK_REPLIED,
      GameState.P2_ASK_REPLIED,
    ];
    var finished = [GameState.P1_WON, GameState.P2_WON];

    setPlayerTurnFinished(finishedTurnStates.includes(gameState));
    if (p1TurnStates.includes(gameState)) {
      setHeader(isHost ? 'Your turn' : 'Their turn');
      setThisPlayerTurn(isHost ? true : false);
    }
    if (p2TurnStates.includes(gameState)) {
      setHeader(isHost ? 'Their turn' : 'Your turn');
      setThisPlayerTurn(isHost ? false : true);
    }
    if (finished.includes(gameState)) {
      setHeader('Finished');
      setActionModalVisible(true);
      setGameFinished(true);
    }
  }, [gameState]);

  const handleBackPressed = async (doBroadcast: boolean) => {
    setPage(PlayPages.MAIN_PAGE);
    setDisplayTabBar('flex');
    if (connection) await leaveGame(connection, gameId, doBroadcast);
  };

  const handleCardPressed = (boardcardId: number) => {
    if (!guessMode) {
      if (activeCards.includes(boardcardId)) {
        setActiveCards((prev) => prev.filter((id) => boardcardId != id));
        return;
      }
      setActiveCards((prev) => [...prev, boardcardId]);
      return;
    }

    setCardToGuess(boardcardId);
  };

  const handleTakeGuessPressed = async () => {
    if (connection) {
      setGuessMode(false);
      const result = await guessBoardCard(connection, gameId, cardToGuess);
      if (result.isError) {
        toggleInfoModal(true, result.message);
      }

      const playersLeftResult = await updateBoardCardsActivity(
        connection,
        gameId,
        board!.id,
        activeCards
      );

      if (playersLeftResult.isError) {
        toggleInfoModal(true, playersLeftResult.message);
      }
    }
  };

  const handleFinishTurn = async () => {
    if (connection) {
      await finishTurn(connection, gameId);

      await updateBoardCardsActivity(
        connection,
        gameId,
        board!.id,
        activeCards
      );
    }
  };

  return (
    <>
      <ActionModal
        modalVisible={actionModalVisible}
        setModalVisible={setActionModalVisible}
        gameFinished={gameFinsihed}
      />

      <View style={styles.container}>
        <Text style={styles.header}>{header}</Text>
        <View style={styles.subHeaderWrapper}>
          <Text style={{ ...styles.header2, color: Colors.Green }}>
            {activeCards.length}
          </Text>
          <Text style={{ ...styles.header2, color: Colors.Cream }}>vs</Text>
          <Text style={{ ...styles.header2, color: Colors.BurgundyRed }}>
            {oponentCardsLeft}
          </Text>
        </View>
        <Pressable
          style={styles.backIconWrapper}
          onPress={() => handleBackPressed(true)}
        >
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
                transition={300}
                placeholder={{ blurhash }}
                style={imageStyles.chosenCardInner}
                source={{
                  uri: board?.chosenCard?.card.url,
                }}
              />
            </View>
            <View style={styles.controlButtonWrapper}>
              {thisPlayerTurn && !guessMode && !playerTurnFinished && (
                <>
                  <MediumButton
                    text="Ask"
                    inverted={false}
                    color={Colors.BurgundyRed}
                    onButtonPress={() => setAskModalVisible(true)}
                  />
                  <MediumButton
                    text="Guess"
                    inverted={false}
                    color={Colors.BurgundyRed}
                    onButtonPress={() => setGuessMode(true)}
                  />
                </>
              )}
              {thisPlayerTurn && guessMode && !playerTurnFinished && (
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
              {thisPlayerTurn && playerTurnFinished && (
                <>
                  <MediumButton
                    text="Finish turn"
                    inverted={false}
                    color={Colors.Green}
                    onButtonPress={handleFinishTurn}
                  />
                </>
              )}
            </View>
          </View>
        </View>
      </View>
    </>
  );
}
