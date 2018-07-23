/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.OleDb;
using NUnit.Framework;

namespace NDbUnit.Test.OleDb
{
    [Category(TestCategories.OleDbTests)]
    public class OleDbUnitIntegrationTest : IntegationTestBase
    {
        protected override NDbUnit.Core.INDbUnitTest GetNDbUnitTest()
        {
            return new OleDbUnitTest(DbConnection.OleDbConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OleDb.XmlFile;
        }

        protected override string GetXmlModFilename()
        {
            return XmlTestFiles.OleDb.XmlModFile;
        }

        protected override string GetXmlRefreshFilename()
        {
            return XmlTestFiles.OleDb.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OleDb.XmlSchemaFile;
        }

    }
}
