/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System;
using System.Data;
using System.Data.SqlServerCe;

namespace NDbUnit.Core.SqlServerCe
{
    public class SqlCeDbUnitTest : NDbUnitTest<SqlCeConnection>
    {
        public SqlCeDbUnitTest(SqlCeConnection connection)
            : base(connection)
        {
        }

        public SqlCeDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SqlCeDataAdapter((SqlCeCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<SqlCeConnection> connectionManager)
        {
            return new SqlCeDbCommandBuilder(connectionManager);
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new SqlCeDbOperation();
        }

    }

    [Obsolete("Use SqlCeDbUnitTest class in place of this.")]
    public class SqlCeUnitTest : SqlCeDbUnitTest
    {
        public SqlCeUnitTest(SqlCeConnection connection) : base(connection)
        {
        }

        public SqlCeUnitTest(string connectionString) : base(connectionString)
        {
        }
    }
}
