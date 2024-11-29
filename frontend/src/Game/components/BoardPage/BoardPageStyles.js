import { Colors } from "@/src/Shared/assets/constants/Colors";
import { moderateScale } from "@/src/Shared/assets/constants/Dimentions";
import { StyleSheet } from "react-native";

export const styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: Colors.Orange,
  },

  header: {
    fontFamily: "Modak",
    fontSize: moderateScale(75),
    color: Colors.Cream,
    textAlign: "center",
    lineHeight: moderateScale(90),
  },
});
