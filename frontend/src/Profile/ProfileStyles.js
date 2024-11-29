import { StyleSheet } from "react-native";
import { verticalScale, moderateScale, horizontalScale } from "../Shared/assets/constants/Dimentions";
import { Colors } from "../Shared/assets/constants/Colors";

export const styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    justifyContent: "center",
    alignItems: "center",
  },

  inputContainer: {
    rowGap: verticalScale(5),
  },

  inputContainer: {
    rowGap: verticalScale(5),
  },

  iconAndInput: {
    columnGap: horizontalScale(15),
    flexDirection: "row",
  },

  icon: {
    paddingLeft: horizontalScale(5),
  },

  border: {
    width: horizontalScale(261),
    height: verticalScale(9),
    backgroundColor: Colors.BorderGray,
    borderRadius: moderateScale(15),
  },

  textInput: {
    fontSize: moderateScale(22),
    fontFamily: "InikaBold",
  },

  editContainer: {
    justifyContent: "center",
    alignItems: "center",
    gap: verticalScale(20)
  },

  buttonWrapper: {
    flexDirection: "row",
    gap: horizontalScale(10)
  },

  imageContainer: {
    width: horizontalScale(200),
    height: verticalScale(100),
    backgroundColor: "red"
  },

  nonEditContainer: {
    justifyContent: "center",
    alignItems: "center",
    gap: verticalScale(20)
  }
});
