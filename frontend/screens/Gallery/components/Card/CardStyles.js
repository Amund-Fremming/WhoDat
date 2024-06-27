import { StyleSheet } from "react-native";
import {
  horizontalScale,
  moderateScale,
  verticalScale,
} from "@/constants/Dimentions";
import { Colors } from "@/constants/Colors";

export default styles = StyleSheet.create({
  container: {
    width: horizontalScale(68),
    height: verticalScale(130),
    justifyContent: "flex-start",
    alignItems: "center",
  },

  outerRim: {
    height: verticalScale(100),
    width: "100%",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "black",
    borderRadius: moderateScale(6),
  },

  innerRim: {
    width: "88%",
    height: "91%",
    overflow: "hidden",
    backgroundColor: Colors.Cream,
    borderRadius: moderateScale(6),
  },

  imageStyle: {
    width: "100%",
    height: "100%",
  },
});
