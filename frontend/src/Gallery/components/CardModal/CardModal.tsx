import { Modal, View, Image, Pressable } from 'react-native';
import { styles, imageStyles } from './CardModalStyles';
import BigButton from '@/src/Shared/components/BigButton/BigButton';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import StrokedText from '@/src/Shared/components/StokedText/StrokedText';
import { ICardDto } from '@/src/Shared/types/CardTypes';
import FontAwesome from '@expo/vector-icons/FontAwesome';
import { useEffect, useState } from 'react';
import { MaterialCommunityIcons } from '@expo/vector-icons';

interface CardModalProps {
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
  card: ICardDto;
  onDeleteCardPressed: () => void;
}

export default function CardModal({
  modalVisible,
  setModalVisible,
  card,
  onDeleteCardPressed,
}: CardModalProps) {
  const [editMode, setEditMode] = useState<boolean>(false);

  useEffect(() => {
    setEditMode(false);
  }, [modalVisible]);

  const handleEditCardPressed = () => {
    setEditMode(false);
  };

  return (
    <Modal visible={modalVisible} animationType="fade" transparent={true}>
      <View style={styles.container}>
        <View style={styles.cardModal}>
          <Pressable
            style={styles.closeButton}
            onPress={() => setModalVisible(false)}
          >
            <FontAwesome name="close" size={36} color={Colors.DarkGray} />
          </Pressable>
          <View style={styles.card}>
            {editMode && (
              <Pressable
                style={styles.deleteButton}
                onPress={onDeleteCardPressed}
              >
                <MaterialCommunityIcons
                  name="close-circle"
                  size={45}
                  color={Colors.Red}
                />
              </Pressable>
            )}
            <Image
              style={imageStyles.imageStyle}
              source={{
                uri: card.url,
              }}
            />
          </View>
          <StrokedText text={card.name} fontBaseSize={40} smallBorder={false} />
          <View style={styles.buttonWrapper}>
            <BigButton
              text={editMode ? 'Cancel' : 'Edit'}
              color={Colors.BurgundyRed}
              inverted={editMode}
              onButtonPress={() => setEditMode(!editMode)}
            />
          </View>
        </View>
      </View>
    </Modal>
  );
}
