import { useFonts } from "expo-font";
import { Stack } from "expo-router";
import { useEffect, useState } from "react";
import "react-native-reanimated";
import { Splash } from "@/src/Splash/Splash";
import { AuthProvider } from "@/src/shared/state/AuthProvider";

export default function RootLayout() {
  const [loadSplash, setLoadSplash] = useState<boolean>(true);
  const [loaded] = useFonts({
    SpaceMono: require("../assets/fonts/SpaceMono-Regular.ttf"),
    Modak: require("../assets/fonts/Modak-Regular.ttf"),
    Inika: require("../assets/fonts/Inika-Regular.ttf"),
    InikaBold: require("../assets/fonts/Inika-Bold.ttf"),
  });

  useEffect(() => {
    if (loaded) {
      const timer = setTimeout(() => {
        setLoadSplash(false);
      }, 300);

      return () => clearTimeout(timer);
    }
  }, [loaded]);

  if (loadSplash) {
    return <Splash />;
  }

  if (loaded) {
    return (
      <AuthProvider>
        <Stack>
          <Stack.Screen name="(tabs)" options={{ headerShown: false }} />
        </Stack>
      </AuthProvider>
    );
  }
}
