import { StyleSheet } from "react-native";
import { Colors } from "../../assets/constants/Colors";
import { verticalScale, moderateScale, horizontalScale } from "../../assets/constants/Dimentions";

export const styles = StyleSheet.create({
  container: {
    justifyContent: "center",
    alignItems: "center",
    flex: 1,
    backgroundColor: "transparent",
  },

  modal: {
    alignItems: "center",
    width: "95%",
    paddingBottom: verticalScale(105),
    backgroundColor: Colors.Cream,
    borderColor: Colors.Red,
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

  message: {
    fontSize: moderateScale(24),
    fontFamily: "Inika",
    textAlign: "center",
    fontWeight: "bold",
    color: Colors.DarkGray,
    paddingHorizontal: horizontalScale(10),
  },

  header: {
    fontSize: moderateScale(84),
    fontFamily: "Modak",
    textAlign: "center",
    fontWeight: "bold",
    color: Colors.Red,
  },

  absoluteButton: {
    position: "absolute",
    bottom: verticalScale(20)
  }
});
