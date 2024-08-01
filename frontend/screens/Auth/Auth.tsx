import { View, Text } from "react-native";
import styles from "./AuthStyles";
import { Login } from "./components/Login/Login";
import { Register } from "./components/Register/Register";

export default function Auth() {
    return <Register />;
    return <Login />;

    return (
        <View style={styles.container}>
            <Text>Auth</Text>
        </View>
    );
}
