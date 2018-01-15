/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Data;
using System.Data.SqlServerCe;

namespace NDbUnit.Core.SqlServerCe
{
    public class SqlCeDbOperation : DbOperation
    {
        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new SqlCeDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new SqlCeCommand(cmdText);
        }
    }
}