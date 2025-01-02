import { View, Text, Pressable } from 'react-native';
import { styles } from './LobbyPageStyles';
import { PlayPages } from '../../types/GamePages';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';

export default function LobbyPage() {
  const { setPage } = useGameProvider();

  return (
    <View style={styles.container}>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.headerWrapper}>
        <Text style={styles.header}>Username</Text>
        <Text style={styles.header2}>is choosing warriors</Text>
      </View>
    </View>
  );
}
