import { Modal, Pressable, View, TextInput } from 'react-native';
import { styles } from './AskModalStyles';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import React, { useEffect, useState } from 'react';
import { validText } from '@/src/Shared/functions/InputValitator';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { useGameProvider } from '@/src/Shared/providers/GameProvider';
import { sendMessage } from '@/src/Game/GameHubClient';
import { FontAwesome } from '@expo/vector-icons';
import BigButton from '@/src/Shared/components/BigButton/BigButton';
import { AskState } from '../../types/AskState';
import MediumButton from '../MediumButton/MediumButton';
import StrokedText from '../StokedText/StrokedText';
import { useGameplayProvider } from '../../providers/GameplayProvider';

interface AskModalProps {
  modalVisible: boolean;
  setModalVisible: React.Dispatch<React.SetStateAction<boolean>>;
}

export default function AskModal({
  modalVisible,
  setModalVisible,
}: AskModalProps) {
  const [question, setQuestion] = useState<string>('');
  const [placeholderText, setPlaceholderText] = useState<string>(
    'Ask a yes or no question'
  );
  const [placeholderColor, setPlaceholderColor] = useState<string>(
    Colors.Placeholder
  );
  const { connection, gameId, isHost, gameState } = useGameProvider();
  const { questionReceived, askState, setAskModalVisible } =
    useGameplayProvider();
  const { toggleInfoModal } = useInfoModalProvider();

  useEffect(() => {
    setQuestion('');
  }, [modalVisible]);

  const handleAskPressed = async () => {
    if (question.length > 25) {
      setPlaceholderColor(Colors.Red);
      setPlaceholderText('To many characters!');
      setQuestion('');
      return;
    }

    if (connection) {
      const result = await sendMessage(connection, gameId, question);
      if (result.isError) toggleInfoModal(false, result.message);
    }
  };

  const handleAskInput = (input: string) => {
    setQuestion(input);
    setPlaceholderColor(Colors.Placeholder);
    setPlaceholderText('Ask a yes or no question');
  };

  const handleQuestionAnswerPressed = async (answer: string) => {
    if (connection) {
      var result = await sendMessage(connection, gameId, answer);
    }
  };

  return (
    <Modal animationType="fade" visible={modalVisible} transparent={true}>
      <View style={styles.container}>
        <View style={styles.modal}>
          {askState === AskState.Asking && (
            <>
              <Pressable
                style={styles.closeButton}
                onPress={() => setModalVisible(false)}
              >
                <FontAwesome name="close" size={36} color={Colors.DarkGray} />
              </Pressable>
              <TextInput
                multiline={true}
                placeholder={placeholderText}
                placeholderTextColor={placeholderColor}
                style={styles.input}
                value={question}
                onChangeText={handleAskInput}
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
            </>
          )}
          {askState === AskState.Answering && (
            <>
              <StrokedText
                text={questionReceived}
                color={Colors.Cream}
                font="Modak"
                fontBaseSize={40}
                smallBorder={false}
              />
              <View style={styles.absoluteDoubleButton}>
                <MediumButton
                  color={Colors.Red}
                  inverted={false}
                  onButtonPress={() => handleQuestionAnswerPressed('No')}
                  text="No"
                />
                <MediumButton
                  color={Colors.Green}
                  inverted={false}
                  onButtonPress={() => handleQuestionAnswerPressed('Yes')}
                  text="Yes"
                />
              </View>
            </>
          )}
          {askState === AskState.Waiting && (
            <View style={styles.waitingContainer}>
              <StrokedText
                text="Waiting"
                color={Colors.Cream}
                font="Modak"
                fontBaseSize={60}
                smallBorder={false}
              />
            </View>
          )}
          {askState === AskState.Finished && (
            <>
              <StrokedText
                text={'Oponent is flipping cards'}
                color={Colors.Cream}
                font="Modak"
                fontBaseSize={40}
                smallBorder={false}
              />
              <View style={styles.absoluteButton}>
                <BigButton
                  text="Close"
                  color={Colors.BurgundyRed}
                  inverted={false}
                  onButtonPress={() => setAskModalVisible(false)}
                />
              </View>
            </>
          )}
          {askState === AskState.Answered && (
            <>
              <StrokedText
                text={questionReceived}
                color={Colors.Cream}
                font="Modak"
                fontBaseSize={40}
                smallBorder={false}
              />
              <View style={styles.absoluteButton}>
                <BigButton
                  text="Close"
                  color={Colors.BurgundyRed}
                  inverted={false}
                  onButtonPress={() => setAskModalVisible(false)}
                />
              </View>
            </>
          )}
        </View>
      </View>
    </Modal>
  );
}
