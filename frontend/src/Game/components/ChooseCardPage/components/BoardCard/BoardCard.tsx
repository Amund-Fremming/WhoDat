import { Image, Pressable, Text, View } from 'react-native';
import { styles, imageStyles } from './BoardCardStyles';
import StrokedText from '@/src/Shared/components/StokedText/StrokedText';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { IBoardCard } from '@/src/Game/types/BoardTypes';

interface CardComponentProps {
  boardcard: IBoardCard;
  handleCardPressed: () => void;
  isActive: boolean;
}

export default function BoardCard({
  boardcard,
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
            uri: boardcard.card.url,
          }}
        />
      </View>
      <StrokedText
        text={boardcard.card.name}
        fontBaseSize={14}
        smallBorder={true}
      />
    </Pressable>
  );
}
