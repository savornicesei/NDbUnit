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
using System.Data.SQLite;

namespace NDbUnit.Test.SqlLite
{
    [Category(TestCategories.SqliteTests)]
    [TestFixture]
    public class SqlLiteDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new SqlLiteDbCommandBuilder(new DbConnectionManager<SQLiteConnection>(DbConnection.SqlLiteConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new SqlLiteDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = String.Format("delete from sqlite_sequence where name = '{0}'", table.TableName);
            return new SQLiteCommand(sql, (SQLiteConnection)_commandBuilder.Connection);
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
