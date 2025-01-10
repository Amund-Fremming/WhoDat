import { IBoardCard } from '@/src/Game/types/BoardTypes';
import { Pressable, View, Text } from 'react-native';
import { Image } from 'expo-image';
import { styles, imageStyles } from './FlipCardStyles';
import StrokedText from '@/src/Shared/components/StokedText/StrokedText';
import { useEffect, useState } from 'react';
import React from 'react';
import { Colors } from '@/src/Shared/assets/constants/Colors';

interface FlipCardProps {
  boardcard: IBoardCard;
  onCardPress: () => void;
  guessMode: boolean;
  cardToGuess: number;
}

const blurhash =
  '|rF?hV%2WCj[ayj[a|j[az_NaeWBj@ayfRayfQfQM{M|azj[azf6fQfQfQIpWXofj[ayj[j[fQayWCoeoeaya}j[ayfQa{oLj?j[WVj[ayayj[fQoff7azayj[ayj[j[ayofayayayj[fQj[ayayj[ayfjj[j[ayjuayj[';

export default function FlipCard({
  boardcard,
  onCardPress,
  guessMode,
  cardToGuess,
}: FlipCardProps) {
  const [flipped, setFlipped] = useState<boolean>(false);
  const [rimColor, setRimColor] = useState<string>(Colors.Black);

  useEffect(() => {
    if (cardToGuess === boardcard.id && guessMode) {
      setRimColor(Colors.Green);
      return;
    }
    setRimColor(Colors.Black);
  }, [cardToGuess, guessMode]);

  const handleCardPressed = () => {
    if (!guessMode) {
      setFlipped(!flipped);
      return;
    }
    onCardPress();
  };

  return (
    <Pressable style={styles.container} onPress={handleCardPressed}>
      {!flipped && (
        <>
          <View style={{ ...styles.card, backgroundColor: rimColor }}>
            <Image
              transition={300}
              placeholder={{ blurhash }}
              style={imageStyles.imageStyle}
              source={{
                uri: boardcard.card.url,
              }}
            />
          </View>
          <StrokedText
            font="Inika"
            color={Colors.Cream}
            text={boardcard.card.name}
            fontBaseSize={14}
            smallBorder={true}
          />
        </>
      )}
      {flipped && (
        <View style={styles.flippedCard}>
          <View style={styles.flippedCardInnerRim}>
            <Text style={styles.flippedCardText}>?</Text>
          </View>
        </View>
      )}
    </Pressable>
  );
}
