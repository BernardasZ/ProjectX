using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ToDoList.Api.Enums;

namespace ToDoList.Api.Exeptions
{
	public class GenericException : Exception
	{
		public string ErrorCode { get; set; }
		public GenericException(GenericErrorEnum errorEnum)
		{
			this.ErrorCode = errorEnum.ToString();
		}

		protected GenericException(SerializationInfo info, StreamingContext context) 
			: base(info, context)
		{

		}

		public GenericException()
		{

		}
	}
}
