/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;

namespace NDbUnit.Test.SqlClient
{
    [Category(TestCategories.SqlServerTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    public class SqlDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new SqlDbCommandBuilder(new DbConnectionManager<SqlConnection>(DbConnection.SqlConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new SqlDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = String.Format("dbcc checkident([{0}], RESEED, 0)", table.TableName);
            return new SqlCommand(sql, (SqlConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.SqlServer.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.SqlServer.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.SqlServer.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServer.XmlSchemaFile;
        }

    }
}
