namespace RaptorProject.Features.CardEntity;

public class CardInputDto
{
    //[StringLength(15, MinimumLength = 3)]
    //[RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public string? Name { get; set; }

    public IFormFile? Image { get; set; }
}