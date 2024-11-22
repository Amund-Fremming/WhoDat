using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public interface ICardRepository
{
    /// <summary>
    /// Get a Card corresponding to the given id.
    /// </summary>
    /// <param name="cardId">The id for the Card.</param>
    /// <returns>The Card asked for.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the Card does not exist.</exception>
    Task<Result<CardEntity>> GetCardById(int cardId);

    /// <summary>
    /// Stores a new Card to the database.
    /// </summary>
    /// <param name="card">The card to be stored.</param>
    Task<Result<int>> CreateCard(CardEntity card);

    /// <summary>
    /// Deletes a Card from the database.
    /// </summary>
    /// <param name="card">The card to be deleted</param>
    Task<Result> DeleteCard(CardEntity card);

    /// <summary>
    /// Fetches all Cards form a specific gallery.
    /// </summary>
    /// <param name="galleryId">The gallery to fetch cards from.</param>
    /// <returns>An Enumerable of cards.</returns>
    Task<Result<IEnumerable<CardEntity>>> GetAllCards(int galleryId);
}