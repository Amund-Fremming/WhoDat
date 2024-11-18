namespace Backend.Features.Card;

public class CreateCardDto
{
    public string? Name { get; set; }

    public IFormFile? Image { get; set; }
}