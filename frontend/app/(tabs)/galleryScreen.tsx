import { StyleSheet, View, Text } from "react-native";

export default function GalleryScreen() {
  return (
    <View style={styles.container}>
      <Text>Gallery</Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    width: "100%",
    height: "100%",
    justifyContent: "center",
    alignItems: "center",
  },
});
