/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.IO;

namespace NDbUnit.Test
{
    public class XmlTestFiles
    {

        public abstract class XmlTestFilesBase
        {
            protected const string _defaultXmlFilename = "User.xml";

            protected const string _defaultXmlModFilename = "UserMod.xml";

            protected const string _defaultXmlRefreshFilename = "UserRefresh.xml";

            protected const string _defaultXmlSchemaFilename = "UserDS.xsd";

            protected XmlTestFilesBase() { }
        }

        public class Postgresql : XmlTestFilesBase
        {
            private static string _xmlPath = @"Xml\Postgresql\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }


        public class MySql : XmlTestFilesBase
        {
            private static string _xmlPath = @"Xml\MySql\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class SqlServer : XmlTestFilesBase
        {
            private static string xmlAppendFilename = "UserAppend.xml";

            private static string _xmlPath = @"Xml\SqlServer\";

            public static string XmlApppendFile
            {
                get { return Path.Combine(_xmlPath, xmlAppendFilename); }
            }

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlFileForSchemaPrefixTests
            {
                get { return Path.Combine(_xmlPath, "DataFileWithSchemaPrefixes.xml"); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

            public static string XmlSchemaFileForSchemaPrefixTests
            {
                get { return Path.Combine(_xmlPath, "SchemaWithSchemaPrefixes.xsd"); }
            }

        }

        public class SqlServerCe : XmlTestFilesBase
        {
            private static string _xmlPath = @"Xml\SqlServerCe\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class OleDb : XmlTestFilesBase
        {
            private static string _xmlPath = @"Xml\OleDb\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class Sqlite : XmlTestFilesBase
        {
            private static string _xmlPath = @"Xml\Sqlite\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

        public class OracleClient : XmlTestFilesBase
        {
            private static string _xmlPath = @"Xml\OracleClient\";

            public static string XmlFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlFilename); }
            }

            public static string XmlModFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlModFilename); }
            }

            public static string XmlRefreshFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlRefreshFilename); }
            }

            public static string XmlSchemaFile
            {
                get { return Path.Combine(_xmlPath, _defaultXmlSchemaFilename); }
            }

        }

    }
}
