namespace PlayerEntityTest;

public class PlayerServiceTest
{
    public readonly Mock<ILogger<IPlayerService>> _mocklogger;
    public readonly Mock<IPlayerRepository> _mockPlayerRepository;
    public readonly Mock<IPasswordHasher<Player>> _mockPasswordHasher;
    public readonly IPlayerService _playerService;
    public readonly AppDbContext _context;

    public PlayerServiceTest()
    {
        _mocklogger = new Mock<ILogger<IPlayerService>>();
        _mockPlayerRepository = new Mock<IPlayerRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher<Player>>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new AppDbContext(options);

        _playerService = new PlayerService(
                _mocklogger.Object,
                _mockPlayerRepository.Object,
                _mockPasswordHasher.Object
                );
    }

    [Fact]
    public async Task CreatePlayer_Successful()
    {
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.DoesUsernameExist(player.Username))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mockPlayerRepository.Setup(repo => repo.CreatePlayer(player))
            .ReturnsAsync(player.PlayerID);

        int result = await _playerService.CreatePlayer(player);

        Assert.Equal(player.PlayerID, result);
        _mockPlayerRepository.Verify(repo => repo.CreatePlayer(player), Times.Once);
    }

    [Fact]
    public async Task CreatePlayer_UsernameNotAvailable_ShouldThrow()
    {
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.DoesUsernameExist(player.Username))
            .ThrowsAsync(new ArgumentException($"Username {player.Username} already exists!"));

        await Assert.ThrowsAsync<ArgumentException>(() => _playerService.CreatePlayer(player));
    }

    ///

    [Fact]
    public async Task DeletePlayer_Successful_PlayerExists()
    {
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(player.PlayerID))
            .ReturnsAsync(player);

        _mockPlayerRepository.Setup(repo => repo.DeletePlayer(player))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _playerService.DeletePlayer(player.PlayerID);
        _mockPlayerRepository.Verify(repo => repo.DeletePlayer(player), Times.Once);
    }

    [Fact]
    public async Task DeletePlayer_PlayerDoesNotExists_ShouldThrow()
    {
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(player.PlayerID))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {player.PlayerID}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _playerService.DeletePlayer(player.PlayerID));
    }

    ///

    [Fact]
    public async Task UpdateUsername_Successful()
    {
        string newUsername = "NewUsername";
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.DoesUsernameExist(player.Username))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(player.PlayerID))
            .ReturnsAsync(player);

        _mockPlayerRepository.Setup(repo => repo.UpdateUsername(player, newUsername))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _playerService.UpdateUsername(player.PlayerID, newUsername);

        _mockPlayerRepository.Verify(repo => repo.UpdateUsername(player, newUsername), Times.Once);
    }

    [Fact]
    public async Task UpdateUsername_PlayerDoesNotExist_ShouldThrow()
    {
        string newUsername = "NewUsername";
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.DoesUsernameExist(newUsername))
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(player.PlayerID))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {player.PlayerID}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _playerService.UpdateUsername(player.PlayerID, newUsername));
    }

    [Fact]
    public async Task UpdateUsername_UsernameExists_ShouldThrow()
    {
        string newUsername = "NewUsername";
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(player.PlayerID))
            .ReturnsAsync(player);

        _mockPlayerRepository.Setup(repo => repo.DoesUsernameExist(newUsername))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {player.PlayerID}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _playerService.UpdateUsername(player.PlayerID, newUsername));
    }

    ///

    [Fact]
    public async Task UpdatePassword_Successful()
    {
        string newPassword = "NewPassword";
        string hashedPassword = "irntianrios";
        string newSalt = "arstoi";
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(player.PlayerID))
            .ReturnsAsync(player);

        _mockPasswordHasher.Setup(pass => pass.HashPassword(player, player.PasswordHash))
            .Returns(hashedPassword);

        _mockPlayerRepository.Setup(repo => repo.UpdatePassword(player, hashedPassword, newSalt))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _playerService.UpdatePassword(player.PlayerID, newPassword);

        _mockPlayerRepository.Verify(repo => repo.UpdatePassword(It.IsAny<Player>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePassword_PlayerDoesNotExist_ShouldThrow()
    {
        string newPassword = "NewPassword";
        Player player = new Player("Username", "PasswordHash", "PasswordSalt", Role.USER);
        _mockPlayerRepository.Setup(repo => repo.GetPlayerById(player.PlayerID))
            .ThrowsAsync(new KeyNotFoundException($"Player with id {player.PlayerID}, does not exist!"));

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _playerService.UpdatePassword(player.PlayerID, newPassword));
    }
}
