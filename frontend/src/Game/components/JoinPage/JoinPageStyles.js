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
    gap: verticalScale(20),
    paddingTop: verticalScale(130),
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

  backIconWrapper: {
    position: "absolute",
    left: horizontalScale(20),
    top: verticalScale(60),
  },

  box: {
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.Cream,
    width: "88%",
    height: "35%",
    borderRadius: moderateScale(25),
    shadowRadius: 5,
    shadowOpacity: 0.3,
    shadowOffset: {
      height: verticalScale(3),
      width: horizontalScale(3),
    },
  },

  inputWrapper: {
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
    paddingBottom: verticalScale(25),
  },

  textInput: {
    fontSize: moderateScale(28),
    fontFamily: "Inika",
    paddingBottom: verticalScale(9),
    color: Colors.Black,
  },

  underline: {
    width: verticalScale(225),
    height: verticalScale(7),
    backgroundColor: Colors.BorderGray,
    borderRadius: moderateScale(25),
  },
});
