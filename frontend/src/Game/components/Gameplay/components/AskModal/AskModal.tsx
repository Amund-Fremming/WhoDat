import { Modal, Pressable, View, TextInput } from 'react-native';
import { styles } from './AskModalStyles';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { useState } from 'react';
import { validText } from '@/src/Shared/functions/InputValitator';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { sendMessage } from '@/src/Game/GameHubClient';
import { FontAwesome } from '@expo/vector-icons';
import BigButton from '@/src/Shared/components/BigButton/BigButton';

interface AskModalProps {
  modalVisible: boolean;
  setModalVisible: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function AskModal({
  modalVisible,
  setModalVisible,
}: AskModalProps) {
  const [question, setQuestion] = useState<string>('');
  const { toggleInfoModal } = useInfoModalProvider();
  const { connection, gameId } = useGameProvider();

  const handleAskPressed = async () => {
    if (!validText(question)) {
      setModalVisible(false);
      toggleInfoModal(false, 'Only letters allowed!');
      setQuestion('');
      return;
    }

    if (connection) {
      const result = await sendMessage(connection, gameId, question);
      if (result.isError) toggleInfoModal(false, result.message);
    }
  };

  return (
    <Modal animationType="fade" visible={modalVisible} transparent={true}>
      <View style={styles.container}>
        <View style={styles.modal}>
          <Pressable
            style={styles.closeButton}
            onPress={() => setModalVisible(false)}
          >
            <FontAwesome name="close" size={36} color={Colors.DarkGray} />
          </Pressable>
          <TextInput
            multiline={true}
            placeholder="Ask a yes or no question"
            placeholderTextColor={'000000'}
            style={styles.input}
            value={question}
            onChangeText={setQuestion}
          />
          <View style={styles.border} />
          <View style={styles.absoluteButton}>
            <BigButton
              text="Ask"
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={handleAskPressed}
            />
          </View>
        </View>
      </View>
    </Modal>
  );
}
