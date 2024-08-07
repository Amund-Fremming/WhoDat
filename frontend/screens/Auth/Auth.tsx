import { View, Text } from "react-native";
import styles from "./AuthStyles";
import { Login } from "./components/Login/Login";
import { Register } from "./components/Register/Register";
import { useState } from "react";

export default function Auth() {
  const [view, setView] = useState<string>("LOGIN");

  if (view === "LOGIN") return <Login setView={setView} />;
  if (view === "REGISTER") return <Register setView={setView} />;
  else
    return (
      <View style={styles.container}>
        <Text>Something went wrong!</Text>
      </View>
    );
}
