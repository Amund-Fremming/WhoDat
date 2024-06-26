namespace GalleryEntity;

public class GalleryService(AppDbContext context, ILogger<IGalleryService> logger, IGalleryRepository galleryRepository, IPlayerRepository playerRepository) : IGalleryService
{
    public readonly AppDbContext _context = context;
    public readonly ILogger<IGalleryService> _logger = logger;
    public readonly IGalleryRepository _galleryRepository = galleryRepository;
    public readonly IPlayerRepository _playerRepository = playerRepository;

    public async Task<int> CreateGallery(int playerId, Gallery gallery)
    {
        if (gallery == null)
            throw new ArgumentNullException(nameof(gallery));

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await _playerRepository.GetPlayerById(playerId);
                gallery.PlayerID = playerId;

                int galleryId = await _galleryRepository.CreateGallery(gallery);

                await transaction.CommitAsync();
                return galleryId;
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while creating Gallery for player {playerId} (GalleryService)");
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
                Player player = await _playerRepository.GetPlayerById(playerId);
                Gallery gallery = await _galleryRepository.GetGalleryById(galleryId);
                PlayerHasPermission(player, gallery);

                await _galleryRepository.DeleteGallery(gallery);
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                // ADD HANDLING
                _logger.LogError(e.Message, $"Error while deleting Gallery with id {galleryId}. (GalleryService)");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public void PlayerHasPermission(Player player, Gallery gallery)
    {
        if (player.Role != Role.ADMIN && gallery.PlayerID != player.PlayerID)
        {
            _logger.LogInformation($"Player with id {player.PlayerID} tried accessing someone elses data");
            throw new UnauthorizedAccessException($"Player with id {player.PlayerID} does not have permission");
        }
    }
}
