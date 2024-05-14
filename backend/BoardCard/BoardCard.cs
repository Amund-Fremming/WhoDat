using System.ComponentModel.DataAnnotations;
using BoardEntity;
using CardEntity;

namespace BoardCardEntity;

public class BoardCard
{
    [Key]
    public int BoardCardID { get; set; }
    public int BoardID { get; set; }
    public Board? Board { get; set; }
    public int CardID { get; set; }
    public Card? Card { get; set; }
    public bool Active { get; set; }

    public BoardCard() { }

    public BoardCard(int boardID, int cardID)
    {
        BoardID = boardID;
        CardID = cardID;
        Active = true;
    }
}
