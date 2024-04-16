namespace BoardEntity
{

    public class BoardCardUpdateDto
    {
        public IEnumerable<CardUpdateDetails>? Updates { get; set; }
    }

    public class CardUpdateDetails
    {
        public int BoardCardID { get; set; }
        public bool Active { get; set; }
    }
}
