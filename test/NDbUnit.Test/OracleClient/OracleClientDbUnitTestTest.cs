/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.OracleClient;
using NDbUnit.Test.Common;
using NUnit.Framework;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace NDbUnit.Test.OracleClient
{
    [Category(TestCategories.OracleTests)]
    public class OracleClientDbUnitTestTest : DbUnitTestTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "USER", "USERROLE", "ROLE"
                };
            }
        }

        protected override IUnitTestStub GetUnitTestStub()
        {
            return new OracleClientDbUnitTestStub(DbConnection.OracleClientConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OracleClient.XmlFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OracleClient.XmlSchemaFile;
        }

        protected class OracleClientDbUnitTestStub : OracleClientDbUnitTest, IUnitTestStub
        {
            public OracleClientDbUnitTestStub(string connectionString)
                : base(connectionString)
            {
            }

            protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<OracleConnection> connectionManager)
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
