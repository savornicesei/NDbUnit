/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.OracleClient;
using NDbUnit.Test.Common;
using NUnit.Framework;
#if MONO
using System.Data.OracleClient;
#else
using Oracle.ManagedDataAccess.Client;
#endif
using System.Collections.Generic;

namespace NDbUnit.Test.OracleClient
{
    [Category(TestCategories.OracleTests)]
    public class OracleClientDbCommandBuilderTest : DbCommandBuilderTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "USER", "USERROLE", "ROLE"
                };
            }
        }

        public override IList<string> ExpectedDeleteAllCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM \"USER\"",
                    "DELETE FROM \"USERROLE\"", 
                    "DELETE FROM \"ROLE\""
                };
            }
        }

        public override IList<string> ExpectedDeleteCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM \"USER\" WHERE \"ID\"=:p1",
                    "DELETE FROM \"USERROLE\" WHERE \"USERID\"=:p1 AND \"ROLEID\"=:p2",
                    "DELETE FROM \"ROLE\" WHERE \"ID\"=:p1"
                };
            }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO \"USER\"(\"ID\", \"FIRSTNAME\", \"LASTNAME\", \"AGE\", \"SUPERVISORID\") VALUES(:p1, :p2, :p3, :p4, :p5)", 
                    "INSERT INTO \"USERROLE\"(\"USERID\", \"ROLEID\") VALUES(:p1, :p2)", 
                    "INSERT INTO \"ROLE\"(\"ID\", \"NAME\", \"DESCRIPTION\") VALUES(:p1, :p2, :p3)"
                };

            }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO \"USER\"(\"ID\", \"FIRSTNAME\", \"LASTNAME\", \"AGE\", \"SUPERVISORID\") VALUES(:p1, :p2, :p3, :p4, :p5)", 
                    "INSERT INTO \"USERROLE\"(\"USERID\", \"ROLEID\") VALUES(:p1, :p2)", 
                    "INSERT INTO \"ROLE\"(\"ID\", \"NAME\", \"DESCRIPTION\") VALUES(:p1, :p2, :p3)"
                };
            }
        }

        public override IList<string> ExpectedSelectCommands
        {
            get
            {
                return new List<string>()
                {
                    "SELECT \"ID\", \"FIRSTNAME\", \"LASTNAME\", \"AGE\", \"SUPERVISORID\" FROM \"USER\"", 
                    "SELECT \"USERID\", \"ROLEID\" FROM \"USERROLE\"", 
                    "SELECT \"ID\", \"NAME\", \"DESCRIPTION\" FROM \"ROLE\""
                };
            }
        }

        public override IList<string> ExpectedUpdateCommands
        {
            get
            {
                return new List<string>()
                { 
                    "UPDATE \"USER\" SET \"FIRSTNAME\"=:p2, \"LASTNAME\"=:p3, \"AGE\"=:p4, \"SUPERVISORID\"=:p5 WHERE \"ID\"=:p1",
                    "UPDATE \"USERROLE\" SET \"USERID\"=:p2, \"ROLEID\"=:p4 WHERE \"USERID\"=:p1 AND \"ROLEID\"=:p3",
                    "UPDATE \"ROLE\" SET \"NAME\"=:p2, \"DESCRIPTION\"=:p3 WHERE \"ID\"=:p1"
                };
            }
        }

        protected override IDbCommandBuilder GetDbCommandBuilder()
        {
            return new OracleClientDbCommandBuilder(new DbConnectionManager<OracleConnection>(DbConnection.OracleClientConnectionString));
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OracleClient.XmlSchemaFile;
        }

    }
}
