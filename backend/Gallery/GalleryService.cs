using System;

namespace GalleryEntity;

public class GalleryService(GalleryRepository galleryRepository) : IGalleryService
{
    public readonly GalleryRepository _galleryRepository = galleryRepository;

    public Task<Gallery> GetGalleryById(int boardId)
    {

    }

    public Task<int> CreateGallery(Gallery gallery)
    {

    }

    public Task DeleteGallery(int galleryId)
    {

    }

}
