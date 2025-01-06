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
    justifyContent: 'flex-start',
    backgroundColor: Colors.Cream,
    borderTopStartRadius: moderateScale(30),
    borderTopRightRadius: moderateScale(30),
  },

  boardContainer: {
    width: '100%',
    marginTop: '5%',
    rowGap: moderateScale(-2),
    columnGap: moderateScale(8),
    flexDirection: 'row',
    flexWrap: 'wrap',
    alignItems: 'center',
    justifyContent: 'center',
  },

  backIconWrapper: {
    color: Colors.Cream,
    position: 'absolute',
    left: horizontalScale(20),
    top: verticalScale(48),
  },

  header: {
    paddingTop: verticalScale(40),
    fontFamily: 'Modak',
    textAlign: 'center',
    lineHeight: moderateScale(30),
    fontSize: moderateScale(45),
    color: Colors.Cream,
  },

  header2: {
    marginTop: verticalScale(-10),
    fontFamily: 'Modak',
    textAlign: 'center',
    lineHeight: moderateScale(30),
    fontSize: moderateScale(25),
  },

  subHeaderWrapper: {
    flexDirection: 'row',
    gap: horizontalScale(10),
  },

  controlPanel: {
    width: '100%',
    height: '30%',
    borderTopStartRadius: moderateScale(30),
    borderTopRightRadius: moderateScale(30),
    borderColor: Colors.DarkGray,
    borderWidth: moderateScale(4),
    justifyContent: 'space-around',
    alignItems: 'flex-start',
    paddingTop: verticalScale(15),
    flexDirection: 'row',
  },

  controlButtonWrapper: {
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    gap: verticalScale(8),
    height: verticalScale(115),
  },

  chosenCardOuter: {
    height: verticalScale(115),
    width: horizontalScale(80),
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: Colors.BurgundyRed,
    borderRadius: moderateScale(6),
  },
});

export const imageStyles = StyleSheet.create({
  chosenCardInner: {
    height: '91%',
    width: '88%',
    borderRadius: moderateScale(6),
  },
});
