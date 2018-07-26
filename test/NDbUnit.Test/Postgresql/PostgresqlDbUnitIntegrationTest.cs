/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Postgresql;
using NUnit.Framework;

namespace NDbUnit.Test.Postgresql
{
    [Category(TestCategories.PostgresTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    public class PostgresqlDbUnitIntegrationTest : IntegationTestBase
    {
        protected override INDbUnitTest GetNDbUnitTest()
        {
            return new PostgresqlDbUnitTest(DbConnection.PostgresqlConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Postgresql.XmlFile;
        }

        protected override string GetXmlModFilename()
        {
            return XmlTestFiles.Postgresql.XmlModFile;
        }

        protected override string GetXmlRefreshFilename()
        {
            return XmlTestFiles.Postgresql.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Postgresql.XmlSchemaFile;
        }
    }
}