import { StyleSheet, View, Text } from "react-native";

export default function PlayScreen() {
  return (
    <View style={styles.container}>
      <Text>Play</Text>
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
