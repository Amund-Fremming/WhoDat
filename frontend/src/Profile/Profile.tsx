import { View, Text, TextInput, Image } from "react-native";
import { styles, imageStyles } from "./ProfileStyles";
import { useEffect, useState } from "react";
import { Feather } from "@expo/vector-icons";
import { Colors } from "../Shared/assets/constants/Colors";
import ErrorModal from "../Shared/components/ErrorModal/ErrorModal";
import { useAuthProvider } from "../Shared/state/AuthProvider";
import MediumButton from "../Shared/components/MediumButton/MediumButton";
import BigButton from "../Shared/components/BigButton/BigButton";
import { pickImage } from "../Shared/functions/ImagePicker";
import { TouchableOpacity } from "react-native";
import { updatePlayer } from "./PlayerClient";
import { IPlayerDto } from "../Shared/domain/PlayerTypes";
import { DevSettings } from "react-native";

export default function Profile() {
  const [editMode, setEditMode] = useState<boolean>(false);
  const [errorModalVisible, setErrorModalVisible] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [newPassword, setNewPassword] = useState<string>("");
  const [newUsername, setNewUsername] = useState<string>("");
  const [imageUri, setImageUri] = useState<any>();
  const { imageUrl, setImageUrl, username, setUsername, playerID, setToken } =
    useAuthProvider();

  useEffect(() => {
    if (imageUrl != null) setImageUri(imageUrl);
  }, []);

  const toggleEditMode = () => {
    setEditMode(!editMode);
    clearValues();
  };

  const handleError = (message: string) => {
    setErrorModalVisible(true);
    setErrorMessage(message);
  };

  const handleSelectImage = async () => {
    const uri = await pickImage();
    if (uri !== "EXIT") setImageUri(uri);
  };

  const handleUpdatePlayer = async () => {
    const dto: IPlayerDto = {
      playerID: playerID,
      username: newUsername,
      password: newPassword,
      imageUrl: imageUri,
    };
    const result = await updatePlayer(dto);
    if (result.isError) {
      handleError(result.message);
      return;
    }

    setImageUrl(result.data?.imageUrl!);
    setUsername(result.data?.username!);
    clearValues();
    setEditMode(false);
  };

  const clearValues = () => {
    setNewPassword("");
    setNewUsername("");
  };

  const handleLogout = () => {
    setToken("");
    DevSettings.reload();
  };

  return (
    <View style={styles.container}>
      <ErrorModal
        message={errorMessage}
        setErrorModalVisible={setErrorModalVisible}
        errorModalVisible={errorModalVisible}
      />
      <Text style={styles.header}>Profile</Text>
      <View style={styles.creamContainer}>
        {!editMode && (
          <View style={styles.nonEditContainer}>
            <View style={styles.imageContainer}>
              <Image
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
            <BigButton
              text="Logout"
              color={Colors.BurgundyRed}
              inverted={true}
              onButtonPress={handleLogout}
            />
          </View>
        )}
        {editMode && (
          <View style={styles.editContainer}>
            <View style={styles.imageContainer}>
              <Image
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
                  placeholderTextColor={"gray"}
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
                  placeholderTextColor={"gray"}
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
