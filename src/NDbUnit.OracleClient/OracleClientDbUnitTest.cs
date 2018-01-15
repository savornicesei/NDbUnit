/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace NDbUnit.OracleClient
{
    public class OracleClientDbUnitTest : NDbUnitTest<OracleConnection>
    {
        public OracleClientDbUnitTest(OracleConnection connection)
            : base(connection)
        {
        }

        public OracleClientDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            OracleDataAdapter oda = new OracleDataAdapter();
            oda.SelectCommand = (OracleCommand)command;
            return oda;
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<OracleConnection> connectionManager)
        {
            OracleClientDbCommandBuilder commandBuilder = new OracleClientDbCommandBuilder(connectionManager);
            commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
            return commandBuilder;
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new OracleClientDbOperation();
        }
    }
}
