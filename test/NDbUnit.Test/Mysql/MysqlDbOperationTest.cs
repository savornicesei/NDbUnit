/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using MySql.Data.MySqlClient;
using NDbUnit.Core;
using NDbUnit.Core.MySqlClient;
using NUnit.Framework;
using System;
using System.Data;

namespace NDbUnit.Test.Mysql
{
    [Category(TestCategories.MySqlTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    class MySqlDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new MySqlDbCommandBuilder(new DbConnectionManager<MySqlConnection>(DbConnection.MySqlConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new MySqlDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "ALTER TABLE " + table.TableName + " AUTO_INCREMENT=1;";
            return new MySqlCommand(sql, (MySqlConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.MySql.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.MySql.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.MySql.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.MySql.XmlSchemaFile;
        }

    }

}