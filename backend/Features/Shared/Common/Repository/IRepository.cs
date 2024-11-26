using Backend.Features.Shared.Common.Entity;
using Backend.Features.Shared.ResultPattern;

namespace Backend.Features.Shared.Common.Repository
{
    public interface IRepository<T> where T : IEntity
    {
        Task<Result<T>> GetById(int ID);

        Task<Result<int>> Create(T entity);

        Task<Result> Delete(T entity);
    }
}