using System;

namespace GalleryEntity;

public class GalleryService(GalleryRepository galleryRepository) : IGalleryService
{
    public readonly GalleryRepository _galleryRepository = galleryRepository;

    public Task<Gallery?> GetGalleryById(int galleryId)
    {

    }

    public Task<int> CreateGallery(Gallery gallery)
    {

    }

    public Task<bool> DeleteGallery(int galleryId)
    {

    }

}
