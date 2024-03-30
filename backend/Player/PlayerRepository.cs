using System;
using Data;

namespace PlayerEntity;

public class PlayerRepository(AppDbContext context)
{
    public readonly AppDbContext _context = context;
}