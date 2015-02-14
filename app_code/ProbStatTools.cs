using System;

namespace edu.neu.ccis.rasala
{

    /**
     * Class <code>ProbStatTools</code> collects static methods that
     * are useful for randomization, probability, and statistics.
     *
     * This class is adapted to C# from the Java Power Tools.
     */
    public class ProbStatTools
    {

        /** Prevent instantiation. */
        private ProbStatTools() { }


        private static Random randomGenerator = new Random();


        /**
         * Return a random int r in the range min <= r <= max.
         * Note that the range is inclusive on both ends.
         * 
         * If max equals Int32.MaxValue, then the return value
         * can never equal max due to limitations in the .NET
         * Random class that is used in implementation.
         * 
         * If min > max, then the values will be swapped prior
         * to finding the random number.
         */
        public static int randomInt(int min, int max)
        {
            if (min > max)
            {
                int a = min;
                min = max;
                max = a;
            }

            if (max < Int32.MaxValue)
                max += 1;

            return randomGenerator.Next(min, max);
        }


        /**
         * <p>Returns an array with n integers in sequence:
         * 0, 1, etc, up to (n-1).</p>
         *
         * <p>If n is less than or equal to zero, then returns an
         * empty array of integers.</p>
         */
        public static int[] integerSequence(int n)
        {
            return integerSequence(n, 0);
        }


        /**
         * <p>Returns an array with n integers in sequence:
         * start, (start+1), etc, up to (start+n-1).</p>
         *
         * <p>If n is less than or equal to zero, then returns an
         * empty array of integers.</p>
         */
        public static int[] integerSequence(int n, int start)
        {
            if (n <= 0)
                return new int[0];

            int[] result = new int[n];

            for (int i = 0; i < n; i++)
                result[i] = start + i;

            return result;
        }


        /**
         * <p>Returns an array whose contents is a random permutation
         * of the integers from 0 to (n-1).</p>
         *
         * <p>If n is less than or equal to zero,
         * then returns an empty array of integers.</p>
         */
        public static int[] randomPermutation(int n)
        {
            return randomPermutation(n, 0);
        }


        /**
         * <p>Returns an array whose contents is a random permutation
         * of the integers from start to (start+n-1).</p>
         *
         * <p>If n is less than or equal to zero,
         * then returns an empty array of integers.</p>
         */
        public static int[] randomPermutation(int n, int start)
        {
            int[] data = integerSequence(n, start);
            randomPermutationInSitu(data);
            return data;
        }


        /**
         * <p>Returns an array whose contents is a random permutation
         * of the contents in the data array.</p>
         *
         * <p>The data array is unchanged.</p>
         *
         * <p>If the data array is <code>null</code>,
         * then returns an empty array of integers.</p>
         */
        public static int[] randomPermutation(int[] data)
        {
            data = repeatData(data, 1);
            randomPermutationInSitu(data);
            return data;
        }


        /**
         * <p>Rearranges the contents of the data array using a
         * random permutation.</p>
         *
         * <p>If the data array is <code>null</code> or has length
         * less than 2, then does nothing.</p>
         */
        public static void randomPermutationInSitu(int[] data)
        {
            if (data == null)
                return;

            int n = data.Length;

            if (n < 2)
                return;

            int m = n - 1;

            for (int i = 0; i < m; i++)
            {
                int j = randomInt(i, m);

                if (j != i)
                {
                    int copy = data[j];
                    data[j] = data[i];
                    data[i] = copy;
                }
            }
        }


        /**
         * <p>Returns an array whose contents is a random even permutation
         * of the integers from 0 to (n-1).</p>
         *
         * <p>If n is less than or equal to zero,
         * then returns an empty array of integers.</p>
         */
        public static int[] randomEvenPermutation(int n)
        {
            return randomEvenPermutation(n, 0);
        }


        /**
         * <p>Returns an array whose contents is a random even permutation
         * of the integers from start to (start+n-1).</p>
         *
         * <p>If n is less than or equal to zero,
         * then returns an empty array of integers.</p>
         */
        public static int[] randomEvenPermutation(int n, int start)
        {
            int[] data = integerSequence(n, start);
            randomEvenPermutationInSitu(data);
            return data;
        }


        /**
         * <p>Returns an array whose contents is a random even permutation
         * of the contents in the data array.</p>
         *
         * <p>The data array is unchanged.</p>
         *
         * <p>If the data array is <code>null</code>,
         * then returns an empty array of integers.</p>
         */
        public static int[] randomEvenPermutation(int[] data)
        {
            data = repeatData(data, 1);
            randomEvenPermutationInSitu(data);
            return data;
        }


        /**
         * <p>Rearranges the contents of the given array using a
         * random even permutation.</p>
         *
         * <p>If the data array is <code>null</code> or has length
         * less than 3, then does nothing.</p>
         */
        public static void randomEvenPermutationInSitu(int[] data)
        {
            if (data == null)
                return;

            int n = data.Length;

            if (n < 3)
                return;

            int m = n - 1;
            int k = n - 2;

            int swaps = 0;

            for (int i = 0; i < k; i++)
            {
                int j = randomInt(i, m);

                if (j != i)
                {
                    int copy = data[j];
                    data[j] = data[i];
                    data[i] = copy;

                    swaps++;
                }
            }

            if ((swaps % 2) != 0)
            {
                int copy = data[m];
                data[m] = data[k];
                data[k] = copy;
            }
        }


        /**
         * <p>Returns the number of reversals of the array
         * viewed as a permutation of its data.</p>
         * 
         * <p>If the array is <code>null</code>
         * or contains repeated elements,
         * then returns -1 to signal an error.</p>
         * 
         * <p>If all elements in the array are distinct, then
         * counts the number k of element pairs (i,j) where:</p>
         * 
         * <pre>    i &lt; j    but    data[i] &gt; data[j]</pre>
         * 
         * <p>Then returns k.</p>
         */
        public static int reversals(int[] data)
        {
            if (data == null)
                return -1;

            int n = data.Length;

            if (n <= 1)
                return 0;

            int m = n - 1;
            int k = 0;

            for (int i = 0; i < m; i++)
                for (int j = (i + 1); j < n; j++)
                {
                    if (data[i] == data[j])
                        return -1;

                    if (data[i] > data[j])
                        k++;
                }

            return k;
        }


        /**
         * <p>Returns the sign of the array
         * viewed as a permutation of its data.</p>
         * 
         * <p>Computes <code>k = reversals(data)</code>.</p>
         * 
         * <p>If k is -1, returns 0 to signal an error.
         * Otherwise returns 1 if k is even and -1 if k is odd.</p>
         */
        public static int sign(int[] data)
        {
            int k = reversals(data);

            if (k == -1)
                return 0;

            if ((k % 2) == 0)
                return 1;
            else
                return -1;
        }


        /**
         * <p>Returns the next list in lexicographic order that
         * follows this list and has the same data elements.</p>
         * 
         * <p>Returns <code>null</code> if this list is the last
         * list in lexicographic order.</p>
         * 
         * <p>This method works correctly for lists that have
         * duplicate elements.</p>
         * 
         * <p>This method may be used as a tool when enumerating
         * the permutations of a list.</p>
         */
        public static int[] nextList(int[] data)
        {
            if (data == null)
                return null;

            int n = data.Length;

            if (n <= 1)
                return null;

            int m = n - 1;
            int i = m;

            while (i > 0)
            {
                if (data[i - 1] >= data[i])
                    i--;
                else
                    break;
            }

            if (i == 0)
                return null;

            int j = i - 1;

            int[] result = new int[n];

            // copy low elements
            for (int k = 0; k < j; k++)
            {
                result[k] = data[k];
            }

            // reverse copy high elements
            for (int k = i; k <= m; k++)
            {
                result[k] = data[m + i - k];
            }

            // find the first high element
            // larger than the element at j
            int p = i;

            while (result[p] <= data[j])
                p++;

            // swap p down to j
            result[j] = result[p];
            result[p] = data[j];

            return result;
        }


        /**
         * <p>Returns the previous list in lexicographic order that
         * precedes this list and has the same data elements.</p>
         * 
         * <p>Returns <code>null</code> if this list is the first
         * list in lexicographic order.</p>
         * 
         * <p>This method works correctly for lists that have
         * duplicate elements.</p>
         * 
         * <p>This method may be used as a tool when enumerating
         * the permutations of a list.</p>
         */
        public static int[] previousList(int[] data)
        {
            if (data == null)
                return null;

            int n = data.Length;

            if (n <= 1)
                return null;

            int m = n - 1;
            int i = m;

            while (i > 0)
            {
                if (data[i - 1] <= data[i])
                    i--;
                else
                    break;
            }

            if (i == 0)
                return null;

            int j = i - 1;

            int[] result = new int[n];

            // copy low elements
            for (int k = 0; k < j; k++)
            {
                result[k] = data[k];
            }

            // reverse copy high elements
            for (int k = i; k <= m; k++)
            {
                result[k] = data[m + i - k];
            }

            // find the first high element
            // smaller than the element at j
            int p = i;

            while (result[p] >= data[j])
                p++;

            // swap p down to j
            result[j] = result[p];
            result[p] = data[j];

            return result;
        }


        /**
         * <p>Returns an array of length k consisting of integers
         * chosen from 0 to (n-1) with repetition permitted.</p>
         *
         * <p>If n or k is less than or equal to zero, then returns
         * an empty array of integers.</p>
         */
        public static int[] selectWithRepetition(int n, int k)
        {
            if ((n <= 0) || (k <= 0))
                return new int[0];

            return selectWithRepetition(integerSequence(n), k);
        }


        /**
         * <p>Selects count items from 0 to (size-1), repeats these selected
         * items by the given repeat value, randomizes the data, and then
         * returns the data.  The selection stage will select distinct items
         * unless that is impossible because count is greater than size.</p>
         * 
         * <p>If size is less than 1, then size is set to 1.</p>
         * 
         * <p>If count is less than 1, then count is set to size.</p>
         * 
         * <p>If repeat is less than 2, then repeat is ignored.</p>
         */
        public static int[] randomSelection(int size, int count, int repeat)
        {
            if (size < 1)
                size = 1;

            if (count < 1)
                count = size;

            // construct random permutation or selection
            int[] data = (count == size)
                ? randomPermutation(size)
                : (count < size)
                    ? selectWithNoRepetition(size, count)
                    : selectWithRepetition(size, count);

            // repeat if need be and permute again
            if (repeat > 1)
            {
                data = repeatData(data, repeat);
                randomPermutationInSitu(data);
            }

            return data;
        }


        /**
         * <p>Returns an array of length k consisting of integers
         * chosen from 0 to (n-1) with no repetition permitted.</p>
         *
         * <p>If k is less than or equal to zero or greater than n,
         * then returns an empty array of integers.</p>
         */
        public static int[] selectWithNoRepetition(int n, int k)
        {
            if ((k <= 0) || (k > n))
                return new int[0];

            return selectWithNoRepetition(integerSequence(n), k);
        }


        /**
         * <p>Returns an array whose contents is a random selection of
         * k elements from the contents of the given data with
         * repetition of a selected index permitted.</p>
         *
         * <p>The data array is unchanged.</p>
         *
         * <p>If the data array is <code>null</code>
         * or if k is less than or equal to zero,
         * then returns an empty array of integers.</p>
         */
        public static int[] selectWithRepetition(int[] data, int k)
        {
            if ((data == null) || (k <= 0))
                return new int[0];

            int size = data.Length;

            int[] result = new int[k];

            for (int i = 0; i < k; i++)
            {
                int j = randomInt(0, size - 1);
                result[i] = data[j];
            }

            return result;
        }


        /**
         * <p>Returns an array whose contents is a random selection of
         * k elements from the contents of the given data with
         * no repetition of a selected index.</p>
         *
         * <p>The data array is unchanged.</p>
         *
         * <p>If the data array is <code>null</code>
         * or if k is less than or equal to zero
         * or if k is greater than the data array size,
         * then returns an empty array of integers.</p>
         */
        public static int[] selectWithNoRepetition(int[] data, int k)
        {
            if (data == null)
                return new int[0];

            int size = data.Length;

            if ((k <= 0) || (k > size))
                return new int[0];

            int[] temp = repeatData(data, 1);

            int[] result = new int[k];

            for (int i = 0; i < k; i++)
            {
                int j = randomInt(i, size - 1);

                int copy = temp[i];
                temp[i] = temp[j];
                temp[j] = copy;

                result[i] = temp[i];
            }

            return result;
        }


        /**
         * <p>Returns an int array that repeats the data in
         * the given array sequentially k times.
         *
         * <p>If the data array is <code>null</code> or
         * if k is less than or equal to zero,
         * then returns an empty array of integers.</p>
         *
         * <p>If k equals one, then copies the given data.</p>
         */
        public static int[] repeatData(int[] data, int k)
        {
            if ((data == null) || (k <= 0))
                return new int[0];

            int size1 = data.Length;
            int sizek = k * size1;

            int[] result = new int[sizek];

            for (int i = 0; i < size1; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    result[i + j * size1] = data[i];
                }
            }

            return result;
        }

    }
}