namespace Backend.Features.Shared.Enums;

public enum GameState
{
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