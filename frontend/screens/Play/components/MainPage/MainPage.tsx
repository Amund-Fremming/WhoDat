import { View, Text } from "react-native";
import styles from "./MainPageStyles";

export default function MainPage() {
    return (
        <View style={styles.container}>
            <Text style={styles.header}>Time to cook!</Text>
        </View>
    );
}
