/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace NDbUnit.Core.MySqlClient
{
    public class MySqlDbCommandBuilder : DbCommandBuilder<MySqlConnection>
    {
        public MySqlDbCommandBuilder(DbConnectionManager<MySqlConnection> connectionManager)
            : base(connectionManager)
        {
        }

        public override string QuotePrefix
        {
            get { return "`"; }
        }

        public override string QuoteSuffix
        {
            get { return "`"; }
        }

        protected override IDbCommand CreateDbCommand()
        {

            MySqlCommand command = new MySqlCommand();
            if (CommandTimeOutSeconds != 0)
                command.CommandTimeout = CommandTimeOutSeconds;
            return command;

        }

        protected override IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + "(");
            StringBuilder sbParam = new StringBuilder();
            IDataParameter sqlParameter;
            IDbCommand sqlInsertCommand = CreateDbCommand();
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow))
                {
                    if (notFirstColumn)
                    {
                        sb.Append(", ");
                        sbParam.Append(", ");
                    }

                    notFirstColumn = true;

                    sb.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                    sbParam.Append(GetParameterDesignator(count));

                    sqlParameter = CreateNewSqlParameter(count, dataRow);
                    sqlInsertCommand.Parameters.Add(sqlParameter);

                    ++count;
                }

            }

            sb.Append(") VALUES(" + sbParam + ")");

            sqlInsertCommand.CommandText = sb.ToString();

            return sqlInsertCommand;
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new MySqlParameter("?p" + index, (MySqlDbType)dataRow["ProviderType"],
                                    (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        protected override string GetParameterDesignator(int count)
        {
            return "?p" + count;
        }

    }
}
