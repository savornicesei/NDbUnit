/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;
using System.IO;

namespace NDbUnit.Core
{
    public interface IDbCommandBuilder
    {
        int CommandTimeOutSeconds { get; set; }
        string QuotePrefix
        {
            get;
        }

        string QuoteSuffix
        {
            get;
        }

        IDbConnection Connection
        {
            get;
        }

        DataSet GetSchema();
        void BuildCommands(string xmlSchemaFile);
        void BuildCommands(Stream xmlSchema);
        IDbCommand GetSelectCommand(string tableName);
        IDbCommand GetInsertCommand(string tableName);
        IDbCommand GetInsertIdentityCommand(string tableName);
        IDbCommand GetDeleteCommand(string tableName);
        IDbCommand GetDeleteAllCommand(string tableName);
        IDbCommand GetUpdateCommand(string tableName);
    }
}