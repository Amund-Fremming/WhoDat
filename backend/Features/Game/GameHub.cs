using Backend.Features.Board;
using Backend.Features.BoardCard;
using Backend.Features.Message;
using Backend.Features.Shared.Enums;

namespace Backend.Features.Game;

public class GameHub(ILogger<GameHub> logger, IGameService gameService, IBoardService boardService, IBoardCardService boardCardService, IMessageService messageService) : Hub
{
    private readonly ILogger<GameHub> _logger = logger;
    private readonly IGameService _gameService = gameService;
    private readonly IBoardService _boardService = boardService;
    private readonly IBoardCardService _boardCardService = boardCardService;
    private readonly IMessageService _messageService = messageService;

    private const string StateIdentifier = "RECEIVE_STATE";
    private const string MessageIdentifier = "RECEIVE_MESSAGE";
    private const string ErrorIdentifier = "RECEIVE_ERROR";
    private const string BoardCardsLeftIdentifier = "RECEIVE_PLAYERS_LEFT";
    private const string GenericErrorMsg = "Something went wrong.";

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var result = await _gameService.GetRecentGamePlayed(playerId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            var recentGamePlayedId = result.Data;
            var groupName = recentGamePlayedId.ToString();

            await Clients.Group(groupName).SendAsync(StateIdentifier, GameState.DISCONNECTED);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(OnDisconnectAsync)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task LeaveGame(int gameId, bool doBroadcast)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();

            var result = await _gameService.LeaveGameById(playerId, gameId);
            if (result.IsError)
            {
                await Clients.Group(groupName).SendAsync(ErrorIdentifier, result.Message);
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            if (doBroadcast)
            {
                await Clients.Group(groupName).SendAsync(StateIdentifier, GameState.PLAYER_LEFT);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(LeaveGame)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task JoinGame(int gameId)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();

            var result = await _gameService.JoinGameById(playerId, gameId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            var game = result.Data;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Groups(groupName).SendAsync(StateIdentifier, game.GameState);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(JoinGame)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task SubscribeToGameAsHost(int gameId)
    {
        try
        {
            var groupName = gameId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(SubscribeToGameAsHost)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task FinishTurn(int gameId)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();

            var result = await _gameService.FinishTurn(playerId, gameId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(StateIdentifier, state);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateGameState)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task SendMessage(int gameId, string messageText)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();
            var encodedMessageText = EncodeForJsAndHtml(messageText);

            var result = await _messageService.CreateMessage(playerId, gameId, encodedMessageText);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            var gameState = result.Data;
            await Clients.Groups(groupName).SendAsync(StateIdentifier, gameState);
            await Clients.GroupExcept(groupName, Context.ConnectionId).SendAsync(MessageIdentifier, messageText);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(SendMessage)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task GuessBoardCard(int gameId, int boardCardId)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();

            var result = await _boardService.GuessBoardCard(playerId, gameId, boardCardId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(StateIdentifier, state);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GuessBoardCard)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task UpdateBoardCardsActivity(int gameId, int boardId, IEnumerable<int> activeBoardCardIds)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();

            var result = await _boardCardService.UpdateBoardCardsActivity(playerId, boardId, activeBoardCardIds);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            await Clients.GroupExcept(groupName, Context.ConnectionId).SendAsync(BoardCardsLeftIdentifier, activeBoardCardIds.Count());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateBoardCardsActivity)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task CreateBoardCards(int gameId, IEnumerable<int> cardIds)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();

            var result = await _boardCardService.CreateBoardCards(playerId, gameId, cardIds);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(StateIdentifier, state);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(CreateBoardCards)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task ChooseBoardCard(int gameId, int boardId, int boardCardId)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();

            var result = await _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(StateIdentifier, state);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(ChooseBoardCard)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public async Task StartGame(int gameId)
    {
        try
        {
            var playerId = ParsePlayerIdClaim();
            var groupName = gameId.ToString();

            var result = await _gameService.StartGame(playerId, gameId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ErrorIdentifier, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(StateIdentifier, state);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(StartGame)");
            await Clients.Caller.SendAsync(ErrorIdentifier, GenericErrorMsg);
        }
    }

    public int ParsePlayerIdClaim() => int.Parse(Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

    private static string EncodeForJsAndHtml(string input) => JavaScriptEncoder.Default.Encode(HtmlEncoder.Default.Encode(input));
}