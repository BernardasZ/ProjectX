using Domain.Filters;
using Domain.Models;

namespace Domain.Abstractions;

public interface IServiceBase<TModel>
	where TModel : ModelBase
{
	List<TModel> GetAll(IFilter<TModel> filter = default);

	TModel GetById(int id);

	TModel Create(TModel item);

	TModel Update(TModel item);

	void Delete(TModel item);
}