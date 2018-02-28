/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Core.SqlLite;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
#if NETSTANDARD
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif
using System.IO;

namespace NDbUnit.Test.SqlLite
{
    [Category(TestCategories.SqliteTests)]
    [TestFixture]
    public class SqlLiteUnitTestTest : NDbUnit.Test.Common.DbUnitTestTestBase
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
            return new SqliteDbUnitTestStub(DbConnection.SqlLiteConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Sqlite.XmlFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Sqlite.XmlSchemaFile;
        }

        protected class SqliteDbUnitTestStub : SqlLiteDbUnitTest, IUnitTestStub
        {
            public SqliteDbUnitTestStub(string connectionString)
                : base(connectionString)
            {
            }

#if NETSTANDARD
            protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<SqliteConnection> connectionManager )
            {
                return _mockDbCommandBuilder;
            }
#else
            protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<SQLiteConnection> connectionManager )
            {
                return _mockDbCommandBuilder;
            }
#endif

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

