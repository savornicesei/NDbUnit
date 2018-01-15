﻿/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */

using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NDbUnit.Core
{
    public static class DataSetComparer
    {
        //useful to increase this during debug/testing, default to 1 so that the first non-matching value will abort comparison and return a FALSE
        public static int MAX_DIFFERENCES_BEFORE_ABORT = 1;

        public static bool HasTheSameDataAs(this DataSet left, DataSet right)
        {
            var config = new ComparisonConfig();
            config.CustomComparers.Add(new NDbUnitDataSetComparer(RootComparerFactory.GetRootComparer()));
            config.CustomComparers.Add(new NDbUnitDataTableComparer(RootComparerFactory.GetRootComparer()));
            config.CustomComparers.Add(new NDbUnitDataRowCollectionComparer(RootComparerFactory.GetRootComparer()));

            config.MaxDifferences = MAX_DIFFERENCES_BEFORE_ABORT;

            var comparer = new CompareLogic(config);

            var result = comparer.Compare(left, right);

            if (!result.AreEqual)
            {
                Log(result.DifferencesString);
            }

            return result.AreEqual;
        }

        //TODO: wire up Common.Logging here...
        private static void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }

    /// <summary>
    /// Compare all tables and all rows in all tables
    /// </summary>
    internal class NDbUnitDataSetComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public NDbUnitDataSetComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are data sets
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDataset(type1) && TypeHelper.IsDataset(type2);
        }

        /// <summary>
        /// Compare two data sets
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            DataSet dataSet1 = parms.Object1 as DataSet;
            DataSet dataSet2 = parms.Object2 as DataSet;

            //This should never happen, null check happens one level up
            if (dataSet1 == null || dataSet2 == null)
                return;

            if (TableCountsDifferent(parms, dataSet2, dataSet1)) return;

            CompareEachTable(parms, dataSet1, dataSet2);
        }

        private bool TableCountsDifferent(CompareParms parms, DataSet dataSet2, DataSet dataSet1)
        {
            if (dataSet1.Tables.Count != dataSet2.Tables.Count)
            {
                Difference difference = new Difference
                                            {
                                                ParentObject1 = new WeakReference(parms.ParentObject1),
                                                ParentObject2 = new WeakReference(parms.ParentObject2),
                                                PropertyName = parms.BreadCrumb,
                                                Object1Value = dataSet1.Tables.Count.ToString(CultureInfo.InvariantCulture),
                                                Object2Value = dataSet2.Tables.Count.ToString(CultureInfo.InvariantCulture),
                                                ChildPropertyName = "Tables.Count",
                                                Object1 = new WeakReference(parms.Object1),
                                                Object2 = new WeakReference(parms.Object2)
                                            };

                AddDifference(parms.Result, difference);

                if (parms.Result.ExceededDifferences)
                    return true;
            }
            return false;
        }

        private void CompareEachTable(CompareParms parms, DataSet dataSet1, DataSet dataSet2)
        {
            for (int i = 0; i < Math.Min(dataSet1.Tables.Count, dataSet2.Tables.Count); i++)
            {
                string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, "Tables", string.Empty,
                                                         dataSet1.Tables[i].TableName);

                CompareParms childParms = new CompareParms();
                childParms.Result = parms.Result;
                childParms.Config = parms.Config;
                childParms.BreadCrumb = currentBreadCrumb;
                childParms.ParentObject1 = dataSet1;
                childParms.ParentObject2 = dataSet2;
                childParms.Object1 = dataSet1.Tables[i];
                childParms.Object2 = dataSet2.Tables[i];

                RootComparer.Compare(childParms);

                if (parms.Result.ExceededDifferences)
                    return;
            }
        }
    }
    /// <summary>
    /// Compare all rows in a data table
    /// </summary>
    internal class NDbUnitDataTableComparer : BaseTypeComparer
    {
        /// <summary>
        /// Constructor that takes a root comparer
        /// </summary>
        /// <param name="rootComparer"></param>
        public NDbUnitDataTableComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        /// <summary>
        /// Returns true if both objects are of type DataTable
        /// </summary>
        /// <param name="type1">The type of the first object</param>
        /// <param name="type2">The type of the second object</param>
        /// <returns></returns>
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return TypeHelper.IsDataTable(type1) && TypeHelper.IsDataTable(type2);
        }

        /// <summary>
        /// Compare two data tables
        /// </summary>
        public override void CompareType(CompareParms parms)
        {
            DataTable dataTable1 = parms.Object1 as DataTable;
            DataTable dataTable2 = parms.Object2 as DataTable;

            //This should never happen, null check happens one level up
            if (dataTable1 == null || dataTable2 == null)
                return;

            //Only compare specific table names
            if (parms.Config.MembersToInclude.Count > 0 && !parms.Config.MembersToInclude.Contains(dataTable1.TableName))
                return;

            //If we should ignore it, skip it
            if (parms.Config.MembersToInclude.Count == 0 && parms.Config.MembersToIgnore.Contains(dataTable1.TableName))
                return;

            //There must be the same amount of rows in the data table
            if (dataTable1.Rows.Count != dataTable2.Rows.Count)
            {
                Difference difference = new Difference
                {
                    ParentObject1 = new WeakReference(parms.ParentObject1),
                    ParentObject2 = new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1Value = dataTable1.Rows.Count.ToString(CultureInfo.InvariantCulture),
                    Object2Value = dataTable2.Rows.Count.ToString(CultureInfo.InvariantCulture),
                    ChildPropertyName = "Rows.Count",
                    Object1 = new WeakReference(parms.Object1),
                    Object2 = new WeakReference(parms.Object2)
                };

                AddDifference(parms.Result, difference);

                if (parms.Result.ExceededDifferences)
                    return;
            }

            if (ColumnCountsDifferent(parms)) return;

            CompareRowCollections(parms);
        }

        private bool ColumnCountsDifferent(CompareParms parms)
        {
            DataTable dataTable1 = parms.Object1 as DataTable;
            DataTable dataTable2 = parms.Object2 as DataTable;

            if (dataTable1.Columns.Count != dataTable2.Columns.Count)
            {
                Difference difference = new Difference
                {
                    ParentObject1 = new WeakReference(parms.ParentObject1),
                    ParentObject2 = new WeakReference(parms.ParentObject2),
                    PropertyName = parms.BreadCrumb,
                    Object1Value = dataTable1.Columns.Count.ToString(CultureInfo.InvariantCulture),
                    Object2Value = dataTable2.Columns.Count.ToString(CultureInfo.InvariantCulture),
                    ChildPropertyName = "Columns.Count",
                    Object1 = new WeakReference(parms.Object1),
                    Object2 = new WeakReference(parms.Object2)
                };

                AddDifference(parms.Result, difference);

                if (parms.Result.ExceededDifferences)
                    return true;
            }
            return false;
        }

        private void CompareRowCollections(CompareParms parms)
        {
            DataTable dataTable1 = parms.Object1 as DataTable;
            DataTable dataTable2 = parms.Object2 as DataTable;

            if (null == dataTable1 || null == dataTable2)
                return;

            string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, "Rows");

            CompareParms childParms = new CompareParms
            {
                Result = parms.Result,
                Config = parms.Config,
                ParentObject1 = parms.Object1,
                ParentObject2 = parms.Object2,
                Object1 = dataTable1.Rows,
                Object2 = dataTable2.Rows,
                BreadCrumb = currentBreadCrumb,
            };


            RootComparer.Compare(childParms);
        }
    }

    internal class NDbUnitDataRowCollectionComparer : BaseTypeComparer
    {
        public NDbUnitDataRowCollectionComparer(RootComparer rootComparer)
            : base(rootComparer)
        {
        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            return type1 != null && type2 != null && type1 == typeof(DataRowCollection) &&
                   type2 == typeof(DataRowCollection);
        }

        public override void CompareType(CompareParms parms)
        {
            var dataRowCollection1 = parms.Object1 as DataRowCollection;
            var dataRowCollection2 = parms.Object2 as DataRowCollection;

            if (null == dataRowCollection1 || null == dataRowCollection2)
                return;

            for (int i = 0; i < Math.Min(dataRowCollection1.Count, dataRowCollection2.Count); i++)
            {
                if (!CollectionContainsMatchingRow(dataRowCollection1[i], dataRowCollection2.Cast<DataRow>()))
                {
                    Difference difference = new Difference
                    {
                        ParentObject1 = new WeakReference(parms.ParentObject1),
                        ParentObject2 = new WeakReference(parms.ParentObject2),
                        PropertyName = parms.BreadCrumb,
                        Object1Value = string.Format("Row Contents: [{0}]", PrettifyRowValues(dataRowCollection1[i])),
                        Object2Value = "[No Matching Row Found in Second DataRowCollection]",
                        ChildPropertyName = string.Format("[{0}]", i),
                        Object1 = new WeakReference(dataRowCollection1[i]),
                        Object2 = new WeakReference(dataRowCollection2[i])
                    };

                    AddDifference(parms.Result, difference);

                    if (parms.Result.ExceededDifferences)
                        return;
                }
            }
        }

        private string PrettifyRowValues(DataRow row)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                builder.Append(PrettifyRowValue(row.ItemArray[i]));

                if (i < row.ItemArray.Length - 1)
                {
                    builder.Append(", ");
                }
            }

            return builder.ToString();
        }

        private string PrettifyRowValue(object item)
        {
            return null == item || item is DBNull ? "<DBNull>" : item.ToString();
        }

        private bool CollectionContainsMatchingRow(DataRow rowToMatch, IEnumerable<DataRow> collectionToSearch)
        {
            return collectionToSearch.Any(candidate => RowsHaveSameData(rowToMatch, candidate));
        }

        private bool RowsHaveSameData(DataRow left, DataRow right)
        {
            //this instance of the comparer is intentionally independent from the main one
            // (i.e., it does not report its DIFFERENCEs, etc. up the chain) b/c at this level we only
            // care about a single match in a vacuum so the 'reporting' back up the chain would be meaningless noise
            var config = new ComparisonConfig { IgnoreCollectionOrder = true, CompareChildren = false };
            var comparer = new CompareLogic(config);

            var result = comparer.Compare(left, right);

            return result.AreEqual;
        }
    }
}