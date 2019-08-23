using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace PCAxis.Sql.DbConfig
{
    class DefaultDbStringProvider : IDbStringProvider 
    {

        public string GetConnectionString(SqlDbConfig config, string user, string password)
        {
            if (String.IsNullOrEmpty(user) || String.IsNullOrEmpty(password))
            {
                if (!(String.IsNullOrEmpty(user) && String.IsNullOrEmpty(password)))
                {
                    throw new Exception("BUG: user and password must both be empty when one is.");
                }

                if (String.IsNullOrEmpty(config.Database.Connection.DefaultUser) || String.IsNullOrEmpty(config.Database.Connection.DefaultPassword))
                {
                    throw new Exception("Please add DefaultUser and DefaultPassword");
                }

                return GetConnectionString(config, config.Database.Connection.DefaultUser, config.Database.Connection.DefaultPassword); 
            }
            else
            {

                var db = config.Database;
                string tmp1 = string.Copy(db.Connection.ConnectionString);
                DbConnectionStringBuilder connBuilder = new DbConnectionStringBuilder();
                connBuilder.ConnectionString = tmp1;
                if (connBuilder.ContainsKey(db.Connection.KeyForPassword))
                {
                    connBuilder.Remove(db.Connection.KeyForPassword);
                    connBuilder.Add(db.Connection.KeyForPassword, password);
                }
                if (connBuilder.ContainsKey(db.Connection.KeyForUser))
                {
                    connBuilder.Remove(db.Connection.KeyForUser);
                    connBuilder.Add(db.Connection.KeyForUser, user);
                }
                return connBuilder.ConnectionString;
            }
            
        }
    }
}
