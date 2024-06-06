namespace BoardCardEntity;

public interface IBoardCardRepository
{
    /// <summary>
    /// Get a BoardCard corresponding to the given id.
    /// </summary>
    /// <param name="boardCardId">The id for the BoardCard.</param>
    /// <returns>The BoardCard asked for.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the BoardCard does not exist.</exception>
    Task<BoardCard> GetBoardCardById(int boardCardId);

    /// <summary>
    /// Stores a enumerable of BoardCards in a batch to the database.
    /// </summary>
    /// <param name="boardCards"></param>
    Task CreateBoardCards(IEnumerable<BoardCard> boardCards);

    /// <summary>
    /// Uses a map to set boardcards activity, then stores in the database.
    /// </summary>
    /// <param name="updateMap">The id of the card that points to the BoardCards activity to be updated to.</param>
    /// <param name="boardCards">The BoardCards to be updated from a board.</param>
    Task UpdateBoardCardsActivity(IDictionary<int, bool> updateMap, IEnumerable<BoardCard> boardCards);

    /// <summary>
    /// Retrieves all the BoardCards from a given board.
    /// </summary>
    /// <param name="boardId">Id of the board to get cards from.</param>
    Task<IList<BoardCard>> GetBoardCardsFromBoard(int boardId);
}
