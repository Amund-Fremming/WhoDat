import { IPlayer } from '@/src/Shared/types/PlayerTypes';
import { IGame } from './GameTypes';

export interface IBoard {
  id: number;
  PlayerID: number;
  PlayerEntity?: IPlayer;
  gameID: number;
  gameEntity?: IGame;
  chosenCardID?: number;
  chosenCard?: IBoardCard;
  playersLeft: number;
  messages?: Array<IMessage>;
  boardCards?: Array<IBoardCard>;
}

export interface IBoardCardUpdate {
  id: number;
  active: boolean;
}

export interface IBoardCard {
  id: number;
  boardID: number;
  cardID: number;
  card: ICard;
  active: boolean;
}

export interface ICard {
  id: number;
  playerID: number;
  name: string;
  url: string;
}
