import { Image, Pressable, Text, View } from "react-native";
import { styles, imageStyles } from "./CardStyles";
import StrokedText from "@/src/Shared/components/StokedText/StrokedText";
import { ICard } from "@/src/Shared/domain/CardTypes";
import { useState } from "react";
import { Colors } from "@/src/Shared/assets/constants/Colors";

interface CardComponentProps {
    card: ICard;
    setCardsPressed: React.Dispatch<React.SetStateAction<Set<number>>>;
    setCardsChoosen: React.Dispatch<React.SetStateAction<number>>;
}

export default function Card({ card, setCardsPressed, setCardsChoosen } : CardComponentProps) {
    const [isActive, setIsActive] = useState<boolean>(false);

    const handleOnCardPressed = () => {
        if(!isActive) {
           setCardsPressed(prev => prev.add(card.cardID)); 
           setCardsChoosen(prev => prev + 1);
        } else {
            // handle removed
           setCardsPressed(prev => prev.delete(card.cardID)); 
           setCardsChoosen(prev => prev -1);
        }
        setIsActive(!isActive);
    }
    
    return (
        <Pressable style={styles.container} onPress={handleOnCardPressed}>
            <View style={{...styles.card, backgroundColor: isActive ? Colors.Green : Colors.Black}}>
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