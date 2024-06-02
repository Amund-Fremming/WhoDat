namespace GalleryEntity;

public interface IGalleryRepository
{
    Task<Gallery> GetGalleryById(int galleryId);

    Task<int> CreateGallery(Gallery gallery);

    Task DeleteGallery(Gallery gallery);
}
