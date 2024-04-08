using Data;

namespace GalleryEntity;

public class GalleryRepository(AppDbContext context, ILogger<GalleryRepository> logger)
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<GalleryRepository> _logger = logger;

    public async Task<Gallery?> GetGalleryById(int galleryId)
    {
        return await _context.Gallery
            .FindAsync(galleryId);
    }

    public async Task<int> CreateGallery(Gallery gallery)
    {
        try
        {
            await _context.Gallery.AddAsync(gallery);

            await _context.SaveChangesAsync();
            return gallery.GalleryID;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Gallery with id {gallery.GalleryID} .(GalleryRepository)");
            return -1;
        }
    }

    public async Task<bool> DeleteGallery(Gallery gallery)
    {
        try
        {
            _context.Remove(gallery);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting Gallery with id {gallery.GalleryID} .(GalleryRepository)");
            return false;
        }
    }

}
