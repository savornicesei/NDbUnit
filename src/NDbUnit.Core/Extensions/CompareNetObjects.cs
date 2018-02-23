using System;
using System.Data;

namespace NDbUnit.Core.Extensions
{
    /// <summary>
    /// Required methods for making CompareNetObjects library compatible with netstandard2.0
    /// </summary>
    public static class CompareNetObjects
    {
#if NETSTANDARD

        /// <summary>
        /// Returns true if the type is a dataset.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if the specified type is dataset; otherwise, <c>false</c>.</returns>
        /// <remarks>Copied from https://github.com/GregFinzer/Compare-Net-Objects/blob/master/Compare-NET-Objects/TypeHelper.cs since CompareNetObjects is currently only targeting netstandard1.3 that has no System.Data</remarks>
        public static bool IsDataset(this Type type)
        {
            if (type == null)
                return false;

            return type == typeof(DataSet);
        }

        /// <summary>
        /// Returns true if the type is a data table.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [is data table] [the specified type]; otherwise, <c>false</c>.</returns>
        /// <remarks>Copied from https://github.com/GregFinzer/Compare-Net-Objects/blob/master/Compare-NET-Objects/TypeHelper.cs since CompareNetObjects is currently only targeting netstandard1.3 that has no System.Data</remarks>
        public static bool IsDataTable(this Type type)
        {
            if (type == null)
                return false;

            return type == typeof(DataTable);
        }
#endif
    }
}
