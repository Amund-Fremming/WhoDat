using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Card;

public interface ICardService
{
    /// <summary>
    /// Creates a new card.
    /// </summary>
    /// <param name="playerId">The player creating the new card.</param>
    /// <param name="cardDto">Dto containing the card object and a image file.</param>
    /// <returns>The id of the card created.</returns>
    public Task<Result> CreateCard(int playerId, CreateCardDto cardDto);

    /// <summary>
    /// Deletes a existing card.
    /// </summary>
    /// <param name="playerId">The player deleting the card.</param>
    /// <param name="cardId">The card to delete.</param>
    public Task<Result> DeleteCard(int playerId, int cardId);

    /// <summary>
    /// Updates a existing card.
    /// </summary>
    /// <param name="playerId">The player updating the card.</param>
    /// <param name="newCardDto">Dto containing the card object and a image file.</param>
    //public Task UpdateCard(int playerId, CardInputDto newCardDto);
}