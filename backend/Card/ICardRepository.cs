namespace CardEntity;

public interface ICardRepository
{
    Task<Card> GetCardById(int cardId);

    Task<int> CreateCard(Card card);

    Task DeleteCard(Card card);

    Task UpdateCard(Card oldCard, Card newCard);

    Task<IEnumerable<Card>> GetAllCards(int galleryId);
}
