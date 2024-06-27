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
    fontSize: moderateScale(16),
    fontFamily: "Inika",
    textAlign: "center",
    fontWeight: "bold",
    color: Colors.Cream,
    position: "absolute",
  },

  shadowRight: {
    textShadowOffset: { width: 1, height: 0 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  shadowLeft: {
    textShadowOffset: { width: -1, height: 0 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  shadowTop: {
    textShadowOffset: { width: 0, height: -1 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },

  shadowBottom: {
    textShadowOffset: { width: 0, height: 1 },
    textShadowRadius: 0,
    textShadowColor: "#000000",
  },
});
