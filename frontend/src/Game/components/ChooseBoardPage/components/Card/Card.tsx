import { Image, Pressable, Text, View } from 'react-native';
import { styles, imageStyles } from './CardStyles';
import StrokedText from '@/src/Shared/components/StokedText/StrokedText';
import { ICardDto } from '@/src/Shared/domain/CardTypes';
import { Colors } from '@/src/Shared/assets/constants/Colors';

interface CardComponentProps {
  card: ICardDto;
  handleCardPressed: () => void;
  isActive: boolean;
}

export default function Card({
  card,
  handleCardPressed,
  isActive,
}: CardComponentProps) {
  return (
    <Pressable style={styles.container} onPress={handleCardPressed}>
      <View
        style={{
          ...styles.card,
          backgroundColor: isActive ? Colors.Green : Colors.Black,
        }}
      >
        <Image
          style={imageStyles.imageStyle}
          source={{
            uri: card.url,
          }}
        />
      </View>
      <StrokedText text={card.name} fontBaseSize={14} smallBorder={true} />
    </Pressable>
  );
}
