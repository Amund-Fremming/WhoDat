using System;
using Data;

namespace GameEntity;

public class GameRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;
}