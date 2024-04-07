using System;
using Data;

namespace CardEntity;

public class CardRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;

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

    public IEnumerable<Card> GetAllCards()
    {
    }
}
