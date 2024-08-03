import { StyleSheet } from "react-native";
import { Colors } from "@/constants/Colors";
import {
  horizontalScale,
  moderateScale,
  verticalScale,
} from "@/constants/Dimentions";

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
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.Cream,
    borderRadius: moderateScale(20),
    width: horizontalScale(345),
    height: verticalScale(350),
    rowGap: verticalScale(30),
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

  textInput: {
    fontSize: moderateScale(22),
    fontFamily: "InikaBold",
  },

  border: {
    width: horizontalScale(261),
    height: verticalScale(9),
    backgroundColor: Colors.BorderGray,
    borderRadius: moderateScale(15),
  },
});
