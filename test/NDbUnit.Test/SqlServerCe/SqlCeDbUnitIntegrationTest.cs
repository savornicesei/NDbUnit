/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.SqlServerCe;
using NUnit.Framework;

namespace NDbUnit.Test.SqlServerCe
{
    [Category(TestCategories.SqlServerCeTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.WindowsOnlyTests)]
    public class SqlCeDbUnitIntegrationTest : IntegationTestBase
    {
        protected override NDbUnit.Core.INDbUnitTest GetNDbUnitTest()
        {
            return new SqlCeDbUnitTest(DbConnection.SqlCeConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlFile;
        }

        protected override string GetXmlModFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlModFile;
        }

        protected override string GetXmlRefreshFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlSchemaFile;
        }

    }
}
