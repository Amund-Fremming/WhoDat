namespace GalleryEntity;

public interface IGalleryService
{
    /// <summary>
    /// Creates a new gallery.
    /// </summary>
    /// <param name="playerId">The player creating the gallery.</param>
    /// <param name="gallery">The new gallery</param>
    /// <returns>The id of the new gallery.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the player does not exist.</exception>
    public Task<int> CreateGallery(int playerId, Gallery gallery);
    
    /// <summary>
    /// Deletes a existin gallery.
    /// </summary>
    /// <param name="playerId">The player deleting the gallery.</param>
    /// <param name="galleryId">The gallery to delete.</param>
    /// <exception cref="KeyNotFoundException">Throws if the player or gallery does not exist.</exception>
    /// <exception cref="UnauthorizedAccessException">Throws if the player id does not exist in the gallery.</exception>
    public Task DeleteGallery(int playerId, int galleryId);
}


