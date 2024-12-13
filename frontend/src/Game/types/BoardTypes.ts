import { IPlayer } from "@/src/Shared/domain/PlayerTypes";
import { IBoardCard, IGame } from "./GameTypes";

export interface IBoard {
  ID: number;
  PlayerID: number;
  Player?: IPlayer;
  gameID: number;
  game?: IGame;
  chosenCardID?: number;
  chosenCard?: IBoardCard;
  playersLeft: number;
  messages?: Array<IMessage>;
  boardCards?: Array<IBoardCard>;
}

export interface IBoardCardUpdate {
  boardCardID: number;
  active: boolean;
}
