/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Core.SqlLite;
using NUnit.Framework;
using System;
using System.Data;
#if NETSTANDARD
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif

namespace NDbUnit.Test.SqlLite
{
    [Category(TestCategories.SqliteTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    public class SqlLiteDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
#if NETSTANDARD
            return new SqlLiteDbCommandBuilder(new DbConnectionManager<SqliteConnection>(DbConnection.SqlLiteConnectionString));
#else
            return new SqlLiteDbCommandBuilder(new DbConnectionManager<SQLiteConnection>(DbConnection.SqlLiteConnectionString));
#endif
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new SqlLiteDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = String.Format("delete from sqlite_sequence where name = '{0}'", table.TableName);
            
#if NETSTANDARD
            return new SqliteCommand(sql, (SqliteConnection)_commandBuilder.Connection);
#else
            return new SQLiteCommand(sql, (SQLiteConnection)_commandBuilder.Connection);
#endif
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Sqlite.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.Sqlite.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.Sqlite.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Sqlite.XmlSchemaFile;
        }

    }
}
