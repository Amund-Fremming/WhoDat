using System;

namespace CardEntity;

public class CardService(CardRepository cardRepository) : ICardService
{
    public readonly CardRepository _cardRepository = cardRepository;
}