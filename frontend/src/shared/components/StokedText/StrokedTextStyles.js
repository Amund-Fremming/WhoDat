import { StyleSheet } from "react-native";
import { Colors } from "../../assets/constants/Colors";

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

  bigShadowRight: {
    textShadowOffset: { width: 2.5, height: 0 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  bigShadowLeft: {
    textShadowOffset: { width: -2.5, height: 0 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  bigShadowTop: {
    textShadowOffset: { width: 0, height: -2.5 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  bigShadowBottom: {
    textShadowOffset: { width: 0, height: 2.5 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },
});
