import { Text, View } from "react-native";
import styles from "./StrokedTextStyles";

interface StokedTextProps {
  text: string;
  fontBaseSize: number;
}

export default function StrokedText({ text, fontBaseSize }: StokedTextProps) {
  if (text.length > 9) text = "Kenneth";
  if ((text.match(/m/gi) || []).length > 3) text = "Kenneth";
  if ((text.match(/w/gi) || []).length > 3) text = "Kenneth";

  const adjustFontSize = (name: string) => {
    let wideCharacters = ["M", "W", "m", "w"];
    let wideCount = name
      .split("")
      .filter((char: string) => wideCharacters.includes(char)).length;

    if (wideCount > 3) {
      return fontBaseSize - 2;
    } else if (name.length > 7) {
      return fontBaseSize - 2;
    } else {
      return fontBaseSize;
    }
  };

  return (
    <View style={styles.container}>
      <Text
        style={[
          { ...styles.text, fontSize: adjustFontSize(text) },
          styles.shadowRight,
        ]}
      >
        {text}
      </Text>
      <Text
        style={[
          { ...styles.text, fontSize: adjustFontSize(text) },
          styles.shadowLeft,
        ]}
      >
        {text}
      </Text>
      <Text
        style={[
          { ...styles.text, fontSize: adjustFontSize(text) },
          styles.shadowTop,
        ]}
      >
        {text}
      </Text>
      <Text
        style={[
          { ...styles.text, fontSize: adjustFontSize(text) },
          styles.shadowBottom,
        ]}
      >
        {text}
      </Text>
      <Text style={{ ...styles.text, fontSize: adjustFontSize(text) }}>
        {text}
      </Text>
    </View>
  );
}
