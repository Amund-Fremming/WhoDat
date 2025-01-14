import { Pressable, Text, View } from 'react-native';
import { Image } from 'expo-image';
import { imageStyles, styles } from './GameplayStyles';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { PlayPages } from '../../types/GamePages';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import {
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
import AskModal from './components/AskModal/AskModal';

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

  const [header, setHeader] = useState<string>('');
  const [thisPlayerTurn, setThisPlayerTurn] = useState<boolean>(isHost);
  const [guessMode, setGuessMode] = useState<boolean>(false);
  const [actionModalVisible, setActionModalVisible] = useState<boolean>(false);
  const [askModalVisible, setAskModalVisible] = useState<boolean>(false);
  const [gameFinsihed, setGameFinished] = useState<boolean>(false);
  const [cardsNotActive, setCardsNotActive] = useState<number[]>([]);
  const [cardToGuess, setCardToGuess] = useState<number>(-1);

  useEffect(() => {
    var stateP1Turn = [2, 5, 8, 9, 10, 11, 12];
    var stateP2Turn = [3, 6, 13, 14, 15, 16, 17];
    var finished = [18, 19];

    if (stateP1Turn.includes(gameState)) {
      setHeader(isHost ? 'Your turn' : 'Their turn');
      setThisPlayerTurn(isHost ? true : false);
    }
    if (stateP2Turn.includes(gameState)) {
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
      if (cardsNotActive.includes(boardcardId)) {
        setCardsNotActive((prev) => prev.filter((id) => boardcardId != id));
        return;
      }
      setCardsNotActive((prev) => [...prev, boardcardId]);
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

      const boardCardUpdates: Array<IBoardCardUpdate> = board!.boardCards!.map(
        (bc) => {
          var update: IBoardCardUpdate = {
            id: bc.id,
            active: !cardsNotActive.includes(bc.id),
          };
          return update;
        }
      );

      const playersLeftResult = await updateBoardCardsActivity(
        connection,
        gameId,
        board!.id,
        boardCardUpdates
      );

      if (playersLeftResult.isError) {
        toggleInfoModal(true, playersLeftResult.message);
      }
    }
  };

  return (
    <>
      <ActionModal
        modalVisible={actionModalVisible}
        setModalVisible={setActionModalVisible}
        gameFinished={gameFinsihed}
      />

      <AskModal
        modalVisible={askModalVisible}
        setModalVisible={setAskModalVisible}
      />

      <View style={styles.container}>
        <Text style={styles.header}>{header}</Text>
        <View style={styles.subHeaderWrapper}>
          <Text style={{ ...styles.header2, color: Colors.Green }}>
            {20 - cardsNotActive.length}
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
              {thisPlayerTurn && !guessMode && (
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
      <View style={styles.container}>
        <Text style={styles.header}>{header}</Text>
        <View style={styles.subHeaderWrapper}>
          <Text style={{ ...styles.header2, color: Colors.Green }}>20</Text>
          <Text style={{ ...styles.header2, color: Colors.Cream }}>vs</Text>
          <Text style={{ ...styles.header2, color: Colors.BurgundyRed }}>
            20
          </Text>
        </View>
        <Pressable
          style={styles.backIconWrapper}
          onPress={() => handleBackPressed(false)}
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
              {thisPlayerTurn && !guessMode && (
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
    </>
  );
}
