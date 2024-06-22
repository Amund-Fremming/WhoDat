import { StyleSheet } from "react-native";
import { moderateScale } from "@/constants/Dimentions";

export default styles = StyleSheet.create({
    container: {
        width: 80,
        height: 100,
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: "red",
    },

    text: {
        fontSize: moderateScale(16),
        fontFamily: "Inika",
        textAlign: "center",
        fontWeight: "bold",
    },
});
