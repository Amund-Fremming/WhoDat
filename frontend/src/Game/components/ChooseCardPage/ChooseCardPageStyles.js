import { Colors } from "@/src/Shared/assets/constants/Colors";
import { StyleSheet } from "react-native";
import { horizontalScale, verticalScale, moderateScale } from "@/src/Shared/assets/constants/Dimentions";

export const styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    alignItems: "center",
    backgroundColor: Colors.Orange,
    paddingTop: verticalScale(40),
  },

  header: {
    fontFamily: "Modak",
    fontSize: moderateScale(75),
    color: Colors.Cream,
    textAlign: "center",
    lineHeight: moderateScale(90),
  },

  creamContainer: {
    width: "100%",
    height: "93%",
    alignItems: "center",
    justifyContent: "space-between",
    backgroundColor: Colors.Cream,
    borderTopStartRadius: moderateScale(30),
    borderTopRightRadius: moderateScale(30),
  },

  boardContainer: {
    width: "100%",
    marginTop: "5%",
    gap: moderateScale(7),
    flexDirection: "row",
    flexWrap: "wrap",
    alignItems: "center",
    justifyContent: "center",
    marginBottom: verticalScale(30),
  },

  backIconWrapper: {
    position: "absolute",
    left: horizontalScale(20),
    top: verticalScale(48),
  },

    header: {
      fontSize: moderateScale(45),
      fontFamily: "Modak",
      color: Colors.Cream,
    },

    buttonWrapper: {
      paddingBottom: verticalScale(30),
      width: "100%",
      justifyContent: "center",
      alignItems: "center",
      flexDirection: "row",
      gap: horizontalScale(20),
    },

    infoText: {
      font: "Inika",
      fontSize: moderateScale(20),
      color: Colors.DarkGray,
      paddingTop: "20%",
      paddingLeft: "10%",
      paddingRight: "10%",
      textAlign: "center",
      lineHeight: verticalScale(30)
    }
});
