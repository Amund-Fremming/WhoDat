using System;

namespace BoardEntity;

public class BoardService(BoardRepository boardRepository) : IBoardService
{
    public readonly BoardRepository _boardRepository = boardRepository;

    public Task<Board> GetBoardById(int boardId)
    {

    }

    public Task<int> CreateBoard(Board board)
    {

    }

    public Task DeleteBoard(int boardId)
    {

    }

    public Task ChooseCard(int cardId)
    {

    }

    public Task UpdatePlayersLeft(int cardId)
    {

    }
}
