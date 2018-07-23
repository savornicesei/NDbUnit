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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace NDbUnit.OracleClient
{
    public class OracleClientDbOperation : DbOperation
    {
        public override string QuotePrefix
        {
            get { return "\""; }
        }

        public override string QuoteSuffix
        {
            get { return QuotePrefix; }
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new OracleCommand(cmdText);
        }

        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new OracleDataAdapter();
        }

        protected override void DisableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            this.enableDisableTableConstraints("DISABLE", dataTable, dbTransaction);
        }

        protected override void EnableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            this.enableDisableTableConstraints("ENABLE", dataTable, dbTransaction);
        }

        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            throw new NotSupportedException("OnInsertIdentity not supported!");
        }

        private void enableDisableTableConstraints(String enableDisable, DataTable dataTable, IDbTransaction dbTransaction)
        {
            DbCommand dbCommand = null;
            DbParameter dbParam = null;
            DbDataReader dbReader = null;
            IList<String> altersList = new List<String>();

            String queryEnables =
                " SELECT 'ALTER TABLE '"
                + "    || table_name"
                + "    || ' " + enableDisable + " CONSTRAINT '"
                + "    || constraint_name AS alterComm"
                + "     FROM user_constraints"
                + "    WHERE UPPER(table_name) = UPPER(:tabela)"
                + "    AND constraint_type IN ('C', 'R')";

            dbCommand = new OracleCommand();
            dbCommand.CommandText = queryEnables;
            dbCommand.Connection = (DbConnection)dbTransaction.Connection;
            dbCommand.Transaction = (DbTransaction)dbTransaction;

            dbParam = new OracleParameter();
            dbParam.ParameterName = "tabela";
            dbParam.Value = dataTable.TableName;
            dbParam.DbType = DbType.String;
            dbCommand.Parameters.Add(dbParam);

            dbReader = dbCommand.ExecuteReader();
            while (dbReader.Read())
            {
                altersList.Add(dbReader.GetString(dbReader.GetOrdinal("alterComm")));
            }

            dbReader.Close();

            foreach (String returnedCommand in altersList)
            {

                var escapedCommand = returnedCommand.Replace(" " + dataTable.TableName + " ", TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix));

                dbCommand = new OracleCommand();
                dbCommand.CommandText = escapedCommand;
                dbCommand.Connection = (DbConnection)dbTransaction.Connection;
                dbCommand.Transaction = (DbTransaction)dbTransaction;
                dbCommand.ExecuteNonQuery();
            }
        }

    }
}
