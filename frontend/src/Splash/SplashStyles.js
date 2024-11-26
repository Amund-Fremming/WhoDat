import { StyleSheet } from "react-native";
import { Colors } from "@/constants/Colors";
import { moderateScale } from "@/constants/Dimentions";

export const styles = StyleSheet.create({
    container: {
        justifyContent: "center",
        alignItems: "center",
        width: "100%",
        height: "100%",
        backgroundColor: Colors.Orange,
    },

    text: {
        color: Colors.Cream,
        font: "Modak",
        fontSize: moderateScale(90),
    },
});
