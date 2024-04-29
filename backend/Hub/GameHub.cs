using GameEntity;
using BoardEntity;
using MessageEntity;
using BoardCardEntity;
using Enum;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GameHub;

public class GameHub(ILogger<GameController> logger, IGameService gameService, IBoardService boardService, IBoardCardService boardCardService, IMessageService messageService) : Hub
{
    public readonly ILogger<GameController> _logger = logger;
    public readonly IGameService _gameService = gameService;
    public readonly IBoardService _boardService = boardService;
    public readonly IBoardCardService _boardCardService = boardCardService;
    public readonly IMessageService _messageService = messageService;

    private IDictionary<string, List<string>> playerGroups = new Dictionary<string, List<string>>();

    // override onConnect
    // override onDisconnect 

    public override async Task OnConnectedAsync()
    {
        try
        {
        }
        catch (Exception e)
        {
            _logger.LogError($"Connect failed (GameHub): {e}");
            await Clients.Caller.SendAsync("ReceiveMessage", "Error", "CONNECT_FAILED");
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
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
            string gameIdString = gameId.ToString();
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
            string gameIdString = gameId.ToString();
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

    public async Task UptadeCurrentPlayerTurn(int gameId, int playerNumber)
    {
        try
        {
            string gameIdString = gameId.ToString();
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

    public async Task SendMessage(int gameId, string message)
    {
        try
        {
            string gameIdString = gameId.ToString();
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
}
