namespace CardEntity;

public interface ICardService
{
    /// <summary>
    /// Creates a new card.
    /// </summary>
    /// <param name="playerId">The player creating the new card.</param>
    /// <param name="cardDto">Dto containing the card object and a image file.</param>
    /// <returns>The id of the card created.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the gallery does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the gallery.</exception>
    /// <exception cref="InvalidOperationException">Throws if there is no image file present in the dto.</exception>
    public Task<int> CreateCard(int playerId, CardInputDto cardDto);

    /// <summary>
    /// Deletes a existing card.
    /// </summary>
    /// <param name="playerId">The player deleting the card.</param>
    /// <param name="cardId">The card to delete.</param>
    /// <exception cref="KeyNotFoundException">Throws if the card or gallery does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the gallery.</exception>
    public Task DeleteCard(int playerId, int cardId);

    /// <summary>
    /// Updates a existing card.
    /// </summary>
    /// <param name="playerId">The player updating the card.</param>
    /// <param name="newCardDto">Dto containing the card object and a image file.</param>
    /// <exception cref="KeyNotFoundException">Throws if the card or gallery does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the gallery.</exception>
    /// <exception cref="InvalidOperationException">Throws if there is no image file present in the dto.</exception>
    //public Task UpdateCard(int playerId, CardInputDto newCardDto);

    /// <summary>
    /// Retrives all cards from a gallery.
    /// </summary>
    /// <param name="playerId">The player retreiving the cards.</param>
    /// <param name="galleryId">The gallery the cards are stored in.</param>
    /// <returns>A collection of cards.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the gallery does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the gallery.</exception>
    public Task<IEnumerable<Card>> GetAllCards(int playerId, int galleryId);
}

