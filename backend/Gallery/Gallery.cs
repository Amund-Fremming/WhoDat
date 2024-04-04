using System.ComponentModel.DataAnnotations;
using PlayerEntity;
using CardEntity;

namespace GalleryEntity;

public class Gallery
{
    [Key]
    public int GalleryID { get; set; }
    public string PlayerID { get; set; }
    public Player? Player { get; set; }

    public IEnumerable<Card> Cards { get; set; }

    public Gallery() { }
}
