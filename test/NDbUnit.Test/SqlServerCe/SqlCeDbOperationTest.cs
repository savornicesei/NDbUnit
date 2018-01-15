/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Core.SqlServerCe;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlServerCe;

namespace NDbUnit.Test.SqlServerCe
{
    [Category(TestCategories.SqlServerCeTests)]
    [TestFixture]
    class SqlCeDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new SqlCeDbCommandBuilder(new DbConnectionManager<SqlCeConnection>(DbConnection.SqlCeConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new SqlCeDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "ALTER TABLE [" + table.TableName + "] ALTER COLUMN [" + column.ColumnName +
                                         "] IDENTITY (1,1)";
            return new SqlCeCommand(sql, (SqlCeConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlSchemaFile;
        }

    }
}
