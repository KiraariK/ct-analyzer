using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMopener
{
    public class ArrayProcessor
    {
        private Array pArray { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inputArray">Any Array</param>
        public ArrayProcessor(Array inputArray)
        {
            pArray = inputArray;
        }

        /// <summary>
        /// Returns the number array strings
        /// </summary>
        /// <returns>number of strings in array</returns>
        public int GetStringsCount()
        {
            if (pArray == null)
                return 0;

            int arrayRank = pArray.Rank;
            if (arrayRank == 1)
                return pArray.Length;
            else
                return pArray.GetUpperBound(0) + 1;
        }

        /// <summary>
        /// Returns the number of array columns
        /// </summary>
        /// <returns>number of columns in array</returns>
        public int GetColumnsCount()
        {
            if (pArray == null)
                return 0;

            int arrayRank = pArray.Rank;
            if (arrayRank == 1)
                return 0;
            else
                return pArray.GetUpperBound(1) + 1;
        }

        /// <summary>
        /// Returns Int32 2D-array
        /// </summary>
        /// <returns>Int32 2D-array</returns>
        public int[,] GetInt32Array()
        {
            if (pArray.Rank < 2)
                return null;

            int strings = GetStringsCount();
            int columns = GetColumnsCount();
            int[,] result = new int[strings, columns];

            for (int i = 0; i < strings; i++)
                for (int j = 0; j < columns; j++)
                    result[i, j] = Convert.ToInt32(pArray.GetValue(i, j));

            return result;
        }

        // TODO: create the method to create the byte array of all images in certain electronic window
    }
}
