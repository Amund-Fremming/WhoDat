import { StyleSheet } from "react-native";
import {
  moderateScale,
  verticalScale,
  horizontalScale,
} from "@/src/Shared/assets/constants/Dimentions";
import { Colors } from "@/src/Shared/assets/constants/Colors";

export const styles = StyleSheet.create({
  container: {
    justifyContent: "center",
    alignItems: "center",
    flex: 1,
  },

  cardModal: {
    justifyContent: "space-between",
    alignItems: "center",
    width: "95%",
    height: "73%",
    backgroundColor: Colors.Cream,
    borderColor: Colors.BorderGray,
    borderWidth: moderateScale(5),
    borderRadius: moderateScale(30),
    paddingTop: verticalScale(40),
    shadowColor: Colors.Black,
    shadowOpacity: 0.5,
    shadowRadius: 9,
    shadowOffset: {
      width: horizontalScale(15),
      height: verticalScale(15),
    },
  },

  card: {
    width: "70%",
    height: "70%",
    backgroundColor: Colors.DarkGray,
    borderRadius: moderateScale(15),
    justifyContent: "center",
    alignItems: "center",
    marginBottom: verticalScale(-20),
  },

  buttonWrapper: {
    marginBottom: verticalScale(30),
  },

  closeButton: {
    position: "absolute",
    zIndex: 1,
    right: horizontalScale(16),
    top: verticalScale(12),
  },

  newNameInput: {
    fontSize: moderateScale(40),
    fontFamily: "Inika",
    textAlign: "center",
    fontWeight: "bold",
    color: Colors.DarkGray,
  },

  deleteButton: {
    position: "absolute",
    zIndex: 1,
    left: -15,
    top: -15,
    borderRadius: 100,
    backgroundColor: Colors.Cream,
  },
});

export const imageStyles = StyleSheet.create({
  imageStyle: {
    width: "91%",
    height: "94%",
    borderRadius: moderateScale(6),
  },
});
