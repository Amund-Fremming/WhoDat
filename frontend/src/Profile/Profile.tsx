import { View, Text, Pressable, TextInput, Image } from "react-native";
import { styles, imageStyles } from "./ProfileStyles";
import { useEffect, useState } from "react";
import { Feather } from "@expo/vector-icons";
import { Colors } from "../Shared/assets/constants/Colors";
import ErrorModal from "../Shared/components/ErrorModal/ErrorModal";
import { useAuthProvider } from "../Shared/state/AuthProvider";
import SmallButton from "../Shared/components/SmallButton/SmallButton";
import MediumButton from "../Shared/components/MediumButton/MediumButton";
import BigButton from "../Shared/components/BigButton/BigButton";

export default function Profile() {
  const [editMode, setEditMode] = useState<boolean>(false);
  const [errorModalVisible, setErrorModalVisible] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");
  const [imageUri, setImageUri] = useState<any>(
    "https://t4.ftcdn.net/jpg/00/64/67/63/360_F_64676383_LdbmhiNM6Ypzb3FM4PPuFP9rHe7ri8Ju.jpg"
  );
  const { imageUrl, username } = useAuthProvider();

  useEffect(() => {
    if (imageUrl != null) setImageUri(imageUrl);
    console.log(imageUri);
  }, []);

  const toggleEditMode = () => setEditMode(!editMode);

  const handleError = (message: string) => {
    setErrorModalVisible(true);
    setErrorMessage(message);
  };

  const handleImageUpload = () => {
    console.log("Image uploaded");
    // TODO
  };

  const handleUpdatePlayer = () => {
    console.log("Player updated");
    // TODO
  };

  const handleLogout = () => {
    console.log("Logged out");
    // TODO
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
              <Pressable style={styles.uploadButton}>
                <Text>Upload icon</Text>
              </Pressable>
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
                onButtonPress={handleUpdatePlayer}
              />
            </View>
          </View>
        )}
      </View>
    </View>
  );
}
