using System.ComponentModel.DataAnnotations;

namespace CardEntity;

public class Card
{
    [Key]
    public int CardID { get; set; }
    public int GalleryID { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
    public bool Active { get; set; }

    public Card() { }
}

