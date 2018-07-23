/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.OleDb;
using NUnit.Framework;
using System;
using System.Data;
using System.Data.OleDb;

namespace NDbUnit.Test.OleDb
{
    [Category(TestCategories.OleDbTests)]
    [TestFixture]
    public class OleDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new NDbUnit.OleDb.OleDbCommandBuilder(new DbConnectionManager<OleDbConnection>(DbConnection.OleDbConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new OleDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "dbcc checkident([" + table.TableName + "], RESEED, 0)";
            return new OleDbCommand(sql, (OleDbConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OleDb.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.OleDb.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.OleDb.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OleDb.XmlSchemaFile;
        }

    }
}
