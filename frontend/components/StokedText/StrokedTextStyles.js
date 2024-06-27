import { StyleSheet } from "react-native";
import { moderateScale } from "@/constants/Dimentions";
import { Colors } from "@/constants/Colors";

export default styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },

  text: {
    fontFamily: "Inika",
    textAlign: "center",
    fontWeight: "bold",
    color: Colors.Cream,
    position: "absolute",
  },

  smallShadowRight: {
    textShadowOffset: { width: 1, height: 0 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  smallShadowLeft: {
    textShadowOffset: { width: -1, height: 0 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  smallShadowTop: {
    textShadowOffset: { width: 0, height: -1 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  smallShadowBottom: {
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  BigShadowRight: {
    textShadowOffset: { width: 1, height: 0 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  BigShadowLeft: {
    textShadowOffset: { width: -1, height: 0 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  BigShadowTop: {
    textShadowOffset: { width: 0, height: -1 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  BigShadowBottom: {
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },
});
