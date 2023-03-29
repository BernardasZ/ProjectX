using Domain.Filters;
using Domain.Models;

namespace Domain.Abstractions;

public interface IServiceBase<TModel>
	where TModel : ModelBase
{
	Task<List<TModel>> GetAllAsync(IFilter<TModel> filter = default);

	Task<TModel> GetByIdAsync(int id);

	Task<TModel> CreateAsync(TModel item);

	Task<TModel> UpdateAsync(TModel item);

	Task DeleteAsync(TModel item);
}