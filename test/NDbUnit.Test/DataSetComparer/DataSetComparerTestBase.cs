/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NUnit.Framework;
using System.Data;

namespace NDbUnit.Test.DataSetComparer
{
    [TestFixture]
    public abstract class DataSetComparerTestBase
    {

        public DataSetComparerTestBase()
        {
            Core.DataSetComparer.MAX_DIFFERENCES_BEFORE_ABORT = 20;
        }

        public DataSet BuildDataSet(string schemaFile, string dataFile = null)
        {
            var dataSet = new DataSet();
            dataSet.ReadXmlSchema(StreamHelper.ReadOnlyStreamFromFilename(schemaFile));

            if (dataFile != null)
            {
                dataSet.ReadXml(StreamHelper.ReadOnlyStreamFromFilename(dataFile));
            }

            return dataSet;
        }
    }
}