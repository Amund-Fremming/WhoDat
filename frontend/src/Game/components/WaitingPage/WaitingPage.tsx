import { View, Text, Pressable } from 'react-native';
import { styles } from './WaitingPageStyles';
import { PlayPages } from '../../types/GamePages';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { Ionicons } from '@expo/vector-icons';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { leaveGame } from '../../GameHubClient';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';

export default function WaitingPage() {
  const { gameId, setPage, connection, waitingMessage } = useGameProvider();
  const { setDisplayTabBar } = useTabBarProvider();
  const { toggleInfoModal } = useInfoModalProvider();

  const handleBackPressed = () => {
    setDisplayTabBar('flex');
    setPage(PlayPages.MAIN_PAGE);
    if (connection) leaveGame(connection, gameId, true);
    if (!connection) toggleInfoModal(true, 'Connection was lost');
  };

  return (
    <View style={styles.container}>
      <Text style={styles.id}>ID: {gameId}</Text>
      <Pressable style={styles.backIconWrapper} onPress={handleBackPressed}>
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.headerWrapper}>
        <Text style={styles.header}>{waitingMessage}</Text>
      </View>
    </View>
  );
}
