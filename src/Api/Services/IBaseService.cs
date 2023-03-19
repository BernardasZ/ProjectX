using Api.Models;
using System.Collections.Generic;

namespace Api.Services;

public interface IBaseService<TModel>
	where TModel : IBaseModel
{
	List<TModel> GetAll();

	TModel GetById(int id);

	TModel Create(TModel item);

	TModel Update(TModel item);

	void Delete(TModel item);
}