using System;
using Data;

namespace CardEntity;

public class CardRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;
}