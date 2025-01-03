import { View, Text, Pressable } from 'react-native';
import { styles } from './LobbyPageStyles';
import { PlayPages } from '../../types/GamePages';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import { leaveGame } from '../../GameHubClient';

export default function LobbyPage() {
  const { setPage } = useGameProvider();
  const { connection, gameId } = useGameProvider();
  const { setDisplayTabBar } = useTabBarProvider();

  const handleBackPressed = async () => {
    setPage(PlayPages.MAIN_PAGE);
    setDisplayTabBar('flex');
    if (connection) await leaveGame(connection, gameId);
  };

  return (
    <View style={styles.container}>
      <Pressable style={styles.backIconWrapper} onPress={handleBackPressed}>
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.headerWrapper}>
        <Text style={styles.header}>The host</Text>
        <Text style={styles.header2}>is choosing warriors</Text>
      </View>
    </View>
  );
}
