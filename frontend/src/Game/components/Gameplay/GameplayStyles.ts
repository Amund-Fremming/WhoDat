import { Colors } from '@/src/Shared/assets/constants/Colors';
import { StyleSheet } from 'react-native';
import {
  horizontalScale,
  verticalScale,
  moderateScale,
} from '@/src/Shared/assets/constants/Dimentions';

export const styles = StyleSheet.create({
  container: {
    height: '100%',
    width: '100%',
    alignItems: 'center',
    backgroundColor: Colors.Orange,
    paddingTop: verticalScale(40),
  },

  creamContainer: {
    width: '100%',
    height: '93%',
    alignItems: 'center',
    justifyContent: 'space-between',
    backgroundColor: Colors.Cream,
    borderTopStartRadius: moderateScale(30),
    borderTopRightRadius: moderateScale(30),
  },

  boardContainer: {
    width: '100%',
    marginTop: '5%',
    gap: moderateScale(7),
    flexDirection: 'row',
    flexWrap: 'wrap',
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: verticalScale(30),
  },

  backIconWrapper: {
    color: Colors.Cream,
    position: 'absolute',
    left: horizontalScale(20),
    top: verticalScale(48),
  },

  header: {
    fontFamily: 'Modak',
    textAlign: 'center',
    lineHeight: moderateScale(70),
    fontSize: moderateScale(45),
    color: Colors.Cream,
  },

  header2: {
    fontFamily: 'Modak',
    textAlign: 'center',
    lineHeight: moderateScale(70),
    fontSize: moderateScale(25),
  },
});
