/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test
{
    [TestFixture]
    public class When_Initial_TableName_Contains_No_Escape_Characters_And_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = "schema.tablename";
            const string ESCAPED_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Single_Element_And_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = "tablename";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Single_Element_And_Extra_Leading_Delimeter
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = ".tablename";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Single_Element_And_Extra_Trailing_Delimeter
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = "tablename.";
            const string ESCAPED_TABLENAME = "[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }

    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Escape_Characters_And_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Original_TableName()
        {
            const string INITIAL_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }
    }



    [TestFixture]
    public class When_Initial_TableName_Contains_No_Escape_Characters_And_No_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Original_TableName()
        {
            const string INITIAL_TABLENAME = "schema.tablename";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, string.Empty, string.Empty));
        }
    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Escape_Characters_And_No_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Original_TableName()
        {
            const string INITIAL_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(INITIAL_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, string.Empty, string.Empty));
        }
    }

    [TestFixture]
    public class When_Initial_TableName_Contains_Escape_Characters_For_A_Single_Element_And_Escape_Characters_Are_Provided
    {
        [Test]
        public void Returns_Properly_Escaped_TableName()
        {
            const string INITIAL_TABLENAME = "[schema].tablename";
            const string ESCAPED_TABLENAME = "[schema].[tablename]";

            Assert.AreEqual(ESCAPED_TABLENAME, TableNameHelper.FormatTableName(INITIAL_TABLENAME, "[", "]"));
        }
    }
}

