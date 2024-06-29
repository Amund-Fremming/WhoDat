import { Colors } from "@/constants/Colors";
import {
  moderateScale,
  verticalScale,
  horizontalScale,
} from "@/constants/Dimentions";
import { StyleSheet } from "react-native";

export default styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.Orange,
    paddingBottom: verticalScale(130),
  },

  header: {
    fontFamily: "Modak",
    fontSize: moderateScale(75),
    color: Colors.Cream,
    textAlign: "center",
    lineHeight: moderateScale(100),
  },

  backIconWrapper: {
    position: "absolute",
    left: horizontalScale(20),
    top: verticalScale(60),
  },
});
