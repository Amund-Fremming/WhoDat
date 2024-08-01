import { Tabs } from "expo-router";
import React from "react";
import { TabBarIcon } from "@/components/navigation/TabBarIcon";
import { Colors } from "@/constants/Colors";
import Auth from "@/screens/Auth/Auth";

export default function TabLayout() {
    if (true) {
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
