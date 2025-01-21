export interface IRegistrationRequest {
  username: string;
  password: string;
}

export interface ILoginRequest {
  username: string;
  password: string;
}

export interface IAuthResponse {
  playerID: number;
  username: string;
  token: string;
  imageUrl: string;
}
