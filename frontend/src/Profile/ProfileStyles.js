import { StyleSheet } from "react-native";
import {
  verticalScale,
  moderateScale,
  horizontalScale,
} from "../Shared/assets/constants/Dimentions";
import { Colors } from "../Shared/assets/constants/Colors";

export const styles = StyleSheet.create({
  inputContainer: {
    rowGap: verticalScale(5),
  },

  inputContainer: {
    rowGap: verticalScale(5),
  },

  iconAndInput: {
    columnGap: horizontalScale(15),
    flexDirection: "row",
  },

  icon: {
    paddingLeft: horizontalScale(5),
  },

  border: {
    width: horizontalScale(261),
    height: verticalScale(9),
    backgroundColor: Colors.BorderGray,
    borderRadius: moderateScale(15),
  },

  textInput: {
    fontSize: moderateScale(22),
    fontFamily: "InikaBold",
  },

  editContainer: {
    justifyContent: "center",
    alignItems: "center",
    gap: verticalScale(20),
    height: "100%",
  },

  buttonWrapper: {
    flexDirection: "row",
    gap: horizontalScale(10),
    paddingTop: verticalScale(40),
  },

  nonEditContainer: {
    justifyContent: "center",
    alignItems: "center",
    gap: verticalScale(20),
    height: "100%",
  },

  username: {
    fontFamily: "InikaBold",
    fontSize: moderateScale(40),
    paddingBottom: verticalScale(20),
  },

  container: {
    height: "100%",
    width: "100%",
    alignItems: "center",
    justifyContent: "flex-end",
    backgroundColor: Colors.Orange,
    paddingTop: verticalScale(40),
  },

  creamContainer: {
    width: "100%",
    height: "85%",
    alignItems: "center",
    justifyContent: "space-between",
    backgroundColor: Colors.Cream,
    borderTopStartRadius: moderateScale(30),
    borderTopRightRadius: moderateScale(30),
  },

  header: {
    fontSize: moderateScale(45),
    fontFamily: "Modak",
    color: Colors.Cream,
  },

  imageContainer: {
    width: horizontalScale(270),
    height: verticalScale(270),
    backgroundColor: Colors.DarkGray,
    borderRadius: moderateScale(400),
    justifyContent: "center",
    alignItems: "center",
    marginTop: verticalScale(40),
  },

  uploadButton: {
    position: "absolute",
    borderRadius: moderateScale(10),
    paddingHorizontal: horizontalScale(20),
    paddingVertical: verticalScale(7),
    backgroundColor: Colors.DarkGray,
    right: horizontalScale(10),
    bottom: verticalScale(10),
    backgroundColor: "rgba(0, 0, 0, 0.8)",
  },
});

export const imageStyles = StyleSheet.create({
  imageStyle: {
    width: "96%",
    height: "96%",
    borderRadius: moderateScale(400),
  },
});
