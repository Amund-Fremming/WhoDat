using System;

namespace GalleryEntity;

public class GalleryService(GalleryRepository galleryRepository) : IGalleryService
{
    public readonly GalleryRepository _galleryRepository = galleryRepository;
}