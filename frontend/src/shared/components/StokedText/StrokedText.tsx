import { Text, View } from 'react-native';
import { styles } from './StrokedTextStyles';
import { moderateScale } from '@/src/Shared/assets/constants/Dimentions';

interface StokedTextProps {
  text: string;
  fontBaseSize: number;
  smallBorder: boolean;
  color: string;
  font: string;
}

export default function StrokedText({
  text,
  fontBaseSize,
  smallBorder,
  color,
  font,
}: StokedTextProps) {
  if ((text.match(/m/gi) || []).length > 3) text = 'Kenneth';
  if ((text.match(/w/gi) || []).length > 3) text = 'Kenneth';

  const adjustFontSize = (name: string) => {
    let wideCharacters = ['M', 'W', 'm', 'w'];
    let wideCount = name
      .split('')
      .filter((char: string) => wideCharacters.includes(char)).length;

    if (wideCount > 3) {
      return moderateScale(fontBaseSize - 2);
    } else if (name.length > 7) {
      return moderateScale(fontBaseSize - 2);
    } else {
      return moderateScale(fontBaseSize);
    }
  };

  return (
    <View style={styles.container}>
      <Text
        style={[
          {
            ...styles.text,
            fontSize: adjustFontSize(text),
            color: color,
            fontFamily: font,
          },
          smallBorder ? styles.smallShadowRight : styles.bigShadowRight,
        ]}
      >
        {text}
      </Text>
      <Text
        style={[
          {
            ...styles.text,
            fontSize: adjustFontSize(text),
            color: color,
            fontFamily: font,
          },
          smallBorder ? styles.smallShadowLeft : styles.bigShadowLeft,
        ]}
      >
        {text}
      </Text>
      <Text
        style={[
          {
            ...styles.text,
            fontSize: adjustFontSize(text),
            color: color,
            fontFamily: font,
          },
          smallBorder ? styles.smallShadowTop : styles.bigShadowTop,
        ]}
      >
        {text}
      </Text>
      <Text
        style={[
          {
            ...styles.text,
            fontSize: adjustFontSize(text),
            color: color,
            fontFamily: font,
          },
          smallBorder ? styles.smallShadowBottom : styles.bigShadowBottom,
        ]}
      >
        {text}
      </Text>
      <Text
        style={{
          ...styles.text,
          fontSize: adjustFontSize(text),
          color: color,
          fontFamily: font,
        }}
      >
        {text}
      </Text>
    </View>
  );
}
