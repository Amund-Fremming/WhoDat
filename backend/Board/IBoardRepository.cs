namespace BoardEntity;

public interface IBoardRepository
{
    /// <summary>
    /// Get a board corresponding to the given id.
    /// </summary>
    /// <param name="boardId">The id for the board.</param>
    /// <returns>The board asked for.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the board does not exist.</exception>
    Task<Board> GetBoardById(int boardId);

    /// <summary>
    /// Stores a new board in the database.
    /// </summary>
    /// <param name="board">Board to be stored.</param>
    /// <returns>The id of the stored board.</returns>
    Task<int> CreateBoard(Board board);

    /// <summary>
    /// Deletes a board from the database.
    /// </summary>
    /// <param name="board">Board to be deleted</param>
    Task DeleteBoard(Board board);

    /// <summary>
    /// Sets the Chosen BoardCard on a board and saves to the database.
    /// </summary>
    /// <param name="board">Board to be set the chosen card on.</param>
    /// <param name="boardCard">The chosen BoardCard.</param>
    Task ChooseBoardCard(Board board, BoardCard boardCard);

    /// <summary>
    /// Updates the number of active BoardCards left.
    /// </summary>
    /// <param name="board">Board to be set cards left on.</param>
    /// <param name="playersLeft">Number of active BoardCards left.</param>
    Task UpdateBoardCardsLeft(Board board, int playersLeft);
}

