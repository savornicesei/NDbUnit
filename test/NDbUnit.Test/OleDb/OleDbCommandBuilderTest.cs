﻿/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.OleDb;
using OleDbCommandBuilder = NDbUnit.OleDb.OleDbCommandBuilder;

namespace NDbUnit.Test.OleDb
{
    [Category(TestCategories.OleDbTests)]
    [TestFixture]
    class OleDbCommandBuilderTest : NDbUnit.Test.Common.DbCommandBuilderTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "dbo.User", "UserRole" 
                };
            }
        }

        public override IList<string> ExpectedDeleteAllCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM [Role]",
                    "DELETE FROM [dbo].[User]",
                    "DELETE FROM [UserRole]"
                };
            }
        }

        public override IList<string> ExpectedDeleteCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM [Role] WHERE [ID]=?",
                    "DELETE FROM [dbo].[User] WHERE [ID]=?",
                    "DELETE FROM [UserRole] WHERE [UserID]=? AND [RoleID]=?"
                };
            }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO [Role]([Name], [Description]) VALUES(?, ?)",
                    "INSERT INTO [dbo].[User]([FirstName], [LastName], [Age], [SupervisorID]) VALUES(?, ?, ?, ?)",
                    "INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(?, ?)"
                };

            }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO [Role]([ID], [Name], [Description]) VALUES(?, ?, ?)",
                    "INSERT INTO [dbo].[User]([ID], [FirstName], [LastName], [Age], [SupervisorID]) VALUES(?, ?, ?, ?, ?)",
                    "INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(?, ?)"
                };
            }
        }

        public override IList<string> ExpectedSelectCommands
        {
            get
            {
                return new List<string>()
                {
                    "SELECT [ID], [Name], [Description] FROM [Role]",
                    "SELECT [ID], [FirstName], [LastName], [Age], [SupervisorID] FROM [dbo].[User]",
                    "SELECT [UserID], [RoleID] FROM [UserRole]"
                };
            }
        }

        public override IList<string> ExpectedUpdateCommands
        {
            get
            {
                return new List<string>()
                {
                    "UPDATE [Role] SET [Name]=?, [Description]=? WHERE [ID]=?",
                    "UPDATE [dbo].[User] SET [FirstName]=?, [LastName]=?, [Age]=?, [SupervisorID]=? WHERE [ID]=?",
                    "UPDATE [UserRole] SET [UserID]=?, [RoleID]=? WHERE [UserID]=? AND [RoleID]=?"
                };
            }
        }

        protected override IDbCommandBuilder GetDbCommandBuilder()
        {
            return new OleDbCommandBuilder(new DbConnectionManager<OleDbConnection>(DbConnection.OleDbConnectionString));
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OleDb.XmlSchemaFile;
        }

    }
}
