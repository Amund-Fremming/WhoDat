namespace CardEntity;

public class Card
{
    [Key]
    [Range(1, 10000000)]
    public int CardID { get; set; }

    [Range(1, 10000000)]
    public int GalleryID { get; set; }
    public Gallery? Gallery { get; set; }

    [StringLength(15, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string? Name { get; set; }

    [StringLength(1000, MinimumLength = 3)]
    [RegularExpression(@"^(https?://)([\w-]+(\.[\w-]+)+)(/[\w- ,./?%&=]*)?(\.(jpg|jpeg|png|gif))$", ErrorMessage = "Please enter a valid image URL.")]
    public string? Url { get; set; }

    public IEnumerable<BoardCard>? BoardCards { get; set; }

    public Card() { }

    public Card(int galleryId)
    {
        GalleryID = galleryId;
    }
}

