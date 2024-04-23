using Data;
using PlayerEntity;

namespace GalleryEntity;

public class GalleryService(AppDbContext context, ILogger<GalleryService> logger, GalleryRepository galleryRepository, PlayerRepository playerRepository) : IGalleryService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<GalleryService> _logger = logger;
    public readonly GalleryRepository _galleryRepository = galleryRepository;
    public readonly PlayerRepository _playerRepository = playerRepository;

    public async Task<int> CreateGallery(int playerId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _playerRepository.GetPlayerById(playerId);
                Gallery gallery = new Gallery(playerId);
                int galleryId = await _galleryRepository.CreateGallery(gallery);

                await transaction.CommitAsync();
                return galleryId;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e, $"Error while creating Gallery for player {playerId} (GalleryService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task DeleteGallery(int playerId, int galleryId)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _playerRepository.GetPlayerById(playerId);
                Gallery gallery = await _galleryRepository.GetGalleryById(galleryId);
                PlayerHasPermission(playerId, gallery);

                bool deletedGallery = await _galleryRepository.DeleteGallery(gallery);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e, $"Error while deleting Gallery with id {galleryId}. (GalleryService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public void PlayerHasPermission(int playerId, Gallery gallery)
    {
        if (gallery.PlayerID != playerId)
        {
            _logger.LogInformation($"Player with id {playerId} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {playerId} does not have permission");
        }
    }
}
