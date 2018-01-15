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
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace NDbUnit.Test.MySqlDb
{
    [Category(TestCategories.MySqlTests)]
    [TestFixture]
    public class MySqlDbUnitTestTest : NDbUnit.Test.Common.DbUnitTestTestBase
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
            return new MySqlDbUnitTestStub(DbConnection.MySqlConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.MySql.XmlFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.MySql.XmlSchemaFile;
        }

        protected class MySqlDbUnitTestStub : MySqlDbUnitTest, IUnitTestStub
        {
            public MySqlDbUnitTestStub(string connectionString)
                : base(connectionString)
            {
            }

            protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<MySqlConnection> connectionManager )
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

