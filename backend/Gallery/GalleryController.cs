using Microsoft.AspNetCore.Mvc;
using GalleryEntity;
using System;

namespace GalleryEntity;

[ApiController]
[Route("api/[controller]")]
public class GalleryController(IGalleryService galleryService) : ControllerBase
{
    public readonly IGalleryService _galleryService = galleryService;
}