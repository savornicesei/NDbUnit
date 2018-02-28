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
#if NETSTANDARD
    public class SqlLiteDbUnitTest : NDbUnitTest<SqliteConnection>
#else
    public class SqlLiteDbUnitTest : NDbUnitTest<SQLiteConnection>
#endif
    {
        public SqlLiteDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

#if NETSTANDARD
        public SqlLiteDbUnitTest(SqliteConnection connection)
            : base(connection)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            throw new NotImplementedException("Microsoft.Data.Sqlite does not implement IDbAdapter");
            //return new SQLiteDataAdapter((SqliteCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<SqliteConnection> connectionManager)
        {
            return new SqlLiteDbCommandBuilder(connectionManager);
        }
#else
        public SqlLiteDbUnitTest(SQLiteConnection connection)
            : base(connection)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SQLiteDataAdapter((SQLiteCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<SQLiteConnection> connectionManager)
        {
            return new SqlLiteDbCommandBuilder(connectionManager);
        }
#endif
        
        protected override IDbOperation CreateDbOperation()
        {
            return new SqlLiteDbOperation();
        }

    }

    [Obsolete("Use SqlLiteDbUnitTest class in place of this.")]
    public class SqlLiteUnitTest : SqlLiteDbUnitTest
    {
        public SqlLiteUnitTest(string connectionString) : base(connectionString)
        {
        }

#if NETSTANDARD
        public SqlLiteUnitTest(SqliteConnection connection) : base(connection)
        {
        }
#else
        public SqlLiteUnitTest(SQLiteConnection connection) : base(connection)
        {
        }
#endif
    }
}
