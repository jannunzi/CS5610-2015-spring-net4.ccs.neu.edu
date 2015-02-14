using System;
using System.Web.UI;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace edu.neu.ccis.rasala
{
    /** Adapted from blog code by Peter Douglass. */

    /// <summary>
    /// Blog may be used to construct a blog associated with
    /// a given Page object and a given directory containing
    /// the html content files.
    /// 
    /// The blog data is sorted internally in order of the
    /// earliest date of the associated files.  See the file
    /// RasalaExtensions.cs for the definition of "earliest".
    /// 
    /// The blog may be delivered in order of earliest date
    /// or in reverse order.  The caller may restrict the blog
    /// to a given year and month if desired.
    /// 
    /// The caller may also obtain an array of the blog items
    /// and then may manipulate this data in whatever fashion
    /// is desired.
    /// 
    /// On 5/23/08, fixed a bug and modified the definition of
    /// "recent" in the method GetRecentBlog.
    /// 
    /// On 1/21/09, added namespace and refactored so that the
    /// StringBuilder object constructed in this class may be
    /// passed to each BlogItem as the blog is created.  Also,
    /// moved CompareCreationTimes to its own file.
    /// 
    /// On 4/29/09, replaced creation date with earliest date.
    /// This is because of the following phenomenon.  If a
    /// file is copied to a new machine, it may carry with it
    /// its modification date but have a creation date on the
    /// new system that reflects the moment of copying.
    /// Whether or not this phenomenon happens depends on the
    /// software used to do the copying and may not be easily
    /// predictable.
    /// 
    /// On 1/17/14, decided to search for blog posts that match
    /// the search pattern *.htm* to include both .htm and .html
    /// 
    /// This accommodates the change in Visual Studio to make a
    /// default HTML file have extension .html while keeping
    /// backward compatibility with the older .htm extension.
    /// </summary>
    public class Blog
    {
        BlogItem[] items;

        static int minimumRecentItems = 5; // minimum number of recent items

        /// <summary>
        /// The constructor constructs an internal array of BlogItem objects
        /// based the given Page object and the given directory of html
        /// content files in the directory specified by a url given relative
        /// to the location of the page.
        /// 
        /// This class can return the html for a blog based on this data.
        /// The blog html may be arranged in order of earliest date or in
        /// reverse order.  The blog html may be restricted to content
        /// created in a particular month and year if desired.
        /// 
        /// This class can return a copy of all BlogItem data for further
        /// manipulation by the caller.
        /// </summary>
        /// <param name="page">The blog page to load.</param>
        /// <param name="relativePath">The relative path from the directory
        /// of the blog page to the directory of the html content files.</param>
        public Blog(Page page, string relativePath)
        {
            string fullPath = page.Server.MapPath(relativePath);

            DirectoryInfo info = new DirectoryInfo(fullPath);

            FileInfo[] files = info.GetFiles("*.htm*");
            int n = files.Length;

            if (n > 0)
            {
                Array.Sort<FileInfo>(files, CompareEarliestTimes.Comparer);
            }

            items = new BlogItem[n];

            for (int i = 0; i < n; i++)
                items[i] = new BlogItem(files[i]);
        }


        /// <summary>
        /// Returns a blog constructed by concatenating the individual blog items
        /// either in order of earliest date or in reverse order depending on the
        /// given reverse parameter.
        /// </summary>
        /// <param name="reverse">Whether or not to reverse order the blog items.</param>
        public string GetBlog(bool reverse)
        {
            int n = items.Length;

            if (n == 0)
                return "";

            StringBuilder builder = new StringBuilder();

            if (reverse)
            {
                for (int i = (n - 1); i >= 0; i--)
                    items[i].AppendTo(builder);
            }
            else
            {
                for (int i = 0; i < n; i++)
                    items[i].AppendTo(builder);
            }

            return builder.ToString();
        }


        /// <summary>
        /// Returns a blog constructed by concatenating the individual blog items
        /// either in order of earliest date or in reverse order depending on the
        /// given reverse parameter.  The items included are restricted to the
        /// given year and month.
        /// </summary>
        /// <param name="reverse">Whether or not to reverse order the blog items.</param>
        /// <param name="year">The blog item year.</param>
        /// <param name="month">The blog item month.</param>
        /// <returns></returns>
        public string GetBlog(bool reverse, int year, int month)
        {
            int n = items.Length;

            if (n == 0)
                return "";

            StringBuilder builder = new StringBuilder();

            if (reverse)
            {
                for (int i = (n - 1); i >= 0; i--)
                {
                    DateTime datetime = items[i].BlogItemEarliestTime;

                    if ((datetime.Year == year) && (datetime.Month == month))
                        items[i].AppendTo(builder);
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    DateTime datetime = items[i].BlogItemEarliestTime;

                    if ((datetime.Year == year) && (datetime.Month == month))
                        items[i].AppendTo(builder);
                }
            }

            return builder.ToString();
        }


        /// <summary>
        /// Returns a blog constructed by concatenating the individual blog items
        /// either in order of earliest date or in reverse order depending on the
        /// given reverse parameter.
        /// 
        /// Originally, the definition of "recent" was "the items created in the
        /// past month from the date of access to the blog."
        /// 
        /// However, if the blog has not been active, this definition may lead to
        /// no postings whatsoever.
        /// 
        /// Therefore, the collection of "recent" blog items is now defined as
        /// follows:
        ///   a. Collect the 5 most recent blog items.
        ///   b. Add to this collection all blog items posted within one month of
        ///      the latest blog item, if any.
        /// 
        /// Also, a bug in the computation has been fixed (5/23/08).
        /// </summary>
        /// <param name="reverse">Whether or not to reverse order the blog items.</param>
        public string GetRecentBlog(bool reverse)
        {
            int n = items.Length;

            if (n == 0)
                return "";

            StringBuilder builder = new StringBuilder();

            int a = n - 2;    // exclusive lower index
            int b = n - 1;    // inclusive upper index

            DateTime mostrecent = items[b].BlogItemEarliestTime;

            int year1 = mostrecent.Year;
            int month1 = mostrecent.Month;
            int day1 = mostrecent.Day;

            int year0 = year1;
            int month0 = month1 - 1;

            if (month0 <= 0)
            {
                year0 = year0 - 1;
                month0 = 12;
            }

            while (a >= 0)
            {
                if ((b - a) < minimumRecentItems)
                {
                    a--;
                    continue;
                }

                DateTime datetime = items[a].BlogItemEarliestTime;

                int year = datetime.Year;
                int month = datetime.Month;
                int day = datetime.Day;

                bool ok = (year > year0)
                    || ((year == year0) && (month > month0))
                    || ((year == year0) && (month == month0) && (day >= day1));

                if (ok)
                {
                    a--;
                    continue;
                }

                break;
            }

            if (reverse)
            {
                for (int i = b; i > a; i--)
                {
                    items[i].AppendTo(builder);
                }
            }
            else
            {
                for (int i = (a + 1); i <= b; i++)
                {
                    items[i].AppendTo(builder);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns a copy of the internal array of BlogItem objects
        /// to be manipulated as desired by the caller.
        /// 
        /// The items are in order of the earliest date of the html
        /// content files.
        /// </summary>
        public BlogItem[] GetBlogItems()
        {
            int n = items.Length;

            BlogItem[] copy = new BlogItem[n];

            for (int i = 0; i < n; i++)
                copy[i] = items[i];

            return copy;
        }


        /// <summary>
        /// Returns a List of int[] objects in which each int[] is a triple
        /// consisting of a year, month, and count where count is the number
        /// of blog items that occur in that month.
        /// </summary>
        public int[][] GetYearMonthCountList()
        {
            List<int[]> list = new List<int[]>();

            int n = items.Length;

            if (n > 0)
            {
                int index = 0;

                BlogItem item = items[index];
                DateTime datetime = item.BlogItemEarliestTime;
                int year0 = datetime.Year;
                int month0 = datetime.Month;

                int[] triple = new int[] { year0, month0, 1 };

                index++;

                while (index < n)
                {
                    item = items[index];
                    datetime = item.BlogItemEarliestTime;

                    int year1 = datetime.Year;
                    int month1 = datetime.Month;

                    if ((year1 == year0) && (month1 == month0))
                    {
                        triple[2]++;
                    }
                    else
                    {
                        list.Add(triple);
                        year0 = year1;
                        month0 = month1;
                        triple = new int[] { year0, month0, 1 };
                    }

                    index++;
                }

                list.Add(triple);
            }
            return (int[][])list.ToArray();
        }
    }
}
