using System;
using System.Text;
using System.Collections.Generic;

namespace edu.neu.ccis.rasala
{
    /// <summary>
    /// Struct used to define an integer range by its start and length.
    /// 
    /// Both start and length will be forced to be >= 0.
    /// </summary>
    public struct Range
    {
        // private data
        private int _start;

        private int _length;


        /// <summary>
        /// Property for the start of the range.
        /// 
        /// Set method will force >= 0.
        /// </summary>
        public int start
        {
            get { return _start; }

            set { checkAndSet(value, out _start); }
        }


        /// <summary>
        /// Property for the length of the range.
        /// 
        /// Set method will force >= 0.
        /// </summary>
        public int length
        {
            get { return _length; }

            set { checkAndSet(value, out _length); }
        }


        /// <summary>
        /// Property for limit of the range = start + length.
        /// 
        /// No set method.
        /// </summary>
        public int limit
        {
            get { return (_start + _length); }
        }


        /// <summary>
        /// Struct constructor for Range.
        /// 
        /// Constructs a range with the given start and length
        /// but will force these to be >= 0.
        /// </summary>
        /// <param name="start">Desired start.</param>
        /// <param name="length">Desired length.</param>
        public Range(int start, int length)
        {
            checkAndSet(start, out this._start);
            checkAndSet(length, out this._length);
        }


        /// <summary>
        /// Returns string: start, limit, length\n
        /// </summary>
        public override string ToString()
        {
            return start + ", " + limit + ", " + length + "\n";
        }


        public static string ToString(List<Range> list)
        {
            if (StringTools.IsTrivial(list))
                return "";

            StringBuilder builder = new StringBuilder();

            foreach (Range range in list)
                builder.Append(range.ToString());

            return builder.ToString();
        }


        /// <summary>
        /// Private method to assign an int value
        /// but force the assigned value to be >= 0.
        /// </summary>
        /// <param name="val">The value to attempt to assign.</param>
        /// <param name="x">The out parameter that will be assigned.</param>
        private static void checkAndSet(int val, out int x)
        {
            x = val;

            if (x < 0)
                x = 0;
        }


        /// <summary>
        /// Returns a nullable Range
        /// that is null if a and b do not intersect
        /// and is the intersection Range if they do.
        /// </summary>
        public static Range? Intersect(Range a, Range b)
        {
            int start = a.start <= b.start
                ? b.start
                : a.start;

            int limit = a.limit >= b.limit
                ? b.limit
                : a.limit;

            if (start <= limit)
                return new Range(start, limit - start);
            else
                return null;
        }
    }
}
