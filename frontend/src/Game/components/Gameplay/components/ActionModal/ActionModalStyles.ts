import {
  verticalScale,
  moderateScale,
  horizontalScale,
} from '@/src/Shared/assets/constants/Dimentions';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import { StyleSheet } from 'react-native';

export const styles = StyleSheet.create({
  container: {
    backgroundColor: 'rgba(0, 0, 0, 0.5);',
    justifyContent: 'center',
    alignItems: 'center',
    flex: 1,
  },

  modal: {
    alignItems: 'center',
    width: '95%',
    height: verticalScale(240),
    paddingBottom: verticalScale(105),
    backgroundColor: Colors.Cream,
    borderWidth: moderateScale(5),
    borderRadius: moderateScale(30),
    paddingTop: verticalScale(40),
    shadowColor: Colors.Black,
    shadowOpacity: 0.5,
    shadowRadius: 9,
    shadowOffset: {
      width: horizontalScale(15),
      height: verticalScale(15),
    },
    paddingHorizontal: horizontalScale(3),
  },

  absoluteButton: {
    position: 'absolute',
    bottom: verticalScale(20),
  },
});
