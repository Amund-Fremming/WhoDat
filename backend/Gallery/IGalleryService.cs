namespace GalleryEntity;

public interface IGalleryService
{
    public Task<int> CreateGallery(Gallery gallery);
    public Task DeleteGallery(int galleryId);
}


