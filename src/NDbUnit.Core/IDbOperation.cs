/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;

namespace NDbUnit.Core
{
    public interface IDbOperation
    {
        void Insert(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void InsertIdentity(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void Delete(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void DeleteAll(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void Update(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
        void Refresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction);
    }
}