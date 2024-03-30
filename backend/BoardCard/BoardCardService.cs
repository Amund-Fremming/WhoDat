using System;

namespace BoardCardEntity;

public class BoardCardService(BoardCardRepository boardcardRepository) : IBoardCardService
{
    public readonly BoardCardRepository _boardcardRepository = boardcardRepository;
}