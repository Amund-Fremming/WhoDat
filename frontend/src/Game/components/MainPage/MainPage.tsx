import { View, Text } from 'react-native';
import { styles } from './MainPageStyles';
import { PlayPages } from '../../types/GamePages';
import IconButton from '@/src/Shared/components/IconButton/IconButton';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';

export default function MainPage() {
  const { setPage } = useGameProvider();

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Time to cook!</Text>
      <IconButton
        text="Start a game"
        icon="host"
        onButtonPress={() => setPage(PlayPages.HOST_PAGE)}
      />
      <IconButton
        text="Join a friend"
        icon="join"
        onButtonPress={() => setPage(PlayPages.JOIN_PAGE)}
      />
    </View>
  );
}
