import { Colors } from '@/src/Shared/assets/constants/Colors';
import {
  horizontalScale,
  moderateScale,
  verticalScale,
} from '@/src/Shared/assets/constants/Dimentions';
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
    minHeight: verticalScale(250),
    paddingBottom: verticalScale(105),
    backgroundColor: Colors.Cream,
    borderWidth: moderateScale(5),
    borderColor: Colors.BorderGray,
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
    justifyContent: 'center',
  },

  closeButton: {
    position: 'absolute',
    zIndex: 1,
    right: horizontalScale(16),
    top: verticalScale(12),
  },

  input: {
    width: '90%',
    textAlign: 'center',
    fontFamily: 'InikaBold',
    fontSize: moderateScale(30),
    height: verticalScale(90),
  },

  absoluteButton: {
    position: 'absolute',
    bottom: verticalScale(20),
    color: Colors.DarkGray,
  },

  border: {
    backgroundColor: Colors.BorderGray,
    height: verticalScale(9),
    width: horizontalScale(300),
    borderRadius: moderateScale(20),
    marginTop: verticalScale(20),
  },

  answeringContainer: {
    justifyContent: 'center',
    alignItems: 'center',
  },

  absoluteDoubleButton: {
    position: 'absolute',
    bottom: verticalScale(20),
    color: Colors.DarkGray,
    flexDirection: 'row',
    columnGap: horizontalScale(15),
  },
});
