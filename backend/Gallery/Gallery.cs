using System.ComponentModel.DataAnnotations;

namespace GalleryEntity;

public class Gallery
{
    [Key]
    public int GalleryID { get; set; }
    public int PlayerID { get; set; }

    public Gallery() { }
}
