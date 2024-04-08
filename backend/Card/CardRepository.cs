using Data;
using Microsoft.EntityFrameworkCore;

namespace CardEntity;

public class CardRepository(AppDbContext context, ILogger<CardRepository> logger)
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<CardRepository> _logger = logger;

    public async Task<Card?> GetCardById(int cardId)
    {
        return await _context.Card
            .FindAsync(cardId);
    }

    public async Task<int> CreateCard(Card card)
    {
        try
        {
            await _context.Card.AddAsync(card);
            await _context.SaveChangesAsync();

            return card.CardID;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error creating Card with id {card.CardID} .(CardRepository)");
            return -1;
        }
    }

    public async Task<bool> DeleteCard(Card card)
    {
        try
        {
            _context.Remove(card);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting Card with id {card.CardID} .(CardRepository)");
            return false;
        }
    }

    public async Task<bool> UpdateCard(Card oldCard, Card newCard)
    {
        try
        {
            oldCard.GalleryID = newCard.GalleryID;
            oldCard.Gallery = newCard.Gallery;
            oldCard.Name = newCard.Name;
            oldCard.Url = newCard.Url;

            _context.Card.Update(oldCard);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating Card with id {oldCard.CardID} .(CardRepository)");
            return false;
        }
    }

    public async Task<IEnumerable<Card>> GetAllCards()
    {
        return await _context.Card
            .ToListAsync();
    }
}
