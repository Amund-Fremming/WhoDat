using Backend.Features.Shared.Common.Repository;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public interface ICardRepository : IRepository<CardEntity>
{
    /// <summary>
    /// Fetches all Cards form a specific gallery.
    /// </summary>
    /// <param name="galleryId">The gallery to fetch cards from.</param>
    /// <returns>An Enumerable of cards.</returns>
    Task<Result<IEnumerable<CardDto>>> GetAllCards(int galleryId);
}