using System;

namespace CardEntity;

public interface ICardService
{
    /* Basic CRUD */
    public Task<Card> GetCardById(int cardId);
    public Task<int> CreateCard(Card card);
    public Task DeleteCard(int cardId);
    public Task UpdateCard(Card card);

    /* Other */
    public Task UpdateActive(bool active);
    public IEnumerable<Card> GetAllCards();
}

