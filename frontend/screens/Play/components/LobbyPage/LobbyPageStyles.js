import { Colors } from "@/constants/Colors";
import { moderateScale, verticalScale } from "@/constants/Dimentions";
import { StyleSheet } from "react-native";

export default styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    justifyContent: "flex-start",
    alignItems: "center",
    backgroundColor: Colors.Orange,
    paddingTop: verticalScale(130),
  },

  header: {
    fontFamily: "Modak",
    fontSize: moderateScale(75),
    color: Colors.Cream,
    textAlign: "center",
    lineHeight: moderateScale(90),
  },
});
