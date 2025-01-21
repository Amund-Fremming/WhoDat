using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public interface ICardService
{
    public Task<Result<CardDto>> CreateCard(int playerId, CreateCardDto cardDto);

    public Task<Result> DeleteCard(int playerId, int cardId);
}