/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Configuration;

namespace NDbUnit.Test
{
    //FIXME: Database connections should be dynamically retrieved so that the tests can be run on multiple versions of the same database
    public static class DbConnection
    {
        public static string PostgresqlConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["PostgresqlConnectionString"].ConnectionString;
            }
        }
        public static string MySqlConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString;
            }
        }

        public static string OleDbConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["OleDbConnectionString"].ConnectionString;
            }
        }

        public static string SqlCeConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlCeConnectionString"].ConnectionString;
            }
        }

        public static string SqlConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            }
        }

        public static string SqlScriptTestsConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlScriptTestsConnectionString"].ConnectionString;
            }
        }

        public static string SqlLiteConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlLiteConnectionString"].ConnectionString;
            }
        }

        public static string SqlLiteInMemConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SqlLiteInMemConnectionString"].ConnectionString;
            }
        }

        public static string OracleClientConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["OracleClientConnectionString"].ConnectionString;
            }
        }
    }
}
