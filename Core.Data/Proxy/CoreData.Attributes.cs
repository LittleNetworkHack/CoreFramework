using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Data
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class CoreDataSchemaAttribute : Attribute
	{
		public CoreDataSchema Schema { get; }
		public CoreDataSchemaAttribute(CoreDataSchema schema)
		{
			Schema = schema;
		}
	}

	public enum CoreDataSchema
	{
		dbo = 0,
		Parametri = 1
	}
}
