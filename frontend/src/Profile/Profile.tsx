import { View, Text, Pressable, TextInput, Image } from "react-native";
import { styles } from "./ProfileStyles";
import { useState } from "react";
import { Feather } from "@expo/vector-icons";
import { Colors } from "../Shared/assets/constants/Colors";
import ErrorModal from "../Shared/components/ErrorModal/ErrorModal";

export default function Profile() {
  const [editMode, setEditMode] = useState<boolean>(false);
  const [errorModalVisible, setErrorModalVisible] = useState<boolean>(false);
  const [errorMessage, setErrorMessage] = useState<string>("");

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

      <Text>Profile</Text>
      {!editMode && (
        <View style={styles.nonEditContainer}>
          <View style={styles.imageContainer}>
            <Image
              source={{
                uri: "https://fbi.cults3d.com/uploaders/32338244/illustration-file/f23d00d1-2861-40af-8e83-1e8f5eda7403/WhatsApp-Image-2024-08-27-at-6.36.25-PM.jpeg",
              }}
            />
          </View>
          <Text>Username</Text>
          <Pressable onPress={toggleEditMode}>
            <Text>Update user</Text>
          </Pressable>
        </View>
      )}
      {editMode && (
        <View style={styles.editContainer}>
          <View style={styles.imageContainer}>
            <Image
              source={{
                uri: "https://fbi.cults3d.com/uploaders/32338244/illustration-file/f23d00d1-2861-40af-8e83-1e8f5eda7403/WhatsApp-Image-2024-08-27-at-6.36.25-PM.jpeg",
              }}
            />
            <Pressable>
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
                placeholder="Username"
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
                placeholder="Password"
                placeholderTextColor={"gray"}
              />
            </View>
            <View style={styles.border}></View>
          </View>
          <View style={styles.buttonWrapper}>
            <Pressable onPress={toggleEditMode}>
              <Text>Cancel</Text>
            </Pressable>
            <Pressable onPress={handleUpdatePlayer}>
              <Text>Save</Text>
            </Pressable>
          </View>
        </View>
      )}
      <Pressable onPress={handleLogout}>
        <Text>Logout</Text>
      </Pressable>
    </View>
  );
}
