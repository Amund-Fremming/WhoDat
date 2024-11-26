import { Colors } from "@/constants/Colors";
import {
  horizontalScale,
  moderateScale,
  verticalScale,
} from "@/constants/Dimentions";
import { StyleSheet } from "react-native";

export default styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.Orange,
    gap: verticalScale(20),
  },

  header: {
    fontFamily: "Modak",
    fontSize: moderateScale(75),
    color: Colors.Cream,
    textAlign: "center",
    lineHeight: moderateScale(100),
    shadowRadius: 5,
    shadowOpacity: 0.2,
    shadowOffset: {
      height: verticalScale(3),
      width: horizontalScale(3),
    },
  },

  textWrapper: {},

  backIconWrapper: {
    position: "absolute",
    left: horizontalScale(20),
    top: verticalScale(60),
  },
});
