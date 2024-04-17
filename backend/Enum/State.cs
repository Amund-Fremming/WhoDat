namespace Enum;

public enum State
{
    // Initial state
    CREATING,
    WAITING_FOR_PLAYER,         // Kanskje droppe for å la host velge kort?

    // Chosing cards
    ONLY_HOST_CHOSING_CARDS,
    BOTH_CHOSING_CARDS,
    CHOSING_CARDS,

    // Picking player
    BOTH_PICKING_PLAYER,
    PICKING_PLAYER,

    // Turns
    TURN_STARTED,
    ASKING,
    WAITING_ASK_REPLY,
    ASK_REPLY,
    GUESSING,
    WAITING_GUESS_REPLY,
    GUESS_REPLY,
    TOGGLING_CARDS,

    // Finished
    P1_WON,
    P2_WON,

    // Someone left
    FINISHED,
}
