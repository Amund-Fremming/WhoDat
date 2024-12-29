import { View, Text, Pressable } from "react-native";
import { styles } from "./ChooseCardPageStyles";
import { PlayPages } from "../../GamePages";
import { Ionicons } from "@expo/vector-icons";
import { Colors } from "@/src/Shared/assets/constants/Colors";
import { useEffect, useState } from "react";
import { useAuthProvider } from "@/src/Shared/state/AuthProvider";
import { getAllCards } from "@/src/Shared/functions/CardClient";
import Result from "@/src/Shared/domain/Result";
import { ICardDto } from "@/src/Shared/domain/CardTypes";
import MediumButton from "@/src/Shared/components/MediumButton/MediumButton";
import Card from "./components/Card/Card";

interface CardPageProps {
  handleError: (message: string, redirect: boolean) => void;
  setPage: React.Dispatch<React.SetStateAction<PlayPages>>;
}

export default function ChooseCardPage({
  handleError,
  setPage
}: CardPageProps) {
  const [cardPressed, setCardPressed] = useState<number>();

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Chose a card</Text>
      <Pressable
        style={styles.backIconWrapper}
        onPress={() => setPage(PlayPages.MAIN_PAGE)}
      >
        <Ionicons name="arrow-back" size={50} color={Colors.Cream} />
      </Pressable>
      <View style={styles.creamContainer}>
      <View style={styles.boardContainer}>
      <Text>Choose card</Text>
      </View>
      </View>
    </View>
  );
}
