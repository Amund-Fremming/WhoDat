using Microsoft.AspNetCore.Mvc;
using CardEntity;
using System;

namespace CardEntity;

[ApiController]
[Route("api/[controller]")]
public class CardController(ICardService cardService) : ControllerBase
{
    public readonly ICardService _cardService = cardService;
}