import { Colors } from "../Shared/assets/constants/Colors";
import {
  horizontalScale,
  verticalScale,
  moderateScale,
} from "../Shared/assets/constants/Dimentions";
import { StyleSheet } from "react-native";

export default styles = StyleSheet.create({
  container: {
    height: "100%",
    width: "100%",
    alignItems: "center",
    backgroundColor: Colors.Orange,
    paddingTop: verticalScale(40),
  },

  header: {
    fontSize: moderateScale(45),
    fontFamily: "Modak",
    color: Colors.Cream,
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

  buttonWrapper: {
    paddingBottom: verticalScale(30),
    width: "100%",
    justifyContent: "center",
    alignItems: "center",
    flexDirection: "row",
    gap: horizontalScale(20),
  },
});
