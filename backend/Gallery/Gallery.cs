using System.ComponentModel.DataAnnotations;

namespace GalleryEntity;

public class Gallery
{
    [Key]
    public int GalleryID { get; set; }
    public string PlayerID { get; set; }

    public Gallery() { }
}
