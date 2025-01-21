namespace Backend.Features.Shared.Common
{
    public static class TokenExtractor
    {
        public static int ParsePlayerIdClaim(ClaimsPrincipal user) => int.Parse(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!);
    }
}