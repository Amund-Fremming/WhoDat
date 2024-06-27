import { StyleSheet } from "react-native";
import {
  moderateScale,
  verticalScale,
  horizontalScale,
} from "@/constants/Dimentions";
import { Colors } from "@/constants/Colors";

export const styles = StyleSheet.create({
  container: {
    justifyContent: "center",
    alignItems: "center",
    flex: 1,
  },

  cardModal: {
    justifyContent: "space-evenly",
    alignItems: "center",
    width: "95%",
    height: "70%",
    backgroundColor: Colors.Cream,
    borderColor: Colors.BurgundyRed,
    borderWidth: moderateScale(5),
    borderRadius: moderateScale(30),
  },

  card: {
    width: "70%",
    height: "70%",
    backgroundColor: Colors.DarkGray,
    borderRadius: moderateScale(15),
    justifyContent: "center",
    alignItems: "center",
  },
});

export const imageStyles = StyleSheet.create({
  imageStyle: {
    width: "91%",
    height: "94%",
    backgroundColor: Colors.Cream,
    borderRadius: moderateScale(6),
  },
});
