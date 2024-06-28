import { Colors } from "@/constants/Colors";
import {
  moderateScale,
  verticalScale,
  horizontalScale,
} from "@/constants/Dimentions";
import { StyleSheet } from "react-native";

export default styles = StyleSheet.create({
  container: {
    height: verticalScale(70),
    width: horizontalScale(330),
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.Cream,
    borderRadius: moderateScale(25),
  },

  wrapper: {
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center",
  },

  text: {
    fontFamily: "InikaBold",
    fontSize: moderateScale(40),
    color: Colors.BurgundyRed,
    width: horizontalScale(250),
    paddingLeft: horizontalScale(10),
  },

  iconWidth: {
    width: horizontalScale(50),
    paddingLeft: horizontalScale(10),
  },
});
