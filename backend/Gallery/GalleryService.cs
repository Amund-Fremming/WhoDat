namespace GalleryEntity;

public class GalleryService(ILogger<GalleryService> logger, GalleryRepository galleryRepository) : IGalleryService
{
    public readonly GalleryRepository _galleryRepository = galleryRepository;
    public readonly ILogger<GalleryService> _logger = logger;

    public async Task<int> CreateGallery(Gallery gallery)
    {
        try
        {
            return await _galleryRepository.CreateGallery(gallery);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while creating Gallery with id {gallery.GalleryID}. (GalleryService)");
            throw;
        }
    }

    public async Task<bool> DeleteGallery(int galleryId)
    {
        try
        {
            Gallery gallery = await _galleryRepository.GetGalleryById(galleryId);

            return await _galleryRepository.DeleteGallery(gallery);
        }
        catch (Exception)
        {
            // ADD HANDLING
            _logger.LogError($"Error while deleting Gallery with id {galleryId}. (GalleryService)");
            throw;
        }
    }

}
