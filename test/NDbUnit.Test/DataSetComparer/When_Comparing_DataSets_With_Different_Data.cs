/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.DataSetComparer
{
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    public class When_Comparing_DataSets_With_Different_Data : DataSetComparerTestBase
    {
        [Test]
        public void CanReportNoMatch()
        {
            var firstDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd", @"Xml\DataSetComparer\FirstDataToCompare.xml");
            var secondDataSet = BuildDataSet(@"Xml\DataSetComparer\FirstDataSetToCompare.xsd", @"Xml\DataSetComparer\DifferingDataToCompare.xml");

            Assert.That(firstDataSet.HasTheSameDataAs(secondDataSet), Is.False);
        }
    }
}