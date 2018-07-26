﻿/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NDbUnit.Test.SqlClient
{
    [Category(TestCategories.SqlServerTests)]
    [Category(TestCategories.AllTests)]
    [Category(TestCategories.CrossPlatformTests)]
    [TestFixture]
    class SqlDbCommandBuilderTest : NDbUnit.Test.Common.DbCommandBuilderTestBase
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
                    "DELETE FROM [Role] WHERE [ID]=@p1",
                    "DELETE FROM [dbo].[User] WHERE [ID]=@p1",
                    "DELETE FROM [UserRole] WHERE [UserID]=@p1 AND [RoleID]=@p2"
                };
            }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO [Role]([Name], [Description]) VALUES(@p1, @p2)",
                    "INSERT INTO [dbo].[User]([FirstName], [LastName], [Age], [SupervisorID]) VALUES(@p1, @p2, @p3, @p4)",
                    "INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(@p1, @p2)"
                };

            }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO [Role]([ID], [Name], [Description]) VALUES(@p1, @p2, @p3)",
                    "INSERT INTO [dbo].[User]([ID], [FirstName], [LastName], [Age], [SupervisorID]) VALUES(@p1, @p2, @p3, @p4, @p5)",
                    "INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(@p1, @p2)"
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
                    "UPDATE [Role] SET [Name]=@p2, [Description]=@p3 WHERE [ID]=@p1",
                    "UPDATE [dbo].[User] SET [FirstName]=@p2, [LastName]=@p3, [Age]=@p4, [SupervisorID]=@p5 WHERE [ID]=@p1",
                    "UPDATE [UserRole] SET [UserID]=@p2, [RoleID]=@p4 WHERE [UserID]=@p1 AND [RoleID]=@p3"
                };
            }
        }

        protected override IDbCommandBuilder GetDbCommandBuilder()
        {
            return new SqlDbCommandBuilder(new DbConnectionManager<SqlConnection>(DbConnection.SqlConnectionString));
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServer.XmlSchemaFile;
        }

    }
}
