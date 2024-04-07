using System;

namespace BoardCardEntity;

public class BoardCardService(BoardCardRepository boardcardRepository) : IBoardCardService
{
    public readonly BoardCardRepository _boardcardRepository = boardcardRepository;

    public Task<BoardCard> GetBoardCardById(int boardId)
    {

    }

    public Task<int> CreateBoardCard(BoardCard boardCard)
    {

    }

    public Task<bool> DeleteBoardCard(int boardCardId)
    {

    }

    public Task<bool> UpdateActive(bool active)
    {

    }
}
