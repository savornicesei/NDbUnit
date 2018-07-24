/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
#if MONO
using System.Data.OracleClient;
#else
using Oracle.ManagedDataAccess.Client;
#endif
using System;
using System.Data;
using System.Text;

namespace NDbUnit.OracleClient
{
    public class OracleClientDbCommandBuilder : DbCommandBuilder<OracleConnection>
    {
        public OracleClientDbCommandBuilder(DbConnectionManager<OracleConnection> connectionManager)
            : base(connectionManager)
        {
        }

        public override string QuotePrefix
        {
            get { return "\""; }
        }

        public override string QuoteSuffix
        {
            get { return QuotePrefix; }
        }

        protected override IDbCommand CreateDbCommand()
        {
            OracleCommand command = new OracleCommand();
            return command;
        }

        protected override IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("INSERT INTO {0}(", TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix)));
            StringBuilder sbParam = new StringBuilder();
            IDataParameter sqlParameter;
            IDbCommand sqlInsertCommand = CreateDbCommand();
            foreach (DataRow dataRow in _dataTableSchema.Rows)
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

            sb.Append(String.Format(") VALUES({0})", sbParam));

            sqlInsertCommand.CommandText = sb.ToString();
            #if !MONO
            ((OracleCommand)sqlInsertCommand).BindByName = true;
            #endif

            return sqlInsertCommand;
        }

        protected override IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName)
        {
            var command = base.CreateUpdateCommand(selectCommand, tableName);
            
            #if !MONO
                ((OracleCommand) command).BindByName = true;
            #endif
            
            return command;
        }

        protected override IDbCommand CreateInsertIdentityCommand(IDbCommand selectCommand, string tableName)
        {
            var command = base.CreateInsertIdentityCommand(selectCommand, tableName);
            
            #if !MONO
                ((OracleCommand)command).BindByName = true;
            #endif
            
            return command;
        }

        protected override IDbCommand CreateDeleteCommand(IDbCommand selectCommand, string tableName)
        {
            var command = base.CreateDeleteCommand(selectCommand, tableName);
            
            #if !MONO
                ((OracleCommand)command).BindByName = true;
            #endif
            
            return command;
        }

        protected override IDbCommand CreateSelectCommand(DataSet ds, string tableName)
        {
            var command = base.CreateSelectCommand(ds, tableName);
            
            #if !MONO
                ((OracleCommand)command).BindByName = true;
            #endif
            
            return command;
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            #if MONO
                return new OracleParameter("p" + index, (OracleType) dataRow["ProviderType"],
                                        (int) dataRow["ColumnSize"], (string) dataRow["ColumnName"]);
            #else
                return new OracleParameter("p" + index, (OracleDbType)dataRow["ProviderType"],
                                      (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
            #endif
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }

        protected override string GetIdentityColumnDesignator()
        {
            return "IsAutoIncrement";
        }

        protected override string GetParameterDesignator(int count)
        {
            return ":p" + count;
        }

    }
}
