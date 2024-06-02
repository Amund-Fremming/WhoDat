namespace BoardCardEntity;

public interface IBoardCardRepository
{
    Task<BoardCard> GetBoardCardById(int boardCardId);

    Task CreateBoardCards(IEnumerable<BoardCard> boardCards);

    Task UpdateBoardCardsActivity(IDictionary<int, bool> updateMap, IEnumerable<BoardCard> boardCards);

    Task<IList<BoardCard>> GetBoardCardsFromBoard(int boardId);
}
