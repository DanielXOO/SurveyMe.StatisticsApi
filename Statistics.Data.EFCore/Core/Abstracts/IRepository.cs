namespace Statistics.Data.EFCore.Core.Abstracts;

public interface IRepository<T>
{
    Task CreateAsync(T data);

    Task UpdateAsync(T data);

    Task DeleteAsync(T data);
}