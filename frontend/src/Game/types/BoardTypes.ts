import { IPlayer } from '@/src/Shared/types/PlayerTypes';
import { IGame } from './GameTypes';
import { ICardDto } from '@/src/Shared/types/CardTypes';

export interface IBoard {
  ID: number;
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
  ID: number;
  active: boolean;
}

export interface IBoardCard {
  ID: number;
  boardID: number;
  cardID: number;
  active: boolean;
}
