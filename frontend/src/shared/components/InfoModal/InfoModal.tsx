import { Modal, Text, View } from 'react-native';
import { styles } from './InfoModalStyles';
import MediumButton from '../MediumButton/MediumButton';
import { Colors } from '../../assets/constants/Colors';

interface IInfoModal {
  message: string;
  isError: boolean;
  modalVisible: boolean;
  setModalVisible: (condition: boolean) => void;
}

export default function InfoModal({
  modalVisible,
  setModalVisible,
  message,
  isError,
}: IInfoModal) {
  return (
    <Modal visible={modalVisible} animationType="fade" transparent={true}>
      <View style={styles.container}>
        <View
          style={[
            styles.modal,
            isError ? styles.errorContainer : styles.messageContainer,
          ]}
        >
          <Text style={styles.header}>{isError ? 'Ooops' : 'Hey'}</Text>
          <Text style={styles.message}>{message}</Text>
          <View style={styles.absoluteButton}>
            <MediumButton
              text="close"
              color={Colors.BurgundyRed}
              inverted={false}
              onButtonPress={() => setModalVisible(!modalVisible)}
            />
          </View>
        </View>
      </View>
    </Modal>
  );
}
