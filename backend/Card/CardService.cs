using System;

namespace CardEntity;

public class CardService(CardRepository cardRepository) : ICardService
{
    public readonly CardRepository _cardRepository = cardRepository;

    public Task<Card> GetCardById(int cardId)
    {

    }

    public Task<int> CreateCard(Card card)
    {

    }

    public Task DeleteCard(int cardId)
    {

    }

    public Task UpdateCard(Card card)
    {

    }

    public Task UpdateActive(bool active)
    {

    }

    public IEnumerable<Card> GetAllCards()
    {

    }

}
