/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using Npgsql;
using System.Data;

namespace NDbUnit.Postgresql
{
    public class PostgresqlDbUnitTest : NDbUnitTest<NpgsqlConnection>
    {
        public PostgresqlDbUnitTest(NpgsqlConnection connection)
            : base(connection)
        {
        }

        public PostgresqlDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            var oda = new NpgsqlDataAdapter();
            oda.SelectCommand = (NpgsqlCommand)command;
            return oda;
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<NpgsqlConnection> connectionManager )
        {
            var commandBuilder = new PostgresqlDbCommandBuilder(connectionManager);
            commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
            return commandBuilder;
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new PostgresqlDbOperation();
        }
    }
}
