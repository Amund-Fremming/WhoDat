import { moderateScale, verticalScale } from "@/constants/Dimentions";
import { StyleSheet } from "react-native";

export default styles = StyleSheet.create({
    button: {
        height: verticalScale(170),
        width: moderateScale(80),
        justifyContent: "center",
        alignItems: "center",
        borderWidth: moderateScale(3),
    },

    text: {
        fontSize: moderateScale(25),
        fontFamily: "Modak",
    },
});
