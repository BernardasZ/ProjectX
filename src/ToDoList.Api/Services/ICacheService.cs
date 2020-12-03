using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Services
{
	public interface ICacheService<T> where T : class
	{
		T GetCache();
	}
}
