/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;
using Microsoft.Data.Sqlite;

namespace NDbUnit.Core.SqlLite
{
    public class SqlLiteDbOperation : DbOperation
    {
        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new SQliteDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new SqliteCommand(cmdText);
        }

        /// <summary>
        /// SQLite doesn't need any changes to insert PK values.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="dbCommand"></param>
        /// <param name="dbTransaction"></param>
        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            OnInsert(dataTable, dbCommand, dbTransaction);
        }
    }
}
