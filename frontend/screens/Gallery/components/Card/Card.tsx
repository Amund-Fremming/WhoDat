import { ICard } from "@/interfaces/ICard";
import { View, Text } from "react-native";
import styles from "./CardStyles";

interface CardProps {
    card: ICard | null;
    onCardPress: () => void;
}

export default function Card({ card, onCardPress }: CardProps) {
    return (
        <View style={styles.container}>
            <Text style={styles.text}>Card</Text>
        </View>
    );
}
