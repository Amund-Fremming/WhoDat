import { useState } from "react";
import { PlayScreens } from "./PlayScreens";
import MainPage from "./components/MainPage/MainPage";

export default function Play() {
    const [screen, setScreen] = useState<PlayScreens>(PlayScreens.MAIN_PAGE);

    switch (screen) {
        case PlayScreens.MAIN_PAGE:
            return <MainPage />;
    }
}
