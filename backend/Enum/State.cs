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

    // Turns from P1
    TURN_STARTED,
    ASKING,
    WAITING_ASK_REPLY,
    REPLY,
    TURN_PLAYED,
    GUESSING,
    WAITING_GUESS_REPLY,

    // Finished
    P1_WON,
    P2_WON,

    // Someone left
    FINISHED,
}
