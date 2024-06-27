import { View, Text } from "react-native";
import { useState } from "react";

import { Colors } from "@/constants/Colors";
import styles from "./GalleryStyles";

import BigButton from "@/components/BigButton/BigButton";
import Card from "./components/Card/Card";
import CardModal from "./components/CardModal/CardModal";

export default function Gallery() {
  const [cardModalVisible, setCardModalVisible] = useState<boolean>(false);

  return (
    <>
      <CardModal
        modalVisible={cardModalVisible}
        setModalVisible={setCardModalVisible}
        card={undefined}
      />

      <View style={styles.container}>
        <Text style={styles.header}>Gallery</Text>
        <View style={styles.creamContainer}>
          <View style={styles.boardContainer}>
            {[
              1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
              20,
            ].map((item) => (
              <Card
                key={item}
                card={null}
                onCardPress={() => console.log("Card Pressed!")}
              />
            ))}
          </View>
          <BigButton
            text="Edit"
            color={Colors.BurgundyRed}
            inverted={false}
            onButtonPress={() => setCardModalVisible(true)}
          />
        </View>
      </View>
    </>
  );
}
