/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using MySql.Data.MySqlClient;
using NDbUnit.Core;
using NDbUnit.Core.MySqlClient;
using NUnit.Framework;
using System.Collections.Generic;

namespace NDbUnit.Test.Mysql
{
    [Category(TestCategories.MySqlTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    class MySqlDbCommandBuilderTest : NDbUnit.Test.Common.DbCommandBuilderTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "User", "UserRole" 
                };
            }
        }

        public override IList<string> ExpectedDeleteAllCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM `Role`",
                    "DELETE FROM `User`",
                    "DELETE FROM `UserRole`"
                };
            }
        }

        public override IList<string> ExpectedDeleteCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM `Role` WHERE `ID`=?p1",
                    "DELETE FROM `User` WHERE `ID`=?p1",
                    "DELETE FROM `UserRole` WHERE `UserID`=?p1 AND `RoleID`=?p2"
                };
            }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO `Role`(`ID`, `Name`, `Description`) VALUES(?p1, ?p2, ?p3)",
                    "INSERT INTO `User`(`ID`, `FirstName`, `LastName`, `Age`, `SupervisorID`) VALUES(?p1, ?p2, ?p3, ?p4, ?p5)",
                    "INSERT INTO `UserRole`(`UserID`, `RoleID`) VALUES(?p1, ?p2)"
                };

            }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO `Role`(`ID`, `Name`, `Description`) VALUES(?p1, ?p2, ?p3)",
                    "INSERT INTO `User`(`ID`, `FirstName`, `LastName`, `Age`, `SupervisorID`) VALUES(?p1, ?p2, ?p3, ?p4, ?p5)",
                    "INSERT INTO `UserRole`(`UserID`, `RoleID`) VALUES(?p1, ?p2)"
                };
            }
        }

        public override IList<string> ExpectedSelectCommands
        {
            get
            {
                return new List<string>()
                {
                    "SELECT `ID`, `Name`, `Description` FROM `Role`",
                    "SELECT `ID`, `FirstName`, `LastName`, `Age`, `SupervisorID` FROM `User`",
                    "SELECT `UserID`, `RoleID` FROM `UserRole`"
                };
            }
        }

        public override IList<string> ExpectedUpdateCommands
        {
            get
            {
                return new List<string>()
                {
                    "UPDATE `Role` SET `Name`=?p2, `Description`=?p3 WHERE `ID`=?p1",
                    "UPDATE `User` SET `FirstName`=?p2, `LastName`=?p3, `Age`=?p4, `SupervisorID`=?p5 WHERE `ID`=?p1",
                    "UPDATE `UserRole` SET `UserID`=?p2, `RoleID`=?p4 WHERE `UserID`=?p1 AND `RoleID`=?p3"
                };
            }
        }

        protected override IDbCommandBuilder GetDbCommandBuilder()
        {
            return new MySqlDbCommandBuilder(new DbConnectionManager<MySqlConnection>(DbConnection.MySqlConnectionString));
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.MySql.XmlSchemaFile;
        }

    }
}
