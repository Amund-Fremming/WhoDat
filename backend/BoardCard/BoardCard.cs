using System.ComponentModel.DataAnnotations;

namespace BoardCardEntity;

public class BoardCard
{
    [Key]
    public int BoardCardID { get; set; }
    public int BoardID { get; set; }
    public int CardID { get; set; }

    public BoardCard() { }
}
