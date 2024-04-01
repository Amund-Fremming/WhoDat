namespace GalleryEntity;

public interface IGalleryService
{
    public Task<Gallery> GetGalleryById(int boardId);
    public Task<int> CreateGallery(Gallery gallery);
    public Task DeleteGallery(int galleryId);
}


