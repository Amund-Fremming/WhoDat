using System;
using Data;

namespace BoardEntity;

public class BoardRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;
}