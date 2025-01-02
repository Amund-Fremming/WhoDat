import { View, Text, Pressable } from 'react-native';
import { styles } from './HostPageStyles';
import { PlayPages } from '../../types/GamePages';
import IconButton from '@/src/Shared/components/IconButton/IconButton';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { GameState } from '../../types/GameTypes';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { createGame } from '../../GameClient';
import { useAuthProvider } from '@/src/Shared/providers/AuthProvider';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { subscribeToGameAsHost } from '../../GameHubClient';

export default function HostPage() {
  const { setPage, setGameState, connection, setGameId, setIsHost } =
    useGameProvider();
  const { token } = useAuthProvider();
  const { toggleInfoModal } = useInfoModalProvider();

  const handleCreateGame = async (gameState: GameState) => {
    var result = await createGame(gameState, token);
    if (result.isError) {
      toggleInfoModal(true, result.message);
      setPage(PlayPages.MAIN_PAGE);
      return;
    }

    if (result.data && connection) {
      setGameId(result.data);
      setIsHost(true);
      await subscribeToGameAsHost(connection, result.data);
    } else {
      toggleInfoModal(
        true,
        'Failed to set incomming game id. Connection failed.'
      );
    }
  };

  const handleBothChoosing = async () => {
    setPage(PlayPages.WAITING_PAGE);
    setGameState(GameState.BOTH_CHOSING_CARDS);
    await handleCreateGame(GameState.BOTH_CHOSING_CARDS);
  };

  const handleHostChoosing = async () => {
    setPage(PlayPages.WAITING_PAGE);
    setGameState(GameState.ONLY_HOST_CHOSING_CARDS);
    await handleCreateGame(GameState.ONLY_HOST_CHOSING_CARDS);
  };

  return (
    <View style={styles.container}>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.textWrapper}>
        <Text style={styles.header}>Choose</Text>
        <Text style={styles.header}>the cards</Text>
      </View>
      <IconButton
        text="Your cards"
        icon="user"
        onButtonPress={handleHostChoosing}
      />
      <IconButton
        text="Split 50/50"
        icon="users"
        onButtonPress={handleBothChoosing}
      />
    </View>
  );
}
