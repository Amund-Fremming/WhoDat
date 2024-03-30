using System;
using Data;

namespace GalleryEntity;

public class GalleryRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;
}