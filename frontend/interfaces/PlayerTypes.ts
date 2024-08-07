export interface IPlayer {
  playerID?: number;
  username: string;
  imageUrl?: string;
  passwordHash: string;
  passwordSalt: string;
  role?: Role;
  gallery?: IGallery;
  boards?: Array<IBoard>;
  messages?: Array<IMessage>;
  gamesAsPlayerOne?: Array<IGame>;
  gamesAsPlayerTwo?: Array<IGame>;
}

enum Role {
  USER,
  ADMIN,
}

export interface IPlayerDto {
  playerID?: number;
  username: string;
  password: string;
  imageUrl?: string;
}
