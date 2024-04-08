namespace CardEntity;

public interface ICardService
{
    /* Basic CRUD */
    public Task<int> CreateCard(Card card);
    public Task<bool> DeleteCard(int cardId);
    public Task<bool> UpdateCard(Card card);

    /* Other */
    public IEnumerable<Card> GetAllCards();
}

