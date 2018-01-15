/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;
using System.Data.SqlServerCe;

namespace NDbUnit.Core.SqlServerCe
{
    public class SqlCeDbCommandBuilder : DbCommandBuilder<SqlCeConnection>
    {
        public SqlCeDbCommandBuilder(DbConnectionManager<SqlCeConnection> connectionManager)
            : base(connectionManager)
        {
        }

        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        protected override IDbCommand CreateDbCommand()
        {
            SqlCeCommand command = new SqlCeCommand();

            if (CommandTimeOutSeconds != 0)
                command.CommandTimeout = CommandTimeOutSeconds;

            return command;
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new SqlCeParameter("@p" + index, ((SqlCeType)dataRow["ProviderType"]).SqlDbType,
                                      (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new SqlCeConnection(connectionString);
        }

    }
}