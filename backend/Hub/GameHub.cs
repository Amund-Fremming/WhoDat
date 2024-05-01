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

    public GameHub(ILogger<GameHub> logger, IGameService gameService, IBoardService boardService, IBoardCardService boardCardService, IMessageService messageService)
    {
        _logger = logger;
        _gameService = gameService;
        _boardService = boardService;
        _boardCardService = boardCardService;
        _messageService = messageService;
    }

    private IDictionary<string, string> connectionIdToGroup = new Dictionary<string, string>();

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            // TODO
        }
        catch (Exception e)
        {
            _logger.LogError($"Disconnect failed (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "CONNECT_FAILED");
        }
    }

    public async Task LeaveGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            await _gameService.LeaveGameById(playerId, gameId);

            if (connectionIdToGroup.TryGetValue(Context.ConnectionId, out string? groupName))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                await Clients.Group(groupName).SendAsync("ReceiveMessage", "Message", State.FINISHED);
            }
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "OPERATION_FAILED");
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "ENTITY_NOT_FOUND");
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNAUTHORIZED");
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error in LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNEXPECTED_ERROR");
        }
    }


    public async Task JoinGame(int gameId)
    {
        try
        {
            string gameIdString = gameId.ToString();
            int playerId = ParsePlayerIdClaim();

            await _gameService.JoinGameById(playerId, gameId);
            connectionIdToGroup.TryAdd(Context.ConnectionId, gameIdString);
            await Clients.Group(gameIdString).SendAsync("ReceiveMessage", "Message", State.READY);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "OPERATION_FAILED");
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "ENTITY_NOT_FOUND");
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNAUTHORIZED");
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error in LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNEXPECTED_ERROR");
        }
    }

    public async Task UpdateGameState(int gameId, State state)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string gameIdString = gameId.ToString();

            await _gameService.UpdateGameState(playerId, gameId, state);
            await Clients.Groups(gameIdString).SendAsync("ReceiveMessage", "Message", state);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "OPERATION_FAILED");
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "ENTITY_NOT_FOUND");
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNAUTHORIZED");
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error in LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNEXPECTED_ERROR");
        }
    }

    public async Task UpdateCurrentPlayerTurn(int gameId, int playerNumber)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string gameIdString = gameId.ToString();

            await _gameService.UpdateCurrentPlayerTurn(playerId, gameId, playerNumber);
            await Clients.Groups(gameIdString).SendAsync("ReceiveMessage", "Message", playerNumber);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "OPERATION_FAILED");
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "ENTITY_NOT_FOUND");
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNAUTHORIZED");
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error in LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNEXPECTED_ERROR");
        }
    }

    public async Task SendMessage(int gameId, string messageText)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string gameIdString = gameId.ToString();

            await _messageService.CreateMessage(playerId, gameId, messageText);
            await Clients.Groups(gameIdString).SendAsync("ReceiveMessage", "Message", messageText);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "OPERATION_FAILED");
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "ENTITY_NOT_FOUND");
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNAUTHORIZED");
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error in LeaveGame (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "UNEXPECTED_ERROR");
        }
    }

    public int ParsePlayerIdClaim()
    {
        return int.Parse(Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
    }
}
