using System;
using System.IO;
using System.Collections.Generic;

namespace edu.neu.ccis.rasala
{
    /// <summary>
    /// The class CompareCreationTimes defines a static IComparer
    /// object named Comparer that will compare files by
    /// creation date and time.
    /// </summary>
    public class CompareCreationTimes : IComparer<FileInfo>
    {
        // Implementation of IComparer<FileInfo>
        public Int32 Compare(FileInfo f1, FileInfo f2)
        {
            return f1.CreationTime.CompareTo(f2.CreationTime);
        }

        // The singleton Comparer object
        public static readonly CompareCreationTimes Comparer
            = new CompareCreationTimes();

        // Private constructor to prevent additional instantiation
        private CompareCreationTimes() { }
    }


    /// <summary>
    /// The class CompareModifiedTimes defines a static IComparer
    /// object named Comparer that will compare files by
    /// modified date and time, that is, what Microsoft calls the
    /// LastWriteTime.
    /// </summary>
    public class CompareModifiedTimes : IComparer<FileInfo>
    {
        // Implementation of IComparer<FileInfo>
        public Int32 Compare(FileInfo f1, FileInfo f2)
        {
            return f1.LastWriteTime.CompareTo(f2.LastWriteTime);
        }

        // The singleton Comparer object
        public static readonly CompareModifiedTimes Comparer
            = new CompareModifiedTimes();

        // Private constructor to prevent additional instantiation
        private CompareModifiedTimes() { }
    }
    

    /// <summary>
    /// The class CompareEarliestTimes defines a static IComparer
    /// object named Comparer that will compare files by
    /// the earliest of the creation and modified dates and times.
    /// 
    /// Uses the FileInfo extension method EarliestTime().
    /// </summary>
    public class CompareEarliestTimes : IComparer<FileInfo>
    {
        // Implementation of IComparer<FileInfo>
        public Int32 Compare(FileInfo f1, FileInfo f2)
        {
            return f1.EarliestTime().CompareTo(f2.EarliestTime());
        }

        // The singleton creationComparer object
        public static readonly CompareEarliestTimes Comparer
            = new CompareEarliestTimes();

        // Private constructor to prevent additional instantiation
        private CompareEarliestTimes() { }
    }


}
