using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DataMigrationConsole.CustomProviders
{
    /// <summary>
    /// I didn't try this provider, just for the sample, you can try and improve it if you need.
    /// </summary>
    public class MySqlProvider : ISqlProvider
    {
        public FKStatement MigrateTableSql(IDbConnection connection, string currentName, string newName)
        {
            var fkStatement = new FKStatement();
            fkStatement.DropStatement = "RENAME TABLE `" + currentName + "` TO `" + newName + "`;";
            return fkStatement;
        }

        public string GetColumnNamesSql(string tableName)
        {
            return "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + tableName + "';";
        }

        public string InsertIntoSql(string intoTableName, string fromTableName, string commaSeparatedColumns)
        {
            return "INSERT INTO `" + intoTableName + "` (" + commaSeparatedColumns + ") SELECT " + commaSeparatedColumns + " FROM `" + fromTableName + "`;";
        }

        public string DropTableSql(string tableName)
        {
            return "DROP TABLE `" + tableName + "`;";
        }
    }
}
