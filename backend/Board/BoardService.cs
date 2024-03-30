using System;

namespace BoardEntity;

public class BoardService(BoardRepository boardRepository) : IBoardService
{
    public readonly BoardRepository _boardRepository = boardRepository;
}