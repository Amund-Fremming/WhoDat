using GameEntity;
using BoardEntity;
using MessageEntity;
using BoardCardEntity;
using Enum;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Hubs;

public class GameHub : Hub
{
    public readonly ILogger<GameHub> _logger;
    public readonly IGameService _gameService;
    public readonly IBoardService _boardService;
    public readonly IBoardCardService _boardCardService;
    public readonly IMessageService _messageService;

    private readonly string IDENTIFIER = "RECEIVE_STATE";
    private readonly string MESSAGE_IDENTIFIER = "RECEIVE_STATE";
    private readonly string BOARDCARDS_LEFT_IDENTIFIER = "RECEIVE_STATE";

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
            await Clients.Group(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, State.PLAYER_LEFT);
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

            Game game = await _gameService.JoinGameById(playerId, gameId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            if (game.State == State.ONLY_HOST_CHOSING_CARDS)
                await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, State.ONLY_HOST_CHOSING_CARDS);

            if (game.State == State.BOTH_CHOSING_CARDS)
                await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, State.BOTH_CHOSING_CARDS);
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
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, state);
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

    public async Task SendMessage(int gameId, string messageText)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();
            string encodedMessageText = EncodeForJsAndHtml(messageText);

            await _messageService.CreateMessage(playerId, gameId, messageText);
            await Clients.Groups(groupName).SendAsync(MESSAGE_IDENTIFIER, GameHubType.SYSTEM, messageText);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Leave game failed SendMessage (GameHub): {e}");
            await Clients.Caller.SendAsync(MESSAGE_IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found SendMessage (GameHub): {e}");
            await Clients.Caller.SendAsync(MESSAGE_IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access SendMessage (GameHub): {e}");
            await Clients.Caller.SendAsync(MESSAGE_IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error SendMessage (GameHub): {e}");
            await Clients.Caller.SendAsync(MESSAGE_IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public async Task GuessBoardCard(int gameId, int boardCardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            State newGameState = await _boardService.GuessBoardCard(playerId, gameId, boardCardId);
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, newGameState);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Guessing card failed GuessBoardCard (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found GuessBoardCard (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access GuessBoardCard (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error GuessBoardCard (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public async Task UpdateBoardCardsActivity(int gameId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            int boardCardsLeft = await _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates);
            await Clients.Groups(groupName).SendAsync(BOARDCARDS_LEFT_IDENTIFIER, GameHubType.SYSTEM, boardCardsLeft);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Operation failed UpdateBoardCardsActivity (GameHub): {e}");
            await Clients.Caller.SendAsync(BOARDCARDS_LEFT_IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found UpdateBoardCardsActivity (GameHub): {e}");
            await Clients.Caller.SendAsync(BOARDCARDS_LEFT_IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access UpdateBoardCardsActivity (GameHub): {e}");
            await Clients.Caller.SendAsync(BOARDCARDS_LEFT_IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error UpdateBoardCardsActivity (GameHub): {e}");
            await Clients.Caller.SendAsync(BOARDCARDS_LEFT_IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public async Task CreateBoardCards(int gameId, IEnumerable<int> cardIds)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            State state = await _boardCardService.CreateBoardCards(playerId, gameId, cardIds);
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, state);
        }
        catch (ArgumentException e)
        {
            _logger.LogError($"Argument failed CreateBoardCards (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.WRONG_AMOUNT_CARDS);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError($"Operation failed CreateBoardCards (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.OPERATION_FAILED);
        }
        catch (KeyNotFoundException e)
        {
            _logger.LogError($"Entity not found CreateBoardCards (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.ENTITY_NOT_FOUND);
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogError($"Unauthorized access CreateBoardCards (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNAUTHORIZED);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error CreateBoardCards (GameHub): {e}");
            await Clients.Caller.SendAsync(IDENTIFIER, GameHubType.ERROR, GameHubError.UNEXPECTED_ERROR);
        }
    }

    public int ParsePlayerIdClaim() => int.Parse(Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

    private string EncodeForJsAndHtml(string input) => JavaScriptEncoder.Default.Encode(HtmlEncoder.Default.Encode(input));
}
