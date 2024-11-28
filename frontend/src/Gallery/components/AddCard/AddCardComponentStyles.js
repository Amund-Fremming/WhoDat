import { StyleSheet } from "react-native";
import {
  verticalScale,
  horizontalScale,
  moderateScale,
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

  innerCard: {
    height: "87%",
    width: "85%",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.BurgundyRed,
    borderRadius: moderateScale(6),
  },

  text: {
    fontFamily: "InikaBold",
    fontSize: moderateScale(23),
    color: Colors.Cream,
  },
});
