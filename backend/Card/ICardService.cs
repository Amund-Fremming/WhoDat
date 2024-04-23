namespace CardEntity;

public interface ICardService
{
    public Task<int> CreateCard(int playerId, Card card);
    public Task DeleteCard(int playerId, int cardId);
    public Task UpdateCard(int playerId, Card card);
    public Task<IEnumerable<Card>> GetAllCards(int playerId, int galleryId);
}

