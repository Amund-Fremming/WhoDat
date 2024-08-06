export interface IRegistrationRequest {
  username: string;
  password: string;
}

export interface ILoginRequest {
  username: string;
  password: string;
}

export interface IAuthResponse {
  playerID: string;
  username: string;
  token: string;
}
