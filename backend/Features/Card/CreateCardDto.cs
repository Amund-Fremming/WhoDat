namespace Backend.Features.Card;

public record CreateCardDto(string? Name = "", IFormFile? Image = null);