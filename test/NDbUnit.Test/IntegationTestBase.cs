/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NUnit.Framework;
using System.Data;
using System.IO;

namespace NDbUnit.Test
{
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    public abstract class IntegationTestBase
    {
        //[NUnit.Framework.Ignore]
        [Test]
        public void Delete_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet expectedDataSet = BuildDataSet();

            database.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            database.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);
            database.PerformDbOperation(DbOperationFlag.DeleteAll);

            DataSet actualDataSet = database.GetDataSetFromDb();

            Assert.That(actualDataSet.HasTheSameDataAs(expectedDataSet));

            database.Dispose();
        }

        //[NUnit.Framework.Ignore]
        [Test]
        public void InsertIdentity_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet expectedDataSet = BuildDataSet(GetXmlFilename());

            database.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            database.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            DataSet actualDataSet = database.GetDataSetFromDb();

            Assert.That(actualDataSet.HasTheSameDataAs(expectedDataSet));
            
            database.Dispose();

        }

        //[NUnit.Framework.Ignore]
        [Test]
        public void Refresh_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet expectedDataSet = BuildDataSet(GetXmlFilename());

            database.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            database.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            database.ReadXml(GetXmlRefreshFilename());
            database.PerformDbOperation(DbOperationFlag.Refresh);

            DataSet actualDataSet = database.GetDataSetFromDb();

            Assert.That(actualDataSet.HasTheSameDataAs(expectedDataSet));
            
            database.Dispose();

        }

        //[NUnit.Framework.Ignore]
        [Test]
        public void Update_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet expectedDataSet = BuildDataSet(GetXmlModFilename());

            database.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            database.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            database.ReadXml(GetXmlModFilename());
            database.PerformDbOperation(DbOperationFlag.Update);

            DataSet actualDataSet = database.GetDataSetFromDb();

            Assert.That(actualDataSet.HasTheSameDataAs(expectedDataSet));
            
            database.Dispose();

        }

        protected abstract INDbUnitTest GetNDbUnitTest();

        protected abstract string GetXmlFilename();

        protected abstract string GetXmlModFilename();

        protected abstract string GetXmlRefreshFilename();

        protected abstract string GetXmlSchemaFilename();

        private FileStream ReadOnlyStreamFromFilename(string filename)
        {
            return new FileStream(filename, FileMode.Open, FileAccess.Read);
        }

        private DataSet BuildDataSet(string dataFilename = null)
        {
            var dataSet = new DataSet();
            dataSet.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));

            if (dataFilename != null)
            {
                dataSet.ReadXml(ReadOnlyStreamFromFilename(dataFilename));
            }

            return dataSet;
        }
    }
}
