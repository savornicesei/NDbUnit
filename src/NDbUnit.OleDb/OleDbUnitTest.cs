/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;
using System.Data.OleDb;

namespace NDbUnit.Core.OleDb
{
    /// <summary>
    /// The OleDb unit test data adapter.
    /// </summary>
    /// <example>
    /// <code>
    /// string connectionString = "Provider=SQLOLEDB;Data Source=V-AL-DIMEOLA\NETSDK;Initial Catalog=testdb;Integrated Security=SSPI;";
    /// OleDbUnitTest oleDbUnitTest = new OleDbUnitTest(connectionString);
    /// string xmlSchemaFile = "User.xsd";
    /// string xmlFile = "User.xml";
    ///	oleDbUnitTest.ReadXmlSchema(xmlSchemaFile);
    ///	oleDbUnitTest.ReadXml(xmlFile);
    ///	oleDbUnitTest.PerformDbOperation(DbOperation.CleanInsertIdentity);
    /// </code>
    /// <seealso cref="INDbUnitTest"/>
    /// </example>
    public class OleDbUnitTest : NDbUnitTest<OleDbConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OleDbUnitTest"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string 
        /// used to open the database.
        /// <seealso cref="System.Data.IDbConnection"/></param>

        public OleDbUnitTest(OleDbConnection connection)
            : base(connection)
        {
        }

        public OleDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Gets or sets the OLE database type.  The default value for an 
        /// instance of an object is <see cref="OleDb.OleDbType.NoDb" />.
        /// </summary>
        public OleDbType OleOleDbType
        {
            get
            {
                return ((OleDbOperation)GetDbOperation()).OleOleDbType;
            }

            set
            {
                ((OleDbOperation)GetDbOperation()).OleOleDbType = value;
            }
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new OleDbDataAdapter((OleDbCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<OleDbConnection> connectionManager)
        {
            return new OleDbCommandBuilder(connectionManager);
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new OleDbOperation();
        }

    }
}
