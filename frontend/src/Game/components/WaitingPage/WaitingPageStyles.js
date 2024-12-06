import { Colors } from "@/src/Shared/assets/constants/Colors";
import {
  moderateScale,
  verticalScale,
  horizontalScale,
} from "@/src/Shared/assets/constants/Dimentions";
import { StyleSheet } from "react-native";

export const styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    justifyContent: "flex-start",
    alignItems: "center",
    backgroundColor: Colors.Orange,
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

  headerWrapper: {
    paddingTop: verticalScale(130),
  },

  id: {
    paddingTop: verticalScale(15),
    fontFamily: "Modak",
    fontSize: moderateScale(30),
    color: Colors.Green,
    textAlign: "center",
    lineHeight: moderateScale(100),
  },
});
