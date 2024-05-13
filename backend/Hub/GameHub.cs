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
    private readonly string MESSAGE_IDENTIFIER = "RECEIVE_MESSAGE";
    private readonly string BOARDCARDS_LEFT_IDENTIFIER = "RECEIVE_PLAYERS_LEFT";

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
        catch (Exception e)
        {
            await NotifyClientOfError(e, IDENTIFIER, gameId);
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
        catch (Exception e)
        {
            await NotifyClientOfError(e, IDENTIFIER, gameId);
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
        catch (Exception e)
        {
            await NotifyClientOfError(e, IDENTIFIER, gameId);
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
        catch (Exception e)
        {
            await NotifyClientOfError(e, MESSAGE_IDENTIFIER, gameId);
        }
    }

    public async Task GuessBoardCard(int gameId, int boardCardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            State state = await _boardService.GuessBoardCard(playerId, gameId, boardCardId);
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, state);
        }
        catch (Exception e)
        {
            await NotifyClientOfError(e, IDENTIFIER, gameId);
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
        catch (Exception e)
        {
            await NotifyClientOfError(e, BOARDCARDS_LEFT_IDENTIFIER, gameId);
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
        catch (Exception e)
        {
            await NotifyClientOfError(e, IDENTIFIER, gameId);
        }
    }

    public async Task ChooseBoardCard(int gameId, int boardId, int boardCardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            State state = await _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId);
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameHubType.SYSTEM, state);
        }
        catch (Exception e)
        {
            await NotifyClientOfError(e, IDENTIFIER, gameId);
        }
    }

    private async Task NotifyClientOfError(Exception exception, string identifier, int gameId)
    {
        var errorType = GameHubError.UNEXPECTED_ERROR;
        switch (exception)
        {
            case KeyNotFoundException _:
                errorType = GameHubError.ENTITY_NOT_FOUND;
                break;
            case UnauthorizedAccessException _:
                errorType = GameHubError.UNAUTHORIZED;
                break;
            case InvalidOperationException _:
                errorType = GameHubError.OPERATION_FAILED;
                break;
        }

        await Clients.Caller.SendAsync(identifier, GameHubType.ERROR, errorType, "Error occurred in game " + gameId + ". " + exception.Message);
    }

    public int ParsePlayerIdClaim() => int.Parse(Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

    private string EncodeForJsAndHtml(string input) => JavaScriptEncoder.Default.Encode(HtmlEncoder.Default.Encode(input));
}
