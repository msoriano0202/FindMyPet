using FindMyPet.DataMigrationConsole.CustomProviders;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DataMigrationConsole
{
    /// <summary>
    /// Do the data migration
    /// </summary>
    public class Migration
    {
        /// <summary>
        /// Update table structure by model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="sqlProvider"></param>
        public void UpdateTable<T>(IDbConnection connection, ISqlProvider sqlProvider) where T : new()
        {
            try
            {
                connection.CreateTableIfNotExists<T>();

                var model = ModelDefinition<T>.Definition;
                string tableName = model.Name;  //the original table
                string tableNameTmp = tableName + "Tmp"; //temp table for save the data

                //get the existing table's columns
                string getDbColumnsSql = sqlProvider.GetColumnNamesSql(tableName);
                var dbColumns = connection.SqlList<string>(getDbColumnsSql);

                //insert the data to a temp table first
                var fkStatement = sqlProvider.MigrateTableSql(connection, tableName, tableNameTmp);
                connection.ExecuteNonQuery(fkStatement.DropStatement);

                //create a new table
                connection.CreateTable<T>();

                //handle the foreign keys
                if (!string.IsNullOrEmpty(fkStatement.CreateStatement))
                {
                    connection.ExecuteNonQuery(fkStatement.CreateStatement);
                }

                //get the new table's columns
                string getModelColumnsSql = sqlProvider.GetColumnNamesSql(tableName);
                var modelColumns = connection.SqlList<string>(getModelColumnsSql);

                //dynamic get columns from model
                List<string> activeFields = dbColumns.Where(dbColumn => modelColumns.Contains(dbColumn)).ToList();

                //move the data from temp table to new table, so that we can keep the original data after migration
                string activeFieldsCommaSep = string.Join(",", activeFields);
                string insertIntoSql = sqlProvider.InsertIntoSql(tableName, "#temp", activeFieldsCommaSep);

                connection.ExecuteSql(insertIntoSql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
