/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NUnit.Framework;
using System;
using System.Data;
using System.IO;

namespace NDbUnit.Test.Common
{
    public abstract class DbOperationTestBase
    {
        protected IDbCommandBuilder _commandBuilder;

        protected IDbOperation _dbOperation;

        protected DataSet _dsData;

        protected string _xmlFile;

        [OneTimeSetUp]
        public void _FixtureSetUp()
        {
            _commandBuilder = GetCommandBuilder();

            string xmlSchemaFile = GetXmlSchemaFilename();
            _xmlFile = GetXmlFilename();

            _commandBuilder.BuildCommands(xmlSchemaFile);

            DataSet dsSchema = _commandBuilder.GetSchema();
            _dsData = dsSchema.Clone();
            _dsData.ReadXml(ReadOnlyStreamFromFilename(_xmlFile));

            _dbOperation = GetDbOperation();
        }

        [SetUp]
        public void _SetUp()
        {
            _commandBuilder.Connection.Open();
        }

        [TearDown]
        public void _TearDown()
        {
            _commandBuilder.Connection.Close();
        }

        [Test]
        public void All_Test_Xml_Files_Comply_With_Test_Xsd_Schema()
        {
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            ds.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            DataSet dsRefresh = new DataSet();
            dsRefresh.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            dsRefresh.ReadXml(ReadOnlyStreamFromFilename(GetXmlRefeshFilename()));

            DataSet dsModify = new DataSet();
            dsModify.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            dsModify.ReadXml(ReadOnlyStreamFromFilename(GetXmlRefeshFilename()));

        }

        [Test]
        public void Delete_Executes_Without_Exception()
        {
            using (var sqlTransaction = _commandBuilder.Connection.BeginTransaction())
            {
                try
                {
                    _dbOperation.Delete(_dsData, _commandBuilder, sqlTransaction);
                    sqlTransaction.Commit();
                }
                catch (Exception)
                {
                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Rollback();
                    }

                    throw;
                }
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void DeleteAll_Executes_Without_Exception()
        {
            using (var sqlTransaction = _commandBuilder.Connection.BeginTransaction())
            {
                try
                {

                    _dbOperation.DeleteAll(_dsData, _commandBuilder, sqlTransaction);
                    sqlTransaction.Commit();
                }
                catch (Exception)
                {
                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Rollback();
                    }

                    throw;
                }
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void Insert_Executes_Without_Exception()
        {
            ResetIdentityColumns();

            DeleteAll_Executes_Without_Exception();

            using (var sqlTransaction = _commandBuilder.Connection.BeginTransaction())
            {
                try
                {
                    _dbOperation.Insert(_dsData, _commandBuilder, sqlTransaction);
                    sqlTransaction.Commit();
                }
                catch (Exception)
                {
                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Rollback();
                    }

                    throw;
                }
            }
            Assert.IsTrue(true);
        }

        [Test]
        public virtual void InsertIdentity_Executes_Without_Exception()
        {
            DeleteAll_Executes_Without_Exception();

            using (var sqlTransaction = _commandBuilder.Connection.BeginTransaction())
            {
                try
                {
                    _dbOperation.InsertIdentity(_dsData, _commandBuilder, sqlTransaction);
                    sqlTransaction.Commit();
                }
                catch (Exception)
                {
                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Rollback();
                    }

                    throw;
                }
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void Refresh_Executes_Without_Exception()
        {
            DeleteAll_Executes_Without_Exception();
            Insert_Executes_Without_Exception();

            DataSet dsSchema = _commandBuilder.GetSchema();
            DataSet ds = dsSchema.Clone();
            ds.ReadXml(ReadOnlyStreamFromFilename(GetXmlRefeshFilename()));

            using (var sqlTransaction = _commandBuilder.Connection.BeginTransaction())
            {
                try
                {
                    _dbOperation.Refresh(ds, _commandBuilder, sqlTransaction);
                    sqlTransaction.Commit();
                }
                catch (Exception)
                {
                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Rollback();
                    }

                    throw;
                }
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void Update_Executes_Without_Exception()
        {
            DeleteAll_Executes_Without_Exception();
            Insert_Executes_Without_Exception();

            DataSet dsSchema = _commandBuilder.GetSchema();
            DataSet ds = dsSchema.Clone();
            ds.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            using (var sqlTransaction = _commandBuilder.Connection.BeginTransaction())
            {
                try
                {
                    _dbOperation.Update(ds, _commandBuilder, sqlTransaction);
                    sqlTransaction.Commit();
                }
                catch (Exception)
                {
                    if (sqlTransaction != null)
                    {
                        sqlTransaction.Rollback();
                    }

                    throw;
                }
            }
            Assert.IsTrue(true);
        }

        protected abstract IDbCommandBuilder GetCommandBuilder();

        protected abstract IDbOperation GetDbOperation();

        protected abstract IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column);

        protected abstract string GetXmlFilename();

        protected abstract string GetXmlModifyFilename();

        protected abstract string GetXmlRefeshFilename();

        protected abstract string GetXmlSchemaFilename();

        protected void ResetIdentityColumns()
        {
            IDbTransaction sqlTransaction = null;
            try
            {
                DataSet dsSchema = _commandBuilder.GetSchema();
                sqlTransaction = _commandBuilder.Connection.BeginTransaction();
                foreach (DataTable table in dsSchema.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.AutoIncrement)
                        {
                            IDbCommand sqlCommand = GetResetIdentityColumnsDbCommand(table, column);
                            sqlCommand.Transaction = sqlTransaction;
                            if (sqlCommand != null)
                                sqlCommand.ExecuteNonQuery();

                            break;
                        }
                    }
                }
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw;
            }
        }

        private FileStream ReadOnlyStreamFromFilename(string filename)
        {
            return new FileStream(filename, FileMode.Open, FileAccess.Read);
        }

    }
}