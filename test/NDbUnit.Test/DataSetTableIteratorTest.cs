/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;

namespace NDbUnit.Test
{
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    public class DataSetTableIteratorTest
    {
        private DataSetTableIterator _defaultIterator;

        private DataSetTableIterator _forwardIterator;

        private DataSetTableIterator _reverseIterator;

        [OneTimeSetUp]
        public void _OneTimeSetUp()
        {
            DataSet dataSet = new DataSet();

            dataSet.ReadXmlSchema(XmlTestFiles.SqlServer.XmlSchemaFile);

            _defaultIterator = new DataSetTableIterator(dataSet);
            _forwardIterator = new DataSetTableIterator(dataSet, false);
            _reverseIterator = new DataSetTableIterator(dataSet, true);
        }

        [Test]
        public void Forward_Iterator_Return_Tables_In_Proper_Order()
        {
            List<string> items = new List<string>();

            foreach (DataTable dt in _forwardIterator)
            {
                items.Add(dt.TableName);
            }

            Assert.AreEqual("Role", items[0]);
            Assert.AreEqual("dbo.User", items[1]);
            Assert.AreEqual("UserRole", items[2]);
        }

        [Test]
        public void Default_Iterator_is_Same_as_Forward_Iterator()
        {
            Assert.That(_forwardIterator, Is.EquivalentTo(_defaultIterator));
        }

        [Test]
        public void Reverse_Iterator_Is_Inverse_of_Forward_Iterator()
        {
            List<DataTable> forwardItems = new List<DataTable>();

            foreach (DataTable dt in _forwardIterator)
            {
                forwardItems.Add(dt);
            }

            forwardItems.Reverse();

            Assert.That(forwardItems, Is.EquivalentTo(_reverseIterator));
        }

    }
}
