/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Postgresql;
using NDbUnit.Test.Common;
using Npgsql;
using NUnit.Framework;
using System;
using System.Data;

namespace NDbUnit.Test.Postgresql
{
    [Category(TestCategories.PostgresTests)]
    [TestFixture]
    internal class PostgresqlDbOperationTest : DbOperationTestBase
    {
        protected override IDbCommandBuilder GetCommandBuilder()
        {
            return new PostgresqlDbCommandBuilder(new DbConnectionManager<NpgsqlConnection>(DbConnection.PostgresqlConnectionString));
        }

        protected override IDbOperation GetDbOperation()
        {
            return new PostgresqlDbOperation();
        }


        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = string.Format("ALTER SEQUENCE \"{0}_{1}_seq\" RESTART WITH 1;", table.TableName,
                                       column.ColumnName);
            return new NpgsqlCommand(sql, (NpgsqlConnection) _commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Postgresql.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.Postgresql.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.Postgresql.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Postgresql.XmlSchemaFile;
        }
    }
}