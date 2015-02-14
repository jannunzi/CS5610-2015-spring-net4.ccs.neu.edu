/*
    File: ~/app_code/RasalaExtensions.cs
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.IO;
using System.Collections.Generic;

namespace edu.neu.ccis.rasala
{
    public static class RasalaExtensions
    {
        /// <summary>
        /// Returns the earliest of the CreationTime and the
        /// LastWriteTime for the given FileInfo argument.
        /// 
        /// Note that if f is a FileInfo object, then one may
        /// call:
        /// 
        ///     f.EarliestTime()
        /// 
        /// The () are mandatory since C# has extension methods
        /// but not extension properties at this moment in time.
        /// </summary>
        public static DateTime EarliestTime(this FileInfo argument)
        {
            DateTime one = argument.CreationTime;
            DateTime two = argument.LastWriteTime;

            if (one.CompareTo(two) <= 0)
                return one;
            else
                return two;
        }


        /// <summary>
        /// Returns the most recent of the CreationTime and the
        /// LastWriteTime for the given FileInfo argument.
        /// 
        /// Note that if f is a FileInfo object, then one may
        /// call:
        /// 
        ///     f.MostRecentTime()
        /// 
        /// The () are mandatory since C# has extension methods
        /// but not extension properties at this moment in time.
        /// </summary>
        public static DateTime MostRecentTime(this FileInfo argument)
        {
            DateTime one = argument.CreationTime;
            DateTime two = argument.LastWriteTime;

            if (one.CompareTo(two) <= 0)
                return two;
            else
                return one;
        }
    }
}
