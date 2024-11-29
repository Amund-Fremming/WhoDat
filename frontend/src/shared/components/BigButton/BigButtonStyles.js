import {
  moderateScale,
  verticalScale,
} from "../../assets/constants/Dimentions";
import { StyleSheet } from "react-native";

export const styles = StyleSheet.create({
  button: {
    width: verticalScale(235),
    height: moderateScale(58),
    justifyContent: "center",
    alignItems: "center",
    borderWidth: moderateScale(4),
    borderRadius: moderateScale(20),
  },

  text: {
    fontSize: moderateScale(30),
    fontFamily: "Inika",
    textAlign: "center",
    fontWeight: "bold",
  },
});
