namespace GalleryEntity;

public interface IGalleryService
{
    /* Basic CRUD */
    public Task<Gallery?> GetGalleryById(int galleryId);
    public Task<int> CreateGallery(Gallery gallery);
    public Task<bool> DeleteGallery(int galleryId);
}


