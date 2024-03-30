using System;

namespace PlayerEntity;

public class PlayerService(PlayerRepository playerRepository) : IPlayerService
{
    public readonly PlayerRepository _playerRepository = playerRepository;
}