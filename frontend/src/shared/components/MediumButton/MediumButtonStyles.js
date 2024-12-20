import { StyleSheet } from "react-native";
import {
  moderateScale,
  verticalScale,
} from "../../assets/constants/Dimentions";

export default styles = StyleSheet.create({
  button: {
    width: verticalScale(140),
    height: moderateScale(50),
    justifyContent: "center",
    alignItems: "center",
    borderWidth: moderateScale(4),
    borderRadius: moderateScale(15),
  },

  text: {
    fontSize: moderateScale(24),
    fontFamily: "Inika",
    textAlign: "center",
    fontWeight: "bold",
  },
});
