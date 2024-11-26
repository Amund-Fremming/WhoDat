export interface IGame {
  gameId?: number;
  playerOneID?: number;
  // player obj
  playerTwoID?: number;
  // player obj
  state: State;
  messages?: Array<IMessage>;
  boards?: Array<IBoard>;
}

export enum State {
  // Chosing cards
  ONLY_HOST_CHOSING_CARDS,
  BOTH_CHOSING_CARDS,
  P1_CHOOSING,
  P2_CHOOSING,

  // Picking player
  BOTH_PICKING_PLAYER,
  P1_PICKING_PLAYER,
  P2_PICKING_PLAYER,
  BOTH_PICKED_PLAYERS,

  // Turns
  P1_TURN_STARTED,
  P1_WAITING_ASK_REPLY,
  P1_ASK_REPLIED,
  P1_WAITING_GUESS_REPLY,
  P1_GUESS_REPLIED,

  P2_TURN_STARTED,
  P2_WAITING_ASK_REPLY,
  P2_ASK_REPLIED,
  P2_WAITING_GUESS_REPLY,
  P2_GUESS_REPLIED,

  // Finished
  P1_WON,
  P2_WON,

  // Someone left
  DISCONNECTED,
  PLAYER_LEFT,
}

export interface IMessage {
  messageID: number;
  gameID: number;
  playerID: number;
  // player obj
  messageText?: string;
}

export interface IBoard {
  boardID: number;
  playerID: number;
  // player obj
  gameID: number;
  // game obj
  chosenCardID?: number;
  // boardcard obj
  playersLeft: number;
  messages?: Array<IMessage>;
  boardCards: Array<IBoardCard>;
}

export interface IBoardCard {
  boardCardID: number;
  boardID: number;
  // board obj
  cardID: number;
  // card obj
  active: boolean;
}
