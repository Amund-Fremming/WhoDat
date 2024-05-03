namespace GalleryEntity;

public interface IGalleryService
{
    public Task<int> CreateGallery(int playerId, Gallery gallery);
    public Task DeleteGallery(int playerId, int galleryId);
}


