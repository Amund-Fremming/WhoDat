import { Colors } from '../Shared/assets/constants/Colors';
import {
  horizontalScale,
  verticalScale,
  moderateScale,
} from '../Shared/assets/constants/Dimentions';
import { StyleSheet } from 'react-native';

export const viewStyles = StyleSheet.create({
  container: {
    height: '110%',
    width: '100%',
    alignItems: 'center',
    backgroundColor: Colors.Orange,
    paddingTop: verticalScale(40),
  },

  creamContainer: {
    width: '100%',
    height: '110%',
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

  buttonWrapper: {
    paddingBottom: verticalScale(30),
    width: '100%',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'row',
    gap: horizontalScale(20),
    position: 'absolute',
    bottom: verticalScale(60),
  },
});

export const textStyles = StyleSheet.create({
  header: {
    fontSize: moderateScale(45),
    fontFamily: 'Modak',
    color: Colors.Cream,
  },
});
