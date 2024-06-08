import { Colors } from "@/constants/Colors";
import { moderateScale, verticalScale } from "@/constants/Dimentions";
import { StyleSheet } from "react-native";

export default styles = StyleSheet.create({
    container: {
        height: "100%",
        width: "100%",
        alignItems: "center",
        backgroundColor: Colors.Orange,
        paddingTop: verticalScale(40),
    },

    header: {
        fontSize: moderateScale(45),
        fontFamily: "Modak",
        color: Colors.Cream,
    },

    creamContainer: {
        width: "100%",
        height: "100%",
        backgroundColor: Colors.Cream,
        borderTopStartRadius: moderateScale(30),
        borderTopRightRadius: moderateScale(30),
        alignItems: "center",
        justifyContent: "center",
    },

    boardContainer: {
        width: "90%",
        height: "100%",
        marginTop: "10%",
    },
});
