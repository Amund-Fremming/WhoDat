using Data;

namespace GalleryEntity;

public class GalleryRepository(AppDbContext context, ILogger<IGalleryRepository> logger) : IGalleryRepository
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IGalleryRepository> _logger = logger;

    public async Task<Gallery> GetGalleryById(int galleryId)
    {
        return await _context.Gallery
            .FindAsync(galleryId) ?? throw new KeyNotFoundException($"Gallery with id {galleryId}, does not exist!");
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
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error creating Gallery with id {gallery.GalleryID} .(GalleryRepository)");
            throw;
        }
    }

    public async Task DeleteGallery(Gallery gallery)
    {
        try
        {
            _context.Remove(gallery);

            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // TODO - more exceptions
            _logger.LogError(e.Message, $"Error deleting Gallery with id {gallery.GalleryID} .(GalleryRepository)");
            throw;
        }
    }
}
