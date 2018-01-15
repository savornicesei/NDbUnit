/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.IO;

namespace NDbUnit.Core
{
    public class OperationEventArgs : EventArgs
    {
        public IDbTransaction DbTransaction { get; set; }
    }

    public delegate void PreOperationEvent(object sender, OperationEventArgs args);

    public delegate void PostOperationEvent(object sender, OperationEventArgs args);

    /// <summary>
    /// The base class implementation of all NDbUnit unit test data adapters.
    /// </summary>
    public abstract class NDbUnitTest<TDbConnection> : INDbUnitTest where TDbConnection : class, IDbConnection, new()
    {
        private DataSet _dataSet;

        private IDbCommandBuilder _dbCommandBuilder;

        private readonly IDbOperation _dbOperation;

        private bool _initialized;

        protected DbConnectionManager<TDbConnection> ConnectionManager;

        protected ScriptManager ScriptManager;

        private string _xmlFile = "";

        private string _xmlSchemaFile = "";

        private const string SCHEMA_NAMESPACE_PREFIX = "http://tempuri.org/";

        public event PostOperationEvent PostOperation;

        public event PreOperationEvent PreOperation;

        protected NDbUnitTest(string connectionString)
        {
            ConnectionManager = new DbConnectionManager<TDbConnection>(connectionString);
            _dbOperation = CreateDbOperation();

        }

        protected NDbUnitTest(TDbConnection connection)
        {
            ConnectionManager = new DbConnectionManager<TDbConnection>(connection);
            _dbOperation = CreateDbOperation();
        }

        public int CommandTimeOut { get; set; }

        protected virtual DataSet DS
        {
            get { return _dataSet; }
        }

        public ScriptManager Scripts
        {
            get
            {
                if (ScriptManager == null)
                    ScriptManager = new ScriptManager(new FileSystemService());

                return ScriptManager;
            }
        }

        public void AppendXml(Stream xml)
        {
            DoReadXml(xml, true);
        }

        public void AppendXml(string xmlFile)
        {
            DoReadXml(xmlFile, true);
        }

        //Todo: remove method at some point
        public DataSet CopyDataSet()
        {
            checkInitialized();
            return _dataSet.Copy();
        }

        //Todo: remove method at some point  
        public DataSet CopySchema()
        {
            checkInitialized();
            return _dataSet.Clone();
        }

        public virtual void ExecuteScripts()
        {
            var connection = ConnectionManager.GetConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            foreach (string ddlText in ScriptManager.ScriptContents)
            {
                IDbCommand command = connection.CreateCommand();
                command.CommandText = ddlText;
                command.ExecuteNonQuery();
            }

            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        public DataSet GetDataSetFromDb(StringCollection tableNames)
        {
            checkInitialized();

            IDbCommandBuilder dbCommandBuilder = GetDbCommandBuilder();
            if (null == tableNames)
            {
                tableNames = new StringCollection();
                foreach (DataTable dt in _dataSet.Tables)
                {
                    tableNames.Add(dt.TableName);
                }
            }

            IDbConnection dbConnection = dbCommandBuilder.Connection;
            try
            {
                dbConnection.Open();
                DataSet dsToFill = _dataSet.Clone();

                dsToFill.EnforceConstraints = false;

                foreach (string tableName in tableNames)
                {
                    OnGetDataSetFromDb(tableName, ref dsToFill, dbConnection);
                }

                dsToFill.EnforceConstraints = true;

                return dsToFill;
            }
            finally
            {
                if (ConnectionState.Open == dbConnection.State)
                {
                    dbConnection.Close();
                }
            }
        }

        public DataSet GetDataSetFromDb()
        {
            return GetDataSetFromDb(null);
        }

        public void PerformDbOperation(DbOperationFlag dbOperationFlag)
        {
            checkInitialized();

            if (dbOperationFlag == DbOperationFlag.None)
            {
                return;
            }

            IDbCommandBuilder dbCommandBuilder = GetDbCommandBuilder();
            IDbOperation dbOperation = GetDbOperation();

            IDbTransaction dbTransaction = null;
            IDbConnection dbConnection = dbCommandBuilder.Connection;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }
                dbTransaction = dbConnection.BeginTransaction();

                OperationEventArgs args = new OperationEventArgs();
                args.DbTransaction = dbTransaction;

                if (null != PreOperation)
                {
                    PreOperation(this, args);
                }

                switch (dbOperationFlag)
                {
                    case DbOperationFlag.Insert:
                        {
                            dbOperation.Insert(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.InsertIdentity:
                        {
                            dbOperation.InsertIdentity(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.Delete:
                        {
                            dbOperation.Delete(_dataSet, dbCommandBuilder, dbTransaction);

                            break;
                        }
                    case DbOperationFlag.DeleteAll:
                        {
                            dbOperation.DeleteAll(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.Refresh:
                        {
                            dbOperation.Refresh(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.Update:
                        {
                            dbOperation.Update(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.CleanInsert:
                        {
                            dbOperation.DeleteAll(_dataSet, dbCommandBuilder, dbTransaction);
                            dbOperation.Insert(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.CleanInsertIdentity:
                        {
                            dbOperation.DeleteAll(_dataSet, dbCommandBuilder, dbTransaction);
                            dbOperation.InsertIdentity(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                }

                if (null != PostOperation)
                {
                    PostOperation(this, args);
                }

                dbTransaction.Commit();
            }
            catch (Exception)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Dispose();
                }

                //only close and release the connection if not externally-managed
                if (!ConnectionManager.HasExternallyManagedConnection)
                {
                    if (ConnectionState.Open == dbConnection.State)
                    {
                        dbConnection.Close();
                    }

                    ConnectionManager.ReleaseConnection();
                }
            }
        }

        public void ReadXml(Stream xml)
        {
            DoReadXml(xml, false);
        }

        public void ReadXml(string xmlFile)
        {
            DoReadXml(xmlFile, false);
        }

        public void ReadXmlSchema(Stream xmlSchema)
        {
            IDbCommandBuilder dbCommandBuilder = GetDbCommandBuilder();
            dbCommandBuilder.BuildCommands(xmlSchema);

            DataSet dsSchema = dbCommandBuilder.GetSchema();

            ValidateNamespace(dsSchema);

            _dataSet = dsSchema.Clone();

            _initialized = true;
        }

        private void ValidateNamespace(DataSet dsSchema)
        {
            string expectedNamespace = string.Format("{0}.xsd", Path.Combine(SCHEMA_NAMESPACE_PREFIX, dsSchema.DataSetName));

            if (expectedNamespace != dsSchema.Namespace)
            {
                throw new ArgumentException(string.Format("The namespace in the file '{0}' is invalid.  Expected '{1}' but was '{2}'", _xmlSchemaFile, expectedNamespace, dsSchema.Namespace));
            }
        }

        public void ReadXmlSchema(string xmlSchemaFile)
        {
            if (string.IsNullOrEmpty(xmlSchemaFile))
            {
                throw new ArgumentException("Schema file cannot be null or empty", "xmlSchemaFile");
            }

            if (XmlSchemaFileHasNotYetBeenRead(xmlSchemaFile))
            {
                Stream stream = null;
                try
                {
                    stream = GetXmlSchemaFileStream(xmlSchemaFile);
                    ReadXmlSchema(stream);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                _xmlSchemaFile = xmlSchemaFile;
            }

            _initialized = true;
        }

        protected abstract IDbDataAdapter CreateDataAdapter(IDbCommand command);

        protected abstract IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<TDbConnection> connectionManager);

        protected abstract IDbOperation CreateDbOperation();

        protected IDbCommandBuilder GetDbCommandBuilder()
        {
            if (_dbCommandBuilder == null)
            {
                _dbCommandBuilder = CreateDbCommandBuilder(ConnectionManager);
            }

            return _dbCommandBuilder;
        }

        protected IDbOperation GetDbOperation()
        {
            return _dbOperation;
        }

        protected virtual FileStream GetXmlDataFileStream(string xmlFile)
        {
            return new FileStream(xmlFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        protected virtual FileStream GetXmlSchemaFileStream(string xmlSchemaFile)
        {
            return new FileStream(xmlSchemaFile, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        protected virtual void OnGetDataSetFromDb(string tableName, ref DataSet dsToFill, IDbConnection dbConnection)
        {
            IDbCommand selectCommand = GetDbCommandBuilder().GetSelectCommand(tableName);
            selectCommand.Connection = dbConnection;
            IDbDataAdapter adapter = CreateDataAdapter(selectCommand);
            ((DbDataAdapter)adapter).Fill(dsToFill, tableName);
        }

        private void checkInitialized()
        {
            if (!_initialized)
            {
                const string message = "INDbUnitTest.ReadXmlSchema(string) or INDbUnitTest.ReadXmlSchema(Stream) must be called successfully";
                throw new NDbUnitException(message);
            }
        }

        private void DoReadXml(Stream xml, bool appendData)
        {
            if (_dataSet == null)
            {
                throw new InvalidOperationException("You must first call ReadXmlSchema before reading in xml data.");
            }

            if (!appendData)
                _dataSet.Clear();

            _dataSet.ReadXml(xml);
        }

        private void DoReadXml(string xmlFile, bool appendData)
        {
            if (string.IsNullOrEmpty(xmlFile))
            {
                throw new ArgumentException("XML file cannot be null or empty", "xmlFile");
            }

            if (XmlDataFileHasNotYetBeenRead(xmlFile))
            {
                Stream stream = null;
                try
                {
                    stream = GetXmlDataFileStream(xmlFile);
                    DoReadXml(stream, appendData);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                _xmlFile = xmlFile;
            }
        }

        private bool XmlDataFileHasNotYetBeenRead(string xmlFile)
        {
            return _xmlFile.ToLower() != xmlFile.ToLower();
        }

        private bool XmlSchemaFileHasNotYetBeenRead(string xmlSchemaFile)
        {
            return _xmlSchemaFile.ToLower() != xmlSchemaFile.ToLower();
        }

        #region IDisposable interface

        // Flag: Has Dispose already been called?
        bool disposed = false;

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                ConnectionManager.ReleaseConnection();
            }

            // Free any unmanaged objects here.

            disposed = true;
        }

        #endregion
    }
}
