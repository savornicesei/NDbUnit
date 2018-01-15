/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
namespace NDbUnit.Core
{
    /// <summary>
    ///	The database operation to perform.
    /// </summary>
    public enum DbOperationFlag
    {
        /// <summary>No operation.</summary>
        None,
        /// <summary>Insert rows into a set of database tables.</summary>
        Insert,
        /// <summary>Insert rows into a set of database tables.  Allow identity 
        /// inserts to occur.</summary>
        InsertIdentity,
        /// <summary>Delete rows from a set of database tables.</summary>
        Delete,
        /// <summary>Delete all rows from a set of database tables.</summary>
        DeleteAll,
        /// <summary>Update rows in a set of database tables.</summary>
        Update,
        /// <summary>Refresh rows in a set of database tables.  Rows that exist 
        /// in the database are updated.  Rows that don't exist are inserted.</summary>
        Refresh,
        /// <summary>Composite operation of DeleteAll and Insert.</summary>
        CleanInsert,
        /// <summary>Composite operation of DeleteAll and InsertIdentity.</summary>
        CleanInsertIdentity
    }
}