/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;
using System.Data.SqlClient;

namespace NDbUnit.Core.SqlClient
{
    public class SqlDbCommandBuilder : DbCommandBuilder<SqlConnection>
    {
        public SqlDbCommandBuilder(DbConnectionManager<SqlConnection> connectionManager)
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
            SqlCommand command = new SqlCommand();
            if (CommandTimeOutSeconds != 0)
                command.CommandTimeout = CommandTimeOutSeconds;

            return command;
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new SqlParameter("@p" + index, (SqlDbType)dataRow["ProviderType"],
                                    (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

    }
}
