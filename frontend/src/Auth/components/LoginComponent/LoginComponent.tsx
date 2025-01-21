import {
  View,
  Text,
  TextInput,
  Pressable,
  KeyboardAvoidingView,
  Platform,
} from 'react-native';
import { styles } from './LoginComponentStyles';
import Feather from '@expo/vector-icons/Feather';
import { Colors } from '@/src/Shared/assets/constants/Colors';
import BigButton from '@/src/Shared/components/BigButton/BigButton';
import { IAuthResponse, ILoginRequest } from '@/src/Auth/AuthTypes';
import { useState } from 'react';
import { useAuthProvider } from '@/src/Shared/providers/AuthProvider';
import { loginPlayer } from '../../AuthClient';
import Result from '@/src/Shared/objects/Result';
import { useInfoModalProvider } from '@/src/Shared/providers/InfoModalProvider';
import { getAllCards } from '@/src/Shared/functions/CardClient';
import { usePreloadProvider } from '@/src/Shared/providers/PreloadProvider';
import { Splash } from '@/src/Splash/Splash';

interface LoginComponentProps {
  setView: React.Dispatch<React.SetStateAction<string>>;
}

export function LoginComponent({ setView }: LoginComponentProps) {
  const [isPreloading, setIsPreloading] = useState<boolean>(false);
  const { setToken, setPlayerID, setUsername, setImageUrl } = useAuthProvider();
  const { toggleInfoModal } = useInfoModalProvider();
  const { setAllGalleryCards } = usePreloadProvider();

  const [loginRequest, setLoginRequest] = useState<ILoginRequest>({
    username: '',
    password: '',
  });

  const handleLogin = async () => {
    /*if (
      loginRequest.password.length <= 0 ||
      loginRequest.username.length <= 0
    ) {
      handleError("Username and password cannot be empty.");
      return;
    }

    if (!validUsername(loginRequest.username)) {
      handleError(
        "Username can only be letters and numbers, and user 9 characters."
      );
      return false;
    }*/

    const result: Result<IAuthResponse> = await loginPlayer(loginRequest);
    if (result.isError) {
      toggleInfoModal(true, result.message);
      return;
    }

    setIsPreloading(true);
    const response: IAuthResponse | null = result.data;
    setToken(response!.token);
    setPlayerID(response!.playerID);
    setUsername(response!.username);
    setImageUrl(response!.imageUrl);

    if (response) {
      var galleryResult = await getAllCards(response.token);
      if (galleryResult.isError) {
        toggleInfoModal(false, galleryResult.message);
        return;
      }

      setAllGalleryCards(galleryResult.data == null ? [] : galleryResult.data);
    }
  };

  if (isPreloading) {
    return <Splash />;
  }

  return (
    <KeyboardAvoidingView
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      style={styles.container}
    >
      <Text style={styles.header}>Login</Text>
      <View style={styles.card}>
        <View style={styles.inputContainer}>
          <View style={styles.iconAndInput}>
            <Feather
              style={styles.icon}
              name="user"
              size={35}
              color={Colors.DarkGray}
            />
            <TextInput
              style={styles.textInput}
              placeholder="Username"
              placeholderTextColor={'gray'}
              onChangeText={(input: string) =>
                setLoginRequest({ ...loginRequest, username: input })
              }
            />
          </View>
          <View style={styles.border}></View>
        </View>
        <View style={styles.inputContainer}>
          <View style={styles.iconAndInput}>
            <Feather
              style={styles.icon}
              name="lock"
              size={35}
              color={Colors.DarkGray}
            />
            <TextInput
              secureTextEntry={true}
              style={styles.textInput}
              placeholder="Password"
              placeholderTextColor={'gray'}
              onChangeText={(input: string) =>
                setLoginRequest({ ...loginRequest, password: input })
              }
            />
          </View>
          <View style={styles.border}></View>
        </View>
        <View style={styles.loginAndRegisterNew}>
          <BigButton
            text="Login"
            color={Colors.BurgundyRed}
            inverted={false}
            onButtonPress={handleLogin}
          />
          <Pressable onPress={() => setView('REGISTER')}>
            <Text style={styles.registerNewText}>Register new player</Text>
          </Pressable>
        </View>
      </View>
    </KeyboardAvoidingView>
  );
}
