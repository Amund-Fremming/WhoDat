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
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.Orange,
    gap: verticalScale(20),
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
});
