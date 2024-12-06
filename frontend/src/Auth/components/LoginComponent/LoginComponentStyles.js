import { StyleSheet } from "react-native";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import {
  verticalScale,
  horizontalScale,
  moderateScale,
} from "@/src/Shared/assets/constants/Dimentions";

export const styles = StyleSheet.create({
  container: {
    justifyContent: "center",
    alignItems: "center",
    width: "100%",
    height: "100%",
    backgroundColor: Colors.Orange,
  },

  header: {
    fontSize: moderateScale(80),
    color: Colors.Cream,
    fontFamily: "Modak",
  },

  card: {
    paddingTop: verticalScale(40),
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.Cream,
    borderRadius: moderateScale(20),
    width: horizontalScale(345),
    rowGap: verticalScale(30),
    paddingBottom: verticalScale(40),
  },

  inputContainer: {
    rowGap: verticalScale(5),
  },

  iconAndInput: {
    columnGap: horizontalScale(15),
    flexDirection: "row",
    width: horizontalScale(180),
    paddingLeft: horizontalScale(2),
  },

  icon: {
    paddingLeft: horizontalScale(5),
  },

  textInput: {
    fontSize: moderateScale(22),
    fontFamily: "InikaBold",
    width: horizontalScale(180),
    marginLeft: horizontalScale(5),
  },

  border: {
    width: horizontalScale(261),
    height: verticalScale(9),
    backgroundColor: Colors.BorderGray,
    borderRadius: moderateScale(15),
  },

  loginAndRegisterNew: {
    rowGap: verticalScale(10),
    justifyContent: "center",
    alignItems: "center",
  },

  registerNewText: {
    color: Colors.BurgundyRed,
    fontFamily: "InikaBold",
    fontSize: moderateScale(14),
  },
});
