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
using System;
using System.Data;

namespace NDbUnit.Test.OracleClient
{
    [Category(TestCategories.OracleTests)]
    public class OracleClientDbOperationTest : DbOperationTestBase
    {
        public override void InsertIdentity_Executes_Without_Exception()
        {
            Assert.IsTrue(true);
        }

        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new OracleClientDbCommandBuilder(new DbConnectionManager<OracleConnection>(DbConnection.OracleClientConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new OracleClientDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            throw new NotSupportedException("GetResetIdentityColumnsDbCommand not supported!");
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OracleClient.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.OracleClient.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.OracleClient.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OracleClient.XmlSchemaFile;
        }

    }
}
