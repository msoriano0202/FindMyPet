using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DataMigrationConsole.CustomProviders
{
    /// <summary>
    /// Interface for Sql provider, you can implement it for your custom provider
    /// </summary>
    public interface ISqlProvider
    {
        /// <summary>
        /// Generate drop FK and create FK sql and temp table for migrate the table data
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="currentName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        FKStatement MigrateTableSql(IDbConnection connection, string currentName, string newName);

        string GetColumnNamesSql(string tableName);

        string InsertIntoSql(string intoTableName, string fromTableName, string commaSeparatedColumns);
    }
}
