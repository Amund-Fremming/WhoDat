import { IBoardCard } from '@/src/Game/types/BoardTypes';
import { Pressable, View, Image, Text } from 'react-native';
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
