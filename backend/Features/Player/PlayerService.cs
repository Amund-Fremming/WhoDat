using Backend.Features.Card;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Player
{
    public class PlayerService(ILogger<PlayerService> logger, IPlayerRepository playerRepository, IPasswordHasher<PlayerEntity> passwordHasher, IImageClient imageClient) : IPlayerService
    {
        private readonly ILogger _logger = logger;
        private readonly IPlayerRepository _playerRepository = playerRepository;
        private readonly IPasswordHasher<PlayerEntity> _passwordHasher = passwordHasher;
        private readonly IImageClient _imageClient = imageClient;

        public async Task<Result<PlayerDto>> Update(PlayerDto playerDto)
        {
            try
            {
                var result = await _playerRepository.GetById(playerDto.PlayerID);
                if (result.IsError)
                    return result.Error;

                var player = result.Data;

                if (playerDto.Username != "" && playerDto.Username != player.Username)
                {
                    var usernameResult = await _playerRepository.UsernameExist(playerDto.Username);
                    if (usernameResult.IsError)
                        return usernameResult.Error;

                    player.Username = playerDto.Username;
                }

                if (playerDto.Password != "")
                {
                    player = UpdatePassword(player, playerDto.Password);
                }

                await _playerRepository.Update(player);
                return new PlayerDto(player.ID, player.Username, string.Empty, player.ImageUrl);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "(PlayerService)");
                return new Error(e, "Failed to update password.");
            }
        }

        public async Task<Result> UpdateImage(int playerId, FormFile formFile)
        {
            var playerResult = await _playerRepository.GetById(playerId);

            if (playerResult.IsError)
                return playerResult.Error;

            var player = playerResult.Data;
            var result = await _imageClient.Upload(formFile);
            if (result.IsError)
                return result.Error;

            player.ImageUrl = result.Data;
            await _playerRepository.Update(player);
            return Result.Ok();
        }

        private PlayerEntity UpdatePassword(PlayerEntity player, string newPassword)
        {
            var salt = GenerateSalt();
            var saltedPassword = newPassword + salt;
            var hashedPassword = _passwordHasher.HashPassword(null!, saltedPassword);
            player.PasswordHash = hashedPassword;
            player.PasswordSalt = salt;

            return player;
        }

        public string GenerateSalt()
        {
            var buffer = new byte[16];
            RandomNumberGenerator.Fill(buffer);

            return Convert.ToBase64String(buffer);
        }
    }
}