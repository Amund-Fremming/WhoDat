import { View, Pressable, Text, TextInput } from 'react-native';
import { styles } from './JoinPageStyles';
import { PlayPages } from '../../types/GamePages';
import { Ionicons } from '@expo/vector-icons';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import BigButton from '@/src/Shared/components/BigButton/BigButton';
import { joinGame } from '../../GameHubClient';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';

export default function JoinPage() {
  const { toggleInfoModal } = useInfoModalProvider();
  const { setGameId, setPage, connection, gameId, setIsHost } =
    useGameProvider();

  const handleJoinGame = async () => {
    if (connection) {
      setIsHost(false);
      await joinGame(connection, gameId);
    } else {
      toggleInfoModal(true, 'Connection was broken.');
    }
  };

  const handleInput = (input: string) => {
    try {
      var val = Number.parseInt(input);
      if (Number.isNaN(val)) {
        toggleInfoModal(true, 'Game id must be numeric.');
      }

      if (val > 10000) {
        toggleInfoModal(true, 'Game ids has to be lower than 10 000.');
        return;
      }
      setGameId(val);
    } catch (error) {
      toggleInfoModal(true, 'Input provided is faulty brah');
    }
  };

  return (
    <View style={styles.container}>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <Text style={styles.header}>Type a friends id</Text>
      <View style={styles.box}>
        <View style={styles.inputWrapper}>
          <TextInput
            onChangeText={(input) => handleInput(input)}
            keyboardType="numeric"
            style={styles.textInput}
            placeholder="37293 ..."
          />
          <View style={styles.underline} />
        </View>
        <BigButton
          text="Join"
          color={Colors.BurgundyRed}
          inverted={false}
          onButtonPress={handleJoinGame}
        />
      </View>
    </View>
  );
}
