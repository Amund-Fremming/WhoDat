import { Pressable, Text, View } from 'react-native';
import { styles } from './GameplayStyles';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { PlayPages } from '../../types/GamePages';
import { useTabBarProvider } from '@/src/Shared/providers/TabBarProvider';
import { leaveGame } from '../../GameHubClient';

export default function Gameplay() {
  const { setPage, connection, gameId } = useGameProvider();
  const { setDisplayTabBar } = useTabBarProvider();

  const handleBackPressed = async () => {
    // TODO: are you sure modal
    setPage(PlayPages.MAIN_PAGE);
    setDisplayTabBar('flex');
    if (connection) await leaveGame(connection, gameId);
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>PLACEHOLDER</Text>
      <Text style={styles.header2}>sub_placeholder</Text>
      <Pressable style={styles.backIconWrapper} onPress={handleBackPressed}>
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.creamContainer}>
        <View style={styles.boardContainer}>
          <Text>Gameplay</Text>
        </View>
      </View>
    </View>
  );
}
