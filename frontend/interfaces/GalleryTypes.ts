import { IPlayer } from "./PlayerTypes";

export interface IGallery {
  galleryID: number;
  name: string;
  playerID: number;
  player?: IPlayer;
  cards: Array<ICard>;
}

export interface ICardInputDto {
  galleryID: number;
  card: ICard;
  image: File;
}

export interface ICard {
  cardId: number;
  galleryID: number;
  name: string;
  url: string;
}
