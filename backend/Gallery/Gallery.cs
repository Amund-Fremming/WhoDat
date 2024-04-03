using System.ComponentModel.DataAnnotations;
using PlayerEntity;

namespace GalleryEntity;

public class Gallery
{
    [Key]
    public int GalleryID { get; set; }
    public string PlayerID { get; set; }
    public Player? Player { get; set; }

    public Gallery() { }
}
