using Backend.Features.Shared.Common;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public interface ICardRepository : IRepository<CardEntity>
{
    Task<Result<IEnumerable<CardDto>>> GetAllCards(int galleryId);
}