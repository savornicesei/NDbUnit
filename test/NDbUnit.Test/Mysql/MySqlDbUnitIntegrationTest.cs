/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Core.MySqlClient;
using NUnit.Framework;

namespace NDbUnit.Test.Mysql
{
    [Category(TestCategories.MySqlTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    public class MySqlDbUnitIntegrationTest : IntegationTestBase
    {
        protected override INDbUnitTest GetNDbUnitTest()
        {
            return new MySqlDbUnitTest(DbConnection.MySqlConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.MySql.XmlFile;
        }

        protected override string GetXmlModFilename()
        {
            return XmlTestFiles.MySql.XmlModFile;
        }

        protected override string GetXmlRefreshFilename()
        {
            return XmlTestFiles.MySql.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.MySql.XmlSchemaFile;
        }

        [Test]
        public void Issue38_Test()
        {
            INDbUnitTest db = GetNDbUnitTest();
            db.ReadXmlSchema( @"Xml\MySql\DateAsPrimaryKey.xsd");
            db.ReadXml(@"Xml\MySql\DateAsPrimaryKey.xml");

            db.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);

        }


    }
}
