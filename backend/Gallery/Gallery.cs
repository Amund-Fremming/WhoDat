namespace GalleryEntity;

public class Gallery
{
    [Key]
    [Range(1, 10000000)]
    public int GalleryID { get; set; }

    [StringLength(15, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string? Name { get; set; }

    [Range(1, 10000000)]
    public int PlayerID { get; set; }
    public Player? Player { get; set; }

    public IEnumerable<Card>? Cards { get; set; }

    public Gallery() { }

    public Gallery(int playerId, string name)
    {
        PlayerID = playerId;
        Name = name;
    }
}
