using Backend.Features.Board;
using Backend.Features.BoardCard;
using Backend.Features.Message;
using Backend.Features.Shared.Enums;

namespace Backend.Features.Game;

public class GameHubBroker(ILogger<GameHubBroker> logger, IGameService gameService, IBoardService boardService, IBoardCardService boardCardService, IMessageService messageService) : Hub
{
    private readonly ILogger<GameHubBroker> _logger = logger;
    private readonly IGameService _gameService = gameService;
    private readonly IBoardService _boardService = boardService;
    private readonly IBoardCardService _boardCardService = boardCardService;
    private readonly IMessageService _messageService = messageService;

    private readonly string IDENTIFIER = "RECEIVE_STATE";
    private readonly string MESSAGE_IDENTIFIER = "RECEIVE_MESSAGE";
    private readonly string ERROR_IDENTIFIER = "RECEIVE_ERROR";
    private readonly string BOARDCARDS_LEFT_IDENTIFIER = "RECEIVE_PLAYERS_LEFT";
    private readonly string _genericErrorMsg = "Something went wrong.";

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            var result = await _gameService.GetRecentGamePlayed(playerId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            var recentGamePlayedId = result.Data;
            string groupName = recentGamePlayedId.ToString();

            await Clients.Group(groupName).SendAsync(IDENTIFIER, GameState.DISCONNECTED);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(OnDisconnectAsync)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task LeaveGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            var result = await _gameService.LeaveGameById(playerId, gameId);
            if (result.IsError)
            {
                await Clients.Group(groupName).SendAsync(ERROR_IDENTIFIER, result.Message);
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync(IDENTIFIER, GameState.PLAYER_LEFT);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(LeaveGame)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task JoinGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            var result = await _gameService.JoinGameById(playerId, gameId);
            if (result.IsError)
            {
                await Clients.Group(groupName).SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            var game = result.Data;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            if (game.GameState == GameState.ONLY_HOST_CHOSING_CARDS)
                await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameState.ONLY_HOST_CHOSING_CARDS);

            if (game.GameState == GameState.BOTH_CHOSING_CARDS)
                await Clients.Groups(groupName).SendAsync(IDENTIFIER, GameState.BOTH_CHOSING_CARDS);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(JoinGame)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task SubscribeToGameAsHost(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(SubscribeToGameAsHost)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task UpdateGameState(int gameId, GameState state)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            var result = await _gameService.UpdateGameState(playerId, gameId, state);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            await Clients.Groups(groupName).SendAsync(IDENTIFIER, state);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateGameState)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task SendMessage(int gameId, string messageText)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();
            string encodedMessageText = EncodeForJsAndHtml(messageText);

            var result = await _messageService.CreateMessage(playerId, gameId, messageText);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            await Clients.Groups(groupName).SendAsync(MESSAGE_IDENTIFIER, messageText);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(SendMessage)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task GuessBoardCard(int gameId, int boardCardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            var result = await _boardService.GuessBoardCard(playerId, gameId, boardCardId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, state);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(GuessBoardCard)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task UpdateBoardCardsActivity(int gameId, int boardId, IEnumerable<BoardCardUpdate> boardCardUpdates)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            var result = await _boardCardService.UpdateBoardCardsActivity(playerId, boardId, boardCardUpdates);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            var boardCardsLeft = result.Data;
            await Clients.Groups(groupName).SendAsync(BOARDCARDS_LEFT_IDENTIFIER, boardCardsLeft);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "(UpdateBoardCardsActivity)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task CreateBoardCards(int gameId, IEnumerable<int> cardIds)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            var result = await _boardCardService.CreateBoardCards(playerId, gameId, cardIds);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, state);
        }
        catch (Exception e)
        {
            logger.LogError(e, "(CreateBoardCards)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task ChooseBoardCard(int gameId, int boardId, int boardCardId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            var result = await _boardService.ChooseBoardCard(playerId, gameId, boardId, boardCardId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, state);
        }
        catch (Exception e)
        {
            logger.LogError(e, "(ChooseBoardCard)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public async Task StartGame(int gameId)
    {
        try
        {
            int playerId = ParsePlayerIdClaim();
            string groupName = gameId.ToString();

            var result = await _gameService.StartGame(playerId, gameId);
            if (result.IsError)
            {
                await Clients.Caller.SendAsync(ERROR_IDENTIFIER, result.Message);
                return;
            }

            var state = result.Data;
            await Clients.Groups(groupName).SendAsync(IDENTIFIER, state);
        }
        catch (Exception e)
        {
            logger.LogError(e, "(StartGame)");
            await Clients.Caller.SendAsync(ERROR_IDENTIFIER, _genericErrorMsg);
        }
    }

    public int ParsePlayerIdClaim() => int.Parse(Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);

    private static string EncodeForJsAndHtml(string input) => JavaScriptEncoder.Default.Encode(HtmlEncoder.Default.Encode(input));
}