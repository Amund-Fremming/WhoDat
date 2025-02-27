import { View, Text, TextInput } from 'react-native';
import { Image } from 'expo-image';
import { styles, imageStyles } from './ProfileStyles';
import { useEffect, useState } from 'react';
import { Feather } from '@expo/vector-icons';
import { Colors } from '../Shared/assets/constants/Colors';
import { useAuthProvider } from '../Shared/providers/AuthProvider';
import MediumButton from '../Shared/components/MediumButton/MediumButton';
import BigButton from '../Shared/components/BigButton/BigButton';
import { pickImage } from '../Shared/functions/ImagePicker';
import { TouchableOpacity } from 'react-native';
import { updatePlayer, updatePlayerImage } from './PlayerClient';
import { IPlayerDto } from '../Shared/types/PlayerTypes';
import { useInfoModalProvider } from '../Shared/providers/InfoModalProvider';

const blurhash =
  '|rF?hV%2WCj[ayj[a|j[az_NaeWBj@ayfRayfQfQM{M|azj[azf6fQfQfQIpWXofj[ayj[j[fQayWCoeoeaya}j[ayfQa{oLj?j[WVj[ayayj[fQoff7azayj[ayj[j[ayofayayayj[fQj[ayayj[ayfjj[j[ayjuayj[';

export default function Profile() {
  const [editMode, setEditMode] = useState<boolean>(false);
  const [newPassword, setNewPassword] = useState<string>('');
  const [newUsername, setNewUsername] = useState<string>('');
  const [imageUri, setImageUri] = useState<any>();
  const { toggleInfoModal } = useInfoModalProvider();
  const { imageUrl, username, setUsername, playerID, token, setToken } =
    useAuthProvider();

  useEffect(() => {
    if (imageUrl != null) setImageUri(imageUrl);
  }, []);

  const toggleEditMode = () => {
    setEditMode(!editMode);
    clearValues();
  };

  const handleSelectImage = async () => {
    const uri = await pickImage();
    if (uri !== 'EXIT') {
      setImageUri(uri);
      const result = await updatePlayerImage(uri, token);
      if (result.isError) {
        toggleInfoModal(true, result.message);
        setImageUri(imageUrl);
      }
    }
  };

  const handleUpdatePlayer = async () => {
    const dto: IPlayerDto = {
      playerID: playerID,
      username: newUsername,
      password: newPassword,
      imageUrl: imageUri,
    };
    const result = await updatePlayer(dto, token);
    if (result.isError) {
      toggleInfoModal(true, result.message);
      return;
    }

    setUsername(result.data?.username!);
    clearValues();
    setEditMode(false);
  };

  const clearValues = () => {
    setNewPassword('');
    setNewUsername('');
  };

  const handleLogout = () => {
    setToken('yeahhjh');
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Profile</Text>
      <View style={styles.creamContainer}>
        {!editMode && (
          <View style={styles.nonEditContainer}>
            <View style={styles.imageContainer}>
              <Image
                transition={300}
                placeholder={{ blurhash }}
                source={{
                  uri: imageUri,
                }}
                style={imageStyles.imageStyle}
              />
            </View>
            <Text style={styles.username}>{username}</Text>
            <BigButton
              text="Update user"
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={toggleEditMode}
            />
          </View>
        )}
        {editMode && (
          <View style={styles.editContainer}>
            <View style={styles.imageContainer}>
              <Image
                transition={300}
                source={{
                  uri: imageUri,
                }}
                style={imageStyles.imageStyle}
              />
              <TouchableOpacity
                activeOpacity={0.5}
                onPress={handleSelectImage}
                style={styles.uploadButton}
              >
                <Feather name="upload" size={28} color={Colors.Cream} />
              </TouchableOpacity>
            </View>
            <View style={styles.inputContainer}>
              <View style={styles.iconAndInput}>
                <Feather
                  style={styles.icon}
                  name="user"
                  size={35}
                  color={Colors.DarkGray}
                />
                <TextInput
                  style={styles.textInput}
                  placeholder="New username"
                  onChangeText={(input) => setNewUsername(input)}
                  placeholderTextColor={'gray'}
                />
              </View>
              <View style={styles.border}></View>
            </View>
            <View style={styles.inputContainer}>
              <View style={styles.iconAndInput}>
                <Feather
                  style={styles.icon}
                  name="lock"
                  size={35}
                  color={Colors.DarkGray}
                />
                <TextInput
                  value={newPassword}
                  onChangeText={(input) => setNewPassword(input)}
                  secureTextEntry={true}
                  style={styles.textInput}
                  placeholder="New password"
                  placeholderTextColor={'gray'}
                />
              </View>
              <View style={styles.border}></View>
            </View>
            <View style={styles.buttonWrapper}>
              <MediumButton
                text="Cancel"
                color={Colors.BurgundyRed}
                inverted={true}
                onButtonPress={toggleEditMode}
              />
              <MediumButton
                text="Save"
                color={Colors.BurgundyRed}
                inverted={false}
                onButtonPress={async () => handleUpdatePlayer()}
              />
            </View>
          </View>
        )}
      </View>
    </View>
  );
}
