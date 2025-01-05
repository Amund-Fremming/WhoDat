import { IBoardCard } from '@/src/Game/types/BoardTypes';
import { Pressable, View, Image, Text } from 'react-native';
import { styles, imageStyles } from './FlipCardStyles';
import StrokedText from '@/src/Shared/components/StokedText/StrokedText';
import { useState } from 'react';
import React from 'react';

interface FlipCardProps {
  boardcard: IBoardCard;
  onCardPress: () => void;
}

export default function FlipCard({ boardcard, onCardPress }: FlipCardProps) {
  const [flipped, setFlipped] = useState<boolean>(false);

  const handleCardPressed = () => {
    setFlipped(!flipped);
    onCardPress();
  };

  return (
    <Pressable style={styles.container} onPress={handleCardPressed}>
      {!flipped && (
        <>
          <View style={styles.card}>
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
