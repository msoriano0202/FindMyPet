using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyPet.DataMigrationConsole.CustomProviders
{
    /// <summary>
    /// MSSQL provider
    /// </summary>
    public class MSSqlProvider : ISqlProvider
    {
        /// <summary>
        /// Generate migration SQL, base on individual Database, so we need to handle this by difference provider
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="currentName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public FKStatement MigrateTableSql(IDbConnection connection, string currentName, string newName)
        {
            var fkStatement = new FKStatement();
            //get the drop and re-create foreign keys sqls
            var sql_get_foreign_keys = @"SELECT OBJECT_NAME(fk.parent_object_id) ParentObject, 
                    OBJECT_NAME(fk.referenced_object_id) ReferencedObject,
                    'ALTER TABLE ' + s.name + '.' + OBJECT_NAME(fk.parent_object_id)
                        + ' DROP CONSTRAINT ' + fk.NAME + ' ;' AS DropStatement,
                    'ALTER TABLE ' + s.name + '.' + OBJECT_NAME(fk.parent_object_id)
                    + ' ADD CONSTRAINT ' + fk.NAME + ' FOREIGN KEY (' + COL_NAME(fk.parent_object_id, fkc.parent_column_id)
                        + ') REFERENCES ' + ss.name + '.' + OBJECT_NAME(fk.referenced_object_id)
                        + '(' + COL_NAME(fk.referenced_object_id, fkc.referenced_column_id) + ');' AS CreateStatement
                FROM
                    sys.foreign_keys fk
                INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
                INNER JOIN sys.schemas s ON fk.schema_id = s.schema_id
                INNER JOIN sys.tables t ON fkc.referenced_object_id = t.object_id
                INNER JOIN sys.schemas ss ON t.schema_id = ss.schema_id
                WHERE
                    OBJECT_NAME(fk.referenced_object_id) = '" + currentName + "' AND ss.name = 'dbo';";

            var fkSql = connection.SqlList<FKStatement>(sql_get_foreign_keys);
            if (fkSql.Count > 0)
            {
                foreach (var fk in fkSql)
                {
                    fkStatement.DropStatement += fk.DropStatement;
                    if (fk.ParentObject != currentName)
                    {
                        fkStatement.CreateStatement += fk.CreateStatement;
                    }
                }
            }

            fkStatement.DropStatement += " select * into #temp from (select * from [" + currentName + "]) as tmp; drop table [" + currentName + "]; ";
            return fkStatement;
        }

        /// <summary>
        /// Get the table's columns
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetColumnNamesSql(string tableName)
        {
            return "SELECT name FROM syscolumns  WHERE id = OBJECT_ID('" + tableName + "');";
        }

        /// <summary>
        /// Insert data to new table, for MSSQL server 2008 above, I will disable all CONSTRAINT before insert data and enable them after done.
        /// </summary>
        /// <param name="intoTableName"></param>
        /// <param name="fromTableName"></param>
        /// <param name="commaSeparatedColumns"></param>
        /// <returns></returns>
        public string InsertIntoSql(string intoTableName, string fromTableName, string commaSeparatedColumns)
        {
            return "EXEC sp_msforeachtable \"ALTER TABLE ? NOCHECK CONSTRAINT all\"; SET IDENTITY_INSERT [" + intoTableName + "] ON; INSERT INTO [" + intoTableName + "] (" +
                commaSeparatedColumns + ") SELECT " + commaSeparatedColumns + " FROM [" + fromTableName + "]; SET IDENTITY_INSERT [" + intoTableName + "] OFF;  drop table [" + fromTableName + "];EXEC sp_msforeachtable \"ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all\"";
        }
    }

}
