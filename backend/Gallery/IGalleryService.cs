namespace GalleryEntity;

public interface IGalleryService
{
    public Task<int> CreateGallery(int playerId);
    public Task DeleteGallery(int playerId, int galleryId);
}


