using Data;

namespace GalleryEntity;

public class GalleryService(AppDbContext context, ILogger<GalleryService> logger, GalleryRepository galleryRepository) : IGalleryService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<GalleryService> _logger = logger;
    public readonly GalleryRepository _galleryRepository = galleryRepository;

    public async Task<int> CreateGallery(Gallery gallery)
    {
        try
        {
            return await _galleryRepository.CreateGallery(gallery);
        }
        catch (Exception e)
        {
            // ADD HANDLING
            _logger.LogError(e, $"Error while creating Gallery with id {gallery.GalleryID}. (GalleryService)");
            throw;
        }
    }

    public async Task DeleteGallery(int galleryId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                Gallery gallery = await _galleryRepository.GetGalleryById(galleryId);
                bool deletedGallery = await _galleryRepository.DeleteGallery(gallery);

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e, $"Error while deleting Gallery with id {galleryId}. (GalleryService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
