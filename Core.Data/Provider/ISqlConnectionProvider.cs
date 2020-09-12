using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Core.Data
{
	public interface ISqlConnectionProvider
	{
		string GetConnectionString();
		SqlConnection CreateConnection(bool open = true);
	}
}
