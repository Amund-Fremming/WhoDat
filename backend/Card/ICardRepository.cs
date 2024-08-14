namespace CardEntity;

public interface ICardRepository
{
    /// <summary>
    /// Get a Card corresponding to the given id.
    /// </summary>
    /// <param name="cardId">The id for the Card.</param>
    /// <returns>The Card asked for.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the Card does not exist.</exception>
    Task<Card> GetCardById(int cardId);

    /// <summary>
    /// Stores a new Card to the database.
    /// </summary>
    /// <param name="card">The card to be stored.</param>
    Task<int> CreateCard(Card card);

    /// <summary>
    /// Deletes a Card from the database.
    /// </summary>
    /// <param name="card">The card to be deleted</param>
    Task DeleteCard(Card card);

    /// <summary>
    /// Updates a card with new values and stores it in the database.
    /// </summary>
    /// <param name="oldCard">The old card with old values.</param>
    /// <param name="newCard">The new card with the new values we are going to set.</param>
    Task UpdateCard(Card oldCard, Card newCard);

    /// <summary>
    /// Fetches all Cards form a specific gallery.
    /// </summary>
    /// <param name="galleryId">The gallery to fetch cards from.</param>
    /// <returns>An Enumerable of cards.</returns>
    Task<IEnumerable<Card>> GetAllCards(int galleryId);

    /// <summary>
    /// Fetches all Cards that a user has, from across all galleries 
    /// </summary>
    /// <param name="playerId">The player to fetch cards from.</param>
    /// <returns>An Enumerable of cards.</returns>
    Task<IEnumerable<Card>> GetAllCardsFromAllGalleries(int playerId);
}
