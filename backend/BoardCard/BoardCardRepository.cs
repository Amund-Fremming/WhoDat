using System;
using Data;

namespace BoardCardEntity;

public class BoardCardRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;
}