using Backend.Features.Database;
using Backend.Features.Shared.Common.Entity;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Shared.Common.Repository
{
    public abstract class RepositoryBase<T, U>(ILogger<U> logger, AppDbContext context) : IRepository<T> where T : class, IEntity
    {
        private readonly ILogger<U> _logger = logger;
        private readonly AppDbContext _context = context;

        public async Task<Result<int>> Create(T entity)
        {
            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity.ID;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"(Create: {typeof(T).Name})");
                return new Error(e, $"Failed to create {typeof(T).Name}");
            }
        }

        public async Task<Result> Delete(T entity)
        {
            try
            {
                var result = await GetById(entity.ID);
                if (result.IsError)
                    return new Error(new KeyNotFoundException($"{typeof(T).Name} id does not exist"), $"{typeof(T).Name} does not exist.");

                _context.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"(Delete: {typeof(T).Name})");
                return new Error(e, $"Failed to create {typeof(T).Name}");
            }
        }

        public async Task<Result<T>> GetById(int ID)
        {
            try
            {
                var entityName = typeof(T).Name.ToString().Replace("Entity", "");
                var data = await _context.FindAsync(typeof(T), ID);
                if (data == null)
                    return new Error(new KeyNotFoundException($"{entityName} does not exist."), $"{entityName}");

                var convertedData = (T)Convert.ChangeType(data, typeof(T));
                return convertedData;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"(GetById: {typeof(T).Name})");
                return new Error(e, $"Failed to create {typeof(T).Name}");
            }
        }
    }
}