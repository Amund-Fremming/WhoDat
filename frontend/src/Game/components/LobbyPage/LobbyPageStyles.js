import { Colors } from "@/src/Shared/assets/constants/Colors";
import { StyleSheet } from "react-native";
import {
  moderateScale,
  verticalScale,
  horizontalScale,
} from "@/src/Shared/assets/constants/Dimentions";

export default styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    justifyContent: "flex-start",
    alignItems: "center",
    backgroundColor: Colors.Orange,
  },

  header: {
    fontFamily: "Modak",
    fontSize: moderateScale(70),
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

  header2: {
    fontFamily: "Modak",
    fontSize: moderateScale(60),
    color: Colors.Cream,
    textAlign: "center",
    lineHeight: moderateScale(90),
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
