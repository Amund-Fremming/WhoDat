import { Colors } from '@/src/Shared/assets/constants/Colors';
import BigButton from '@/src/Shared/components/BigButton/BigButton';
import { Modal, Text, View } from 'react-native';
import { styles } from './ActionModalStyles';
import { useEffect, useState } from 'react';
import StrokedText from '@/src/Shared/components/StokedText/StrokedText';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { leaveGame } from '@/src/Game/GameHubClient';
import { PlayPages } from '@/src/Game/types/GamePages';
import { GameState } from '@/src/Game/types/GameTypes';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';

interface ActionModalProps {
  modalVisible: boolean;
  setModalVisible: React.Dispatch<React.SetStateAction<boolean>>;
  gameFinished: boolean;
}

export default function ActionModal({
  modalVisible,
  setModalVisible,
  gameFinished,
}: ActionModalProps) {
  const { connection, gameId, setPage, isHost, gameState } = useGameProvider();
  const { setDisplayTabBar } = useTabBarProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const [buttonText, setButtonText] = useState<string>('');
  const [headerText, setHeaderText] = useState<string>('');
  const [headerColor, setHeaderColor] = useState<string>('');

  useEffect(() => {
    if (gameFinished) {
      setButtonText('Leave game');
      const isVictory =
        (isHost && gameState === GameState.P1_WON) ||
        (!isHost && gameState === GameState.P2_WON);
      setHeaderColor(isVictory ? Colors.Green : Colors.Red);
      setHeaderText(`You ${isVictory ? 'won!' : 'lost ...'}`);
      return;
    }

    setHeaderColor(Colors.Cream);
    setButtonText('Close');
  }, [gameFinished]);

  const handleClosePressed = async () => {
    setModalVisible(false);
    if (gameFinished) {
      setDisplayTabBar('flex');
      setPage(PlayPages.MAIN_PAGE);
      if (connection) await leaveGame(connection, gameId);
    }
  };

  return (
    <Modal visible={modalVisible} animationType="fade" transparent={true}>
      <View style={styles.container}>
        <View style={styles.modal}>
          <StrokedText
            font="Modak"
            color={headerColor}
            text={headerText}
            fontBaseSize={70}
            smallBorder={false}
          />
          <View style={styles.absoluteButton}>
            <BigButton
              text={buttonText}
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={handleClosePressed}
            />
          </View>
        </View>
      </View>
    </Modal>
  );
}
