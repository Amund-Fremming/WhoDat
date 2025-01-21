import { Pressable, View } from 'react-native';
import { Image } from 'expo-image';
import { styles, imageStyles } from './CardStyles';
import StrokedText from '@/src/Shared/components/StokedText/StrokedText';
import { ICardDto } from '@/src/Shared/types/CardTypes';
import { Colors } from '@/src/Shared/assets/constants/Colors';

interface CardComponentProps {
  card: ICardDto;
  handleCardPressed: () => void;
  isActive: boolean;
}
const blurhash =
  '|rF?hV%2WCj[ayj[a|j[az_NaeWBj@ayfRayfQfQM{M|azj[azf6fQfQfQIpWXofj[ayj[j[fQayWCoeoeaya}j[ayfQa{oLj?j[WVj[ayayj[fQoff7azayj[ayj[j[ayofayayayj[fQj[ayayj[ayfjj[j[ayjuayj[';

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
          placeholder={{ blurhash }}
          style={imageStyles.imageStyle}
          source={{
            uri: card.url,
          }}
        />
      </View>
      <StrokedText
        font="Inika"
        color={Colors.Cream}
        text={card.name}
        fontBaseSize={14}
        smallBorder={true}
      />
    </Pressable>
  );
}
