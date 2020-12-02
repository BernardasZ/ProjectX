using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Services
{
	public interface IAesCryptoService
	{
		string EncryptString(string text);
		string DecryptString(string text);
	}
}
