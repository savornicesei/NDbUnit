/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace NDbUnit.Core.MySqlClient
{
    public class MySqlDbOperation : DbOperation
    {
        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new MySqlCommand(cmdText);
        }

        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            IDbTransaction sqlTransaction = dbTransaction;

            try
            {
                DisableTableConstraints(dataTable, dbTransaction);

                IDbDataAdapter sqlDataAdapter = CreateDbDataAdapter();
                sqlDataAdapter.InsertCommand = dbCommand;
                sqlDataAdapter.InsertCommand.Connection = sqlTransaction.Connection;
                sqlDataAdapter.InsertCommand.Transaction = sqlTransaction;

                ((DbDataAdapter)sqlDataAdapter).Update(dataTable);

            }
            finally
            {
                EnableTableConstraints(dataTable, dbTransaction);                
            }
        }

        protected override void EnableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            MySqlCommand sqlCommand = (MySqlCommand)CreateDbCommand("SET foreign_key_checks = 1;");
            sqlCommand.Connection = (MySqlConnection)dbTransaction.Connection;
            sqlCommand.Transaction = (MySqlTransaction)dbTransaction;
            sqlCommand.ExecuteNonQuery();
        }

        protected override void DisableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            MySqlCommand sqlCommand = (MySqlCommand)CreateDbCommand("SET foreign_key_checks = 0;");
            sqlCommand.Connection = (MySqlConnection)dbTransaction.Connection;
            sqlCommand.Transaction = (MySqlTransaction)dbTransaction;
            sqlCommand.ExecuteNonQuery();

        }
    }
}
