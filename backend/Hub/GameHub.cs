using GameEntity;
using BoardEntity;
using MessageEntity;
using BoardCardEntity;
using Enum;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Hubs;

public class GameHub : Hub
{
    public readonly ILogger<GameHub> _logger;
    public readonly IGameService _gameService;
    public readonly IBoardService _boardService;
    public readonly IBoardCardService _boardCardService;
    public readonly IMessageService _messageService;

    private readonly string IDENTIFIER = "RECEIVE_STATE";

    public GameHub(ILogger<GameHub> logger, IGameService gameService, IBoardService boardService, IBoardCardService boardCardService, IMessageService messageService)
    {
        _logger = logger;
        _gameService = gameService;
        _boardService = boardService;
        _boardCardService = boardCardService;
        _messageService = messageService;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            int recentGamePlayedId = await _gameService.GetRecentGamePlayed(playerId);
            string groupName = recentGamePlayedId.ToString();

            await Clients.Group(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, State.DISCONNECTED);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        catch (Exception e)
        {
            _logger.LogError($"Disconnect failed (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.CONNECT_FAILED);
        }
    }

    public async Task LeaveGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            await _gameService.LeaveGameById(playerId, gameId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, State.FINISHED);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public async Task JoinGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            await _gameService.JoinGameById(playerId, gameId);
            await Clients.Group(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, State.READY);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed JoinGame (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found JoinGame (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access JoinGame (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public async Task UpdateGameState(int gameId, State state)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            await _gameService.UpdateGameState(playerId, gameId, state);
            await Clients.Groups(groupName).SendAsync("ReceiveMessage", GameHubType.SYSTEM, state);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed UpdateGameState (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found UpdateGameState (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access UpdateGameState (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error UpdateGameState (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public async Task UpdateCurrentPlayerTurn(int gameId, int playerNumber)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            await _gameService.UpdateCurrentPlayerTurn(playerId, gameId, playerNumber);
            await Clients.Groups(groupName).SendAsync("ReceiveMessage", GameHubType.SYSTEM, playerNumber);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed UpdateCurrentPlayerTurn (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found UpdateCurrentPlayerTurn (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access UpdateCurrentPlayerTurn (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error UpdateCurrentPlayerTurn (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public async Task SendMessage(int gameId, string messageText)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            await _messageService.CreateMessage(playerId, gameId, messageText);
            await Clients.Groups(groupName).SendAsync("ReceiveMessage", GameHubType.SYSTEM, messageText);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed SendMessage (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found SendMessage (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access SendMessage (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error SendMessage (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public int ParsePlayerIdClaim()
    {
        return int.Parse(Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
    }
}
