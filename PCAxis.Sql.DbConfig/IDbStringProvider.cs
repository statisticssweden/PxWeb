using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCAxis.Sql.DbConfig
{
    public interface IDbStringProvider
    {
        string GetConnectionString(SqlDbConfig config, string user, string password);
    }
}
