import { Tabs } from "expo-router";
import React from "react";
import { TabBarIcon } from "@/src/shared/components/navigation/TabBarIcon";
import { Colors } from "@/src/shared/assets/constants/Colors";
import Auth from "@/src/views/Auth/Auth";
import { useEffect, useState } from "react";
import { useAuthProvider } from "@/src/shared/AuthProvider";

export default function TabLayout() {
  const { token } = useAuthProvider();

  const [playerLoggedIn, setPlayerLoggedIn] = useState<boolean>(false);
  useEffect(() => {
    if (token != "") setPlayerLoggedIn(true);
  }, [token]);

  if (!playerLoggedIn) {
    return <Auth />;
  }

  return (
    <Tabs
      screenOptions={{
        headerShown: false,
        tabBarActiveTintColor: Colors.BurgundyRed,
        tabBarStyle: {
          backgroundColor: Colors.Cream,
        },
      }}
    >
      <Tabs.Screen
        name="index"
        options={{
          title: "Play",
          tabBarIcon: ({ color, focused }) => (
            <TabBarIcon
              name={focused ? "game-controller" : "game-controller-outline"}
              color={color}
            />
          ),
        }}
      />
      <Tabs.Screen
        name="gallery"
        options={{
          title: "Gallery",
          tabBarIcon: ({ color, focused }) => (
            <TabBarIcon
              name={focused ? "images" : "images-outline"}
              color={color}
            />
          ),
        }}
      />
      <Tabs.Screen
        name="profile"
        options={{
          title: "Profile",
          tabBarIcon: ({ color, focused }) => (
            <TabBarIcon name={focused ? "man" : "man-outline"} color={color} />
          ),
        }}
      />
    </Tabs>
  );
}
