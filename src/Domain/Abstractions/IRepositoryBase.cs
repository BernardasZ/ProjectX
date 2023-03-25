﻿using Domain.Filters;
using Domain.Models;

namespace Domain.Abstractions;

public interface IRepositoryBase<TEntity>
	where TEntity : ModelBase
{
	List<TEntity> GetAll(IFilter<TEntity> filter = default);

	TEntity GetById(int id);

	TEntity Insert(TEntity entity);

	TEntity Update(TEntity entity);

	void Delete(TEntity entity);

	void Save();
}