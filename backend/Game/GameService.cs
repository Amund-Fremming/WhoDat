using System;

namespace GameEntity;

public class GameService(GameRepository gameRepository) : IGameService
{
    public readonly GameRepository _gameRepository = gameRepository;
}