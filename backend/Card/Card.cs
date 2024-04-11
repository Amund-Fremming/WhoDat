using System.ComponentModel.DataAnnotations;
using GalleryEntity;
using BoardCardEntity;

namespace CardEntity;

public class Card
{
    [Key]
    public int CardID { get; set; }
    public int GalleryID { get; set; }
    public Gallery? Gallery { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }

    public IEnumerable<BoardCard>? BoardCards { get; set; }

    public Card() { }

    public Card(int galleryId)
    {
        GalleryID = galleryId;
    }
}

