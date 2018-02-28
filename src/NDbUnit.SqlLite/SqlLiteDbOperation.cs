/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System;
using System.Data;
#if NETSTANDARD
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

namespace NDbUnit.Core.SqlLite
{
    public class SqlLiteDbOperation : DbOperation
    {
        protected override IDbDataAdapter CreateDbDataAdapter()
        {
#if NETSTANDARD
            throw new NotImplementedException("Microsoft.Data.Sqlite does not implement IDbAdapter");
#else
            return new SQLiteDataAdapter();
#endif
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
#if NETSTANDARD
            return new SqliteCommand(cmdText);
#else
            return new SQLiteCommand(cmdText);
#endif
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
