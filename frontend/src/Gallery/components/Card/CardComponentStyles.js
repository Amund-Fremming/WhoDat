import { StyleSheet } from "react-native";
import {
  verticalScale,
  moderateScale,
  horizontalScale,
} from "@/src/Shared/assets/constants/Dimentions";
import { Colors } from "@/src/Shared/assets/constants/Colors";

export const styles = StyleSheet.create({
  container: {
    width: horizontalScale(68),
    height: verticalScale(130),
    justifyContent: "flex-start",
    alignItems: "center",
  },

  card: {
    height: verticalScale(110),
    width: "100%",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "black",
    borderRadius: moderateScale(6),
  },

  innerRim: {
    width: "88%",
    height: "91%",
    backgroundColor: Colors.Cream,
    borderRadius: moderateScale(6),
  },
});

export const imageStyles = StyleSheet.create({
  imageStyle: {
    width: "88%",
    height: "91%",
    borderRadius: moderateScale(6),
  },
});
