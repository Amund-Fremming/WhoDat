import React from 'react';
import {
  Modal,
  View,
  Pressable,
  TextInput,
  Text,
  KeyboardAvoidingView,
  Platform,
} from 'react-native';
import { Image } from 'expo-image';
import { styles, imageStyles } from './AddCardModalStyles';
import BigButton from '@/src/Shared/components/BigButton/BigButton';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import FontAwesome from '@expo/vector-icons/FontAwesome';
import { useState } from 'react';
import { useAuthProvider } from '@/src/Shared/providers/AuthProvider';
import { addCard } from '@/src/Shared/functions/CardClient';
import { validText } from '@/src/Shared/functions/InputValitator';
import { pickImage } from '@/src/Shared/functions/ImagePicker';
import Result from '@/src/Shared/objects/Result';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { ICardDto } from '@/src/Shared/types/CardTypes';
import MediumButton from '@/src/Shared/components/MediumButton/MediumButton';

interface AddCardModalProps {
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
  setJustAddedCard: React.Dispatch<React.SetStateAction<ICardDto | undefined>>;
}

const blurhash =
  '|rF?hV%2WCj[ayj[a|j[az_NaeWBj@ayfRayfQfQM{M|azj[azf6fQfQfQIpWXofj[ayj[j[fQayWCoeoeaya}j[ayfQa{oLj?j[WVj[ayayj[fQoff7azayj[ayj[j[ayofayayayj[fQj[ayayj[ayfjj[j[ayjuayj[';

export default function AddCardModal({
  modalVisible,
  setModalVisible,
  setJustAddedCard,
}: AddCardModalProps) {
  const [nameInput, setNameInput] = useState<string>('');
  const [imageUri, setImageUri] = useState<any>(
    'https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg'
  );
  const [isErrorView, setIsErrorView] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const { token } = useAuthProvider();
  const { toggleInfoModal } = useInfoModalProvider();

  const handleNameInput = (name: string): boolean => {
    if (name.length <= 0) {
      setErrorMessage('Name cannot be empty.');
      setIsErrorView(true);
      return false;
    }

    if (name.length > 9 || !validText(name)) {
      setErrorMessage('Name must be text only and under 9 letters long');
      setIsErrorView(true);
      setNameInput('');
      return false;
    }

    setNameInput(
      nameInput.charAt(0).toUpperCase() + nameInput.slice(1).toLowerCase()
    );
    return true;
  };

  const handleImageInput = async () => {
    try {
      const result = await pickImage();
      if (result === 'EXIT') return;
      setImageUri(result);
    } catch (Exception) {
      toggleInfoModal(true, 'Image picker failed.');
    }
  };

  const uploadCard = async () => {
    const namePresent = handleNameInput(nameInput);
    if (!namePresent) return;
    setModalVisible(false);

    var result: Result<ICardDto> = await addCard(imageUri, nameInput, token);
    if (result.isError) {
      toggleInfoModal(false, result.message);
      setNameInput('');
      return;
    }

    setJustAddedCard(result.data!);
    setModalVisible(false);
    setNameInput('');
    setImageUri(
      'https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg'
    );
  };

  return (
    <View>
      <Modal visible={modalVisible} animationType="fade" transparent={true}>
        <KeyboardAvoidingView
          behavior={Platform.OS == 'ios' ? 'padding' : 'height'}
          style={styles.container}
        >
          <View style={styles.cardModal}>
            {!isErrorView && (
              <>
                <Pressable
                  style={styles.closeButton}
                  onPress={() => setModalVisible(false)}
                >
                  <FontAwesome name="close" size={36} color={Colors.DarkGray} />
                </Pressable>
                <View style={styles.card}>
                  <Pressable
                    style={styles.uploadButton}
                    onPress={handleImageInput}
                  >
                    <Text style={styles.uploadText}>upload</Text>
                  </Pressable>
                  <Image
                    transition={300}
                    placeholder={{ blurhash }}
                    style={imageStyles.imageStyle}
                    source={{
                      uri: imageUri,
                    }}
                  />
                </View>
                <TextInput
                  value={nameInput}
                  onChangeText={(input: string) => setNameInput(input)}
                  style={styles.inputText}
                  placeholder="Name ..."
                  placeholderTextColor={Colors.Gray}
                />
                <View style={styles.border} />
                <View style={styles.buttonWrapper}>
                  <BigButton
                    text="Add Card"
                    color={Colors.BurgundyRed}
                    inverted={false}
                    onButtonPress={uploadCard}
                  />
                </View>
              </>
            )}
            {isErrorView && (
              <View style={styles.errorContainer}>
                <FontAwesome name="bell" size={100} color={Colors.Red} />
                <Text style={styles.header}>Ooops</Text>
                <Text style={styles.message}>{errorMessage}</Text>
                <View style={styles.absoluteButton}>
                  <BigButton
                    text="Back"
                    color={Colors.BurgundyRed}
                    inverted={true}
                    onButtonPress={() => setIsErrorView(false)}
                  />
                </View>
              </View>
            )}
          </View>
        </KeyboardAvoidingView>
      </Modal>
    </View>
  );
}
