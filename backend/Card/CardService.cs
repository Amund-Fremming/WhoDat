namespace CardEntity;

public class CardService(CardRepository cardRepository) : ICardService
{
    public readonly CardRepository _cardRepository = cardRepository;

    public Task<int> CreateCard(Card card)
    {

    }

    public Task<bool> DeleteCard(int cardId)
    {

    }

    public Task<bool> UpdateCard(Card card)
    {

    }

    public Task<bool> UpdateActive(bool active)
    {

    }

    public IEnumerable<Card> GetAllCards()
    {

    }

}
