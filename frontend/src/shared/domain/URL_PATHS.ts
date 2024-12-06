/*
export const AUTH_ENDPOINT: string = "http://localhost:5158/api/auth";
export const CARD_ENDPOINT: string = "http://localhost:5158/api/Card";
export const GAME_ENDPOINT: string = "http://localhost:5158/api/Game";

export const HUB_ENDPOINT: string = "http://localhost:5158/hub";
*/

const BASE =
  "https://prod-whodat-csg5cqckabfue8hh.northeurope-01.azurewebsites.net";

export const AUTH_ENDPOINT: string = `${BASE}/api/auth`;
export const CARD_ENDPOINT: string = `${BASE}/api/Card`;
export const GAME_ENDPOINT: string = `${BASE}/api/Game`;
export const PLAYER_ENDPOINT: string = `${BASE}/api/Player`;

export const HUB_ENDPOINT: string = `${BASE}/hub`;
