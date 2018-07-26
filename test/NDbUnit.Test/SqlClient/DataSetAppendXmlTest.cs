/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NUnit.Framework;

namespace NDbUnit.Test.SqlClient
{
    [Category(TestCategories.SqlServerTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    public class DataSetAppendXmlTest
    {
        [Test]
        public void DataSet_Contains_Both_Appended_and_New_Records()
        {
            var db = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlConnectionString);
            db.ReadXmlSchema(XmlTestFiles.SqlServer.XmlSchemaFile);
            db.ReadXml(XmlTestFiles.SqlServer.XmlFile);

            db.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);

            var preAppendDataset = db.GetDataSetFromDb();

            Assert.AreEqual(2, preAppendDataset.Tables["dbo.User"].Rows.Count);
            Assert.AreEqual(2, preAppendDataset.Tables["Role"].Rows.Count);

            db.AppendXml(XmlTestFiles.SqlServer.XmlApppendFile);

            db.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);

            var postAppendDataset = db.GetDataSetFromDb();

            Assert.AreEqual(4, postAppendDataset.Tables["dbo.User"].Rows.Count);
            Assert.AreEqual(4, postAppendDataset.Tables["Role"].Rows.Count);

        }
    }

}
