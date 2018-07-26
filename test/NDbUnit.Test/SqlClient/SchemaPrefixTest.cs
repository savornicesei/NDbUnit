/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NUnit.Framework;
using System;

namespace NDbUnit.Test.SqlClient
{
    [Category(TestCategories.SqlServerTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    public class SchemaPrefixTest
    {
        [Test]
        public void Can_Perform_CleanInsertUpdate_Operation_Without_Exception_When_Schema_Has_Prefix()
        {
            try
            {
                var db = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlConnectionString);
                db.ReadXmlSchema(XmlTestFiles.SqlServer.XmlSchemaFileForSchemaPrefixTests);
                db.ReadXml(XmlTestFiles.SqlServer.XmlFileForSchemaPrefixTests);

                db.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);
            }
            catch (Exception)
            {
                Assert.Fail("Operation not successful when using tables with Schema Prefixes.");
            }

        }
    }
}