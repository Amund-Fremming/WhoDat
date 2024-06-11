import { View, Text } from "react-native";
import styles from "./GalleryStyles";
import BigButton from "@/components/BigButton/BigButton";
import { Colors } from "@/constants/Colors";

export default function Gallery() {
    return (
        <View style={styles.container}>
            <Text style={styles.header}>Gallery</Text>

            <View style={styles.creamContainer}>
                <View style={styles.boardContainer}></View>
                <BigButton
                    text="Edit"
                    color={Colors.BurgundyRed}
                    inverted={false}
                    onButtonPress={() => console.log("hey!")}
                />
            </View>
        </View>
    );
}
