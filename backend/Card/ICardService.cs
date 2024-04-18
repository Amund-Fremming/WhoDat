namespace CardEntity;

public interface ICardService
{
    public Task<int> CreateCard(Card card);
    public Task DeleteCard(int cardId);
    public Task UpdateCard(Card card);
    public Task<IEnumerable<Card>> GetAllCards();
}

