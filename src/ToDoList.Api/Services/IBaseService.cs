using System.Collections.Generic;
using ToDoList.Api.Models;

namespace ToDoList.Api.Services;

public interface IBaseService<TModel>
	where TModel : IBaseModel
{
	List<TModel> GetAll();

	TModel GetById(int id);

	TModel Add(TModel item);

	TModel Update(TModel item);

	void Delete(TModel item);
}