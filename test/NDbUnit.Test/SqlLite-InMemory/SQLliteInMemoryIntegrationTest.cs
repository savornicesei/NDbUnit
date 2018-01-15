/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Core.SqlLite;
using NUnit.Framework;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace NDbUnit.Test.SqlLite_InMemory
{
    [Category(TestCategories.SqliteTests)]
    [TestFixture]
    public class SQLliteInMemoryIntegrationTest
    {
        private SQLiteConnection _connection;

        [OneTimeSetUp]
        public void _OneTimeSetUp()
        {
            _connection = new SQLiteConnection(DbConnection.SqlLiteInMemConnectionString);
            ExecuteSchemaCreationScript();
        }

        [Test]
        public void Can_Get_Data_From_In_Memory_Instance()
        {
            var database = new SqlLiteDbUnitTest(_connection);

            database.ReadXmlSchema(XmlTestFiles.Sqlite.XmlSchemaFile);
            database.ReadXml(XmlTestFiles.Sqlite.XmlFile);

            database.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);

            var command = _connection.CreateCommand();
            command.CommandText = "Select * from [Role]";

            var results = command.ExecuteReader();
            
            Assert.IsTrue(results.HasRows);

            int recordCount = 0;

            while (results.Read())
            {
                recordCount++;
                Debug.WriteLine(results.GetString(1));
            }

            Assert.AreEqual(2, recordCount);

        }

        private void ExecuteSchemaCreationScript()
        {
            IDbCommand command = _connection.CreateCommand();
            command.CommandText = ReadTextFromFile(@"scripts\sqlite-testdb-create.sql");

            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            command.ExecuteNonQuery();

            command.CommandText = "Select * from Role";
            command.ExecuteReader();
        }

        private string ReadTextFromFile(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                return sr.ReadToEnd();
            }
        }

    }
}
