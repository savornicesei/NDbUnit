/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace NDbUnit.Core.OleDb
{
    public class OleDbOperation : DbOperation
    {
        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        public OleDbType OleOleDbType{ get; set; }

        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new OleDbCommand(cmdText);
        }

        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            if (OleOleDbType == OleDbType.SqlServer)
            {
                base.OnInsertIdentity(dataTable, dbCommand, dbTransaction);
            }
        }

        protected override void EnableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            if (OleOleDbType != OleDbType.SqlServer) return;

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
            if (OleOleDbType != OleDbType.SqlServer) return;

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
