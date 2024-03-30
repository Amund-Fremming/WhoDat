using Microsoft.EntityFrameworkCore;
using PlayerEntity;
using GalleryEntity;
using GameEntity;
using MessageEntity;
using BoardEntity;
using CardEntity;
using BoardCardEntity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<PlayerRepository>();
builder.Services.AddScoped<GalleryRepository>();
builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<BoardRepository>();
builder.Services.AddScoped<CardRepository>();
builder.Services.AddScoped<BoardCardRepository>();

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IBoardCardService, BoardCardService>();

builder.Services.AddDbContext<Data.AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
