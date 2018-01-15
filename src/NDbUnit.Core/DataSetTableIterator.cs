/*
 * NDbUnit2
 * https://github.com/savornicesei/NDbUnit2
 * This source code is released under the Apache 2.0 License; see the accompanying license file.
 *
 */
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;


namespace NDbUnit.Core
{

    /// <summary>
    /// Class that builds an iterator for tables.  The order of the tables returned by the iterator
    /// is determined by the foreign keys between tables.
    /// </summary>
    public class DataSetTableIterator : CollectionBase, IEnumerable<DataTable>, IEnumerator
    {
        //TODO: Refactor.. the reverse sort is unnecessary now that constraints are dropped prior to inserts
        private int _index = 0;
        private readonly bool _iterateInReverse;


        /// <summary>
        /// Constructor that takes in a dataset to build iterator for the tables.
        /// </summary>
        /// <param name="dataSet">DataSet containing tables.</param>
        public DataSetTableIterator(DataSet dataSet)
        {
            _iterateInReverse = false;
            BuildTableList(dataSet);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetTableIterator"/> class.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="iterateInReverse">if set to <c>true</c> [iterate in reverse].</param>
        public DataSetTableIterator(DataSet dataSet, bool iterateInReverse)
        {
            _iterateInReverse = iterateInReverse;
            BuildTableList(dataSet);

            ReverseListIfNeeded();
        }

        /// <summary>
        /// Builds the table list.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        private void BuildTableList(DataSet dataSet)
        {
            AddTablesToList(dataSet.Tables);

            if (List.Count != dataSet.Tables.Count)
            {
                Debug.WriteLine("Iterator Contents:");
                foreach (var item in List)
                {
                    Debug.WriteLine(((DataTable)item).TableName);
                }

                Debug.WriteLine("DataSet Contents:");
                foreach (var item in dataSet.Tables)
                {
                    Debug.WriteLine(((DataTable)item).TableName);
                }
            }

            Trace.Assert(List.Count == dataSet.Tables.Count, "Dataset iterator did not add all tables to collection.");
        }


        /// <summary>
        /// Reverses the list if needed.
        /// </summary>
        private void ReverseListIfNeeded()
        {
            if (_iterateInReverse)
            {
                ArrayList tempList = new ArrayList();

                foreach (DataTable dataTable in List)
                {
                    tempList.Add(dataTable);
                }

                tempList.Reverse();

                List.Clear();

                foreach (DataTable dataTable in tempList)
                {
                    List.Add(dataTable);
                }

            }
        }

        /// <summary>
        /// Iterate over tables in dataset and at them to the internal list.
        /// </summary>
        /// <param name="tables">Collection of tables.</param>
        private void AddTablesToList(DataTableCollection tables)
        {
            foreach (DataTable table in tables)
            {
                List.Add(table);
            }
        }

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///<returns>
        ///An IEnumerator that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        IEnumerator<DataTable> IEnumerable<DataTable>.GetEnumerator()
        {
            foreach (DataTable table in InnerList)
            {
                yield return table;
            }
        }


        ///<summary>
        ///Advances the enumerator to the next element of the collection.
        ///</summary>
        ///<returns>
        ///true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        ///</returns>
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        ///<filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            if (_index < Count)
            {
                _index++;
                return true;
            }
            else
            {
                return false;
            }
        }

        ///<summary>
        ///Sets the enumerator to its initial position, which is before the first element in the collection.
        ///</summary>
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        ///<filterpriority>2</filterpriority>
        public void Reset()
        {
            _index = 0;
        }

        ///<summary>
        ///Gets the current element in the collection.
        ///</summary>
        ///<returns>
        ///The current element in the collection.
        ///</returns>
        ///<exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception>
        ///<filterpriority>2</filterpriority>
        object IEnumerator.Current
        {
            get { return List[_index]; }
        }
    }
}