namespace GalleryEntity;

public interface IGalleryRepository
{
    /// <summary>
    /// Get a Gallery corresponding to the given id.
    /// </summary>
    /// <param name="galleryId">The id for the Gallery.</param>
    /// <returns>The Gallery asked for.</returns>
    /// <exception cref="KeyNotFoundException">Throws if the Gallery does not exist.</exception>
    Task<Gallery> GetGalleryById(int galleryId);

    /// <summary>
    /// Stores a new Gallery to the database.
    /// </summary>
    /// <param name="gallery">The Gallery to be stored.</param>
    Task<int> CreateGallery(Gallery gallery);

    /// <summary>
    /// Deletes a Gallery from the database.
    /// </summary>
    /// <param name="gallery">The Gallery to be deleted</param>
    Task DeleteGallery(Gallery gallery);
}
