/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace NDbUnit.Core.SqlClient
{
    public class SqlDbOperation : DbOperation
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
            return new SqlDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new SqlCommand(cmdText);
        }

        protected override void EnableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            DbCommand sqlCommand =
                    (DbCommand)CreateDbCommand("ALTER TABLE " +
                                    TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix) +
                                    " CHECK CONSTRAINT ALL");
            sqlCommand.Connection = (DbConnection)dbTransaction.Connection;
            sqlCommand.Transaction = (DbTransaction)dbTransaction;
            sqlCommand.ExecuteNonQuery();
        }

        protected override void DisableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            DbCommand sqlCommand =
                    (DbCommand)CreateDbCommand("ALTER TABLE " +
                                    TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix) +
                                    " NOCHECK CONSTRAINT ALL");
            sqlCommand.Connection = (DbConnection)dbTransaction.Connection;
            sqlCommand.Transaction = (DbTransaction)dbTransaction;
            sqlCommand.ExecuteNonQuery();
        }
    }
}
