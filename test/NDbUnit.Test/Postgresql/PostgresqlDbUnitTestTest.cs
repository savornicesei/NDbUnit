/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Postgresql;
using Npgsql;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace NDbUnit.Test.Postgresql
{
    [Category(TestCategories.PostgresTests)]
    [TestFixture]
    public class PostgresqlDbUnitTestTest : NDbUnit.Test.Common.DbUnitTestTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "User", "UserRole" 
                };
            }
        }

        protected override IUnitTestStub GetUnitTestStub()
        {
            return new PostgresqlDbUnitTestStub(DbConnection.PostgresqlConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Postgresql.XmlFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Postgresql.XmlSchemaFile;
        }

        protected class PostgresqlDbUnitTestStub : PostgresqlDbUnitTest, IUnitTestStub
        {
            public PostgresqlDbUnitTestStub(string connectionString)
                : base(connectionString)
            {
            }

            protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<NpgsqlConnection> connectionManager )
            {
                return _mockDbCommandBuilder;
            }

            protected override IDbOperation CreateDbOperation()
            {
                return _mockDbOperation;
            }

            protected override FileStream GetXmlSchemaFileStream(string xmlSchemaFile)
            {
                return _mockSchemaFileStream;
            }

            protected override FileStream GetXmlDataFileStream(string xmlFile)
            {
                return _mockDataFileStream;
            }

            public DataSet TestDataSet
            {
                get { return DS; }
            }
        }
    }

}

