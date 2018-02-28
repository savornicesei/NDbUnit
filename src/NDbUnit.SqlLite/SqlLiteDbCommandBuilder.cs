/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
#if NETSTANDARD
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace NDbUnit.Core.SqlLite
{
#if NETSTANDARD
    public class SqlLiteDbCommandBuilder : DbCommandBuilder<SqliteConnection>
#else
    public class SqlLiteDbCommandBuilder : DbCommandBuilder<SQLiteConnection>
#endif
    {
        private new DataTable _dataTableSchema;
#if NETSTANDARD
        public SqlLiteDbCommandBuilder(DbConnectionManager<SqliteConnection> connectionManager )
            : base(connectionManager)
#else
        public SqlLiteDbCommandBuilder(DbConnectionManager<SQLiteConnection> connectionManager )
            : base(connectionManager)
#endif
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
#if NETSTANDARD
            var command = new SqliteCommand();
#else
            var command = new SQLiteCommand();
#endif

            if (CommandTimeOutSeconds != 0)
                command.CommandTimeout = CommandTimeOutSeconds;

            return command;
        }

        protected override IDbCommand CreateDeleteAllCommand(string tableName)
        {
#if NETSTANDARD
            return
                new SqliteCommand("DELETE FROM " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix));
#else
            return
                new SQLiteCommand("DELETE FROM " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix));
#endif
        }

        protected override IDbCommand CreateDeleteCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + " WHERE ");

#if NETSTANDARD
            SqliteCommand sqlDeleteCommand = CreateDbCommand() as SqliteCommand;
#else
            SQLiteCommand sqlDeleteCommand = CreateDbCommand() as SQLiteCommand;
#endif

            int count = 1;
            DbParameter sqlParameter;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow) && (bool)dataRow[SchemaColumns.IsKey]) // A key column.
                {
                    if (count != 1)
                    {
                        sb.Append(" AND ");
                    }

                    sb.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                    sb.Append("=@p" + count);

#if NETSTANDARD
                    sqlParameter = (SqliteParameter)CreateNewSqlParameter(count, dataRow);
#else
                    sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
#endif
                    sqlDeleteCommand.Parameters.Add(sqlParameter);

                    ++count;
                }
            }

            sqlDeleteCommand.CommandText = sb.ToString();

            return sqlDeleteCommand;
        }

        protected override IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + "(");
            StringBuilder sbParam = new StringBuilder();
            DbParameter sqlParameter = null;
            
#if NETSTANDARD
            SqliteCommand sqlInsertCommand = CreateDbCommand() as SqliteCommand;
#else
            SQLiteCommand sqlInsertCommand = CreateDbCommand() as SQLiteCommand;
#endif
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow) && !IsAutoIncrementing(dataRow)) // Not an identity column.
                {
                    if (notFirstColumn)
                    {
                        sb.Append(", ");
                        sbParam.Append(", ");
                    }

                    notFirstColumn = true;

                    sb.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                    sbParam.Append("@p" + count);

#if NETSTANDARD
                    sqlParameter = (SqliteParameter)CreateNewSqlParameter(count, dataRow);
#else
                    sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
#endif
                    sqlInsertCommand.Parameters.Add(sqlParameter);

                    ++count;
                }
            }

            sb.Append(") VALUES(" + sbParam + ")");

            sqlInsertCommand.CommandText = sb.ToString();

            return sqlInsertCommand;
        }

        protected override IDbCommand CreateInsertIdentityCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + "(");
            StringBuilder sbParam = new StringBuilder();
            DbParameter sqlParameter = null;
            
#if NETSTANDARD
            SqliteCommand sqlInsertIdentityCommand = CreateDbCommand() as SqliteCommand;
#else
            SQLiteCommand sqlInsertIdentityCommand = CreateDbCommand() as SQLiteCommand;
#endif
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

                    sb.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                    sbParam.Append("@p" + count);

#if NETSTANDARD
                    sqlParameter = (SqliteParameter)CreateNewSqlParameter(count, dataRow);
#else
                    sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
#endif
                    sqlInsertIdentityCommand.Parameters.Add(sqlParameter);

                    ++count;
                }
            }

            sb.Append(") VALUES(" + sbParam + ")");

            sqlInsertIdentityCommand.CommandText = sb.ToString();

            return sqlInsertIdentityCommand;
        }


        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
#if NETSTANDARD
            return new SqliteParameter("@p" + index, (SqliteType)dataRow[SchemaColumns.ProviderType],
                                       (int)dataRow[SchemaColumns.ColumnSize],
                                       (string)dataRow[SchemaColumns.ColumnName]);
#else
            return new SQLiteParameter("@p" + index, (DbType)dataRow[SchemaColumns.ProviderType],
                (int)dataRow[SchemaColumns.ColumnSize],
                (string)dataRow[SchemaColumns.ColumnName]);
#endif
        }

        protected override IDbCommand CreateSelectCommand(DataSet ds, string tableName)
        {
#if NETSTANDARD
            SqliteCommand sqlSelectCommand = CreateDbCommand() as SqliteCommand;
#else
            SQLiteCommand sqlSelectCommand = CreateDbCommand() as SQLiteCommand;
#endif

            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder("SELECT ");
            DataTable dataTable = ds.Tables[tableName];
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                if (notFirstColumn)
                {
                    sb.Append(", ");
                }

                notFirstColumn = true;

                sb.Append(QuotePrefix + dataColumn.ColumnName + QuoteSuffix);
            }

            sb.Append(" FROM ");
            sb.Append(TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix));

            sqlSelectCommand.CommandText = sb.ToString();
            sqlSelectCommand.Connection = ConnectionManager.GetConnection();

            try
            {
                _dataTableSchema = GetSchemaTable(sqlSelectCommand);
            }
            catch (Exception e)
            {
                string message =
                    String.Format(
                        "SqlDbCommandBuilder.CreateSelectCommand(DataSet, string) failed for tableName = '{0}'",
                        tableName);
                throw new NDbUnitException(message, e);
            }

            return sqlSelectCommand;
        }

        protected override IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + " SET ");

#if NETSTANDARD
            SqliteCommand sqlUpdateCommand = CreateDbCommand() as SqliteCommand;
#else
            SQLiteCommand sqlUpdateCommand = CreateDbCommand() as SQLiteCommand;
#endif

            int count = 1;
            bool notFirstKey = false;
            bool notFirstColumn = false;
            DbParameter sqlParameter = null;
            StringBuilder sbPrimaryKey = new StringBuilder();

            bool containsAllPrimaryKeys = true;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (!(bool)dataRow[SchemaColumns.IsKey])
                {
                    containsAllPrimaryKeys = false;
                    break;
                }
            }

            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow))
                {
                    // A key column.
                    if ((bool)dataRow[SchemaColumns.IsKey])
                    {
                        if (notFirstKey)
                        {
                            sbPrimaryKey.Append(" AND ");
                        }

                        notFirstKey = true;

                        sbPrimaryKey.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                        sbPrimaryKey.Append("=@p" + count);

#if NETSTANDARD
                        sqlParameter = (SqliteParameter)CreateNewSqlParameter(count, dataRow);
#else
                        sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
#endif
                        sqlUpdateCommand.Parameters.Add(sqlParameter);

                        ++count;
                    }


                    if (containsAllPrimaryKeys || !(bool)dataRow[SchemaColumns.IsKey])
                    {
                        if (notFirstColumn)
                        {
                            sb.Append(", ");
                        }

                        notFirstColumn = true;

                        sb.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                        sb.Append("=@p" + count);

#if NETSTANDARD
                        sqlParameter = (SqliteParameter)CreateNewSqlParameter(count, dataRow);
#else
                        sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
#endif
                        sqlUpdateCommand.Parameters.Add(sqlParameter);

                        ++count;
                    }
                }
            }

            sb.Append(" WHERE " + sbPrimaryKey);

            sqlUpdateCommand.CommandText = sb.ToString();

            return sqlUpdateCommand;
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
#if NETSTANDARD
            return new SqliteConnection(connectionString);
#else
            return new SQLiteConnection(connectionString);
#endif
        }

        /// <summary>
        /// Since SQLite keys are auto incremented by default we need to check to see if the column
        /// is a key as well, since not all columns will be marked with AUTOINCREMENT
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool IsAutoIncrementing(DataRow row)
        {
            return (bool)row[SchemaColumns.IsAutoIncrement];
        }

        private static class SchemaColumns
        {
            public const string ColumnName = "ColumnName";
            public const string ColumnOrdinal = "ColumnOrdinal";
            public const string ColumnSize = "ColumnSize";
            public const string NumericalPrecision = "NumericalPrecision";
            public const string NumericalScale = "NumericalScale";
            public const string IsUnique = "IsUnique";
            public const string IsKey = "IsKey";
            public const string BaseServerName = "BaseServerName";
            public const string BaseCatalogName = "BaseCatalogName";
            public const string BaseColumnName = "BaseColumnName";
            public const string BaseSchemaName = "";
            public const string IsAutoIncrement = "IsAutoIncrement";
            public const string ProviderType = "ProviderType";

        }

    }
}
