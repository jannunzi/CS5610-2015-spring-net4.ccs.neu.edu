/*
    File: ~/app_code/DateTimeExtensions.cs
    
    Copyright 2013,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace edu.neu.ccis.rasala
{
    public static class DateTimeExtensions
    {

        /// <summary>
        /// Returns the DateTime argument as a string of the format:
        /// 
        ///     yyyy-mm-dd_hh-mm-ss_xxx
        ///     
        /// where in order:
        /// 
        ///     yyyy = year
        ///     mm = 2 digit month
        ///     dd = 2 digit day
        ///     hh = 2 digit hours on the 24 hour clock
        ///     mm = 2 digit minutes
        ///     ss = 2 digit seconds
        ///     xxx = 3 digit milliseconds
        /// 
        /// This string format sorts as a string in the same order as
        /// the original DateTime objects.
        /// 
        /// Since the format uses only digits, hyphen, and underscore,
        /// it might be used within a file name in any file system.
        /// </summary>
        public static string ToYMDHMS(this DateTime argument)
        {
            char hyphen = '-';
            char underscore = '_';

            StringBuilder builder = new StringBuilder();

            builder.Append(argument.Year.ToString("0000"));
            builder.Append(hyphen);
            builder.Append(argument.Month.ToString("00"));
            builder.Append(hyphen);
            builder.Append(argument.Day.ToString("00"));
            builder.Append(underscore);
            builder.Append(argument.Hour.ToString("00"));
            builder.Append(hyphen);
            builder.Append(argument.Minute.ToString("00"));
            builder.Append(hyphen);
            builder.Append(argument.Second.ToString("00"));
            builder.Append(underscore);
            builder.Append(argument.Millisecond.ToString("000"));

            return builder.ToString();
        }


        /// <summary>
        /// Returns the Date portion of the DateTime argument as a
        /// string of the format:
        /// 
        ///     yyyy-mm-dd
        ///     
        /// where in order:
        /// 
        ///     yyyy = year
        ///     mm = 2 digit month
        ///     dd = 2 digit day
        /// </summary>
        public static string ToYMD(this DateTime argument)
        {
            char hyphen = '-';

            StringBuilder builder = new StringBuilder();

            builder.Append(argument.Year.ToString("0000"));
            builder.Append(hyphen);
            builder.Append(argument.Month.ToString("00"));
            builder.Append(hyphen);
            builder.Append(argument.Day.ToString("00"));

            return builder.ToString();
        }


        /// <summary>
        /// Returns the Time portion of the DateTime argument as a
        /// string of the format:
        /// 
        ///     hh-mm-ss_xxx
        ///     
        /// where in order:
        /// 
        ///     hh = 2 digit hours on the 24 hour clock
        ///     mm = 2 digit minutes
        ///     ss = 2 digit seconds
        ///     xxx = 3 digit milliseconds
        /// </summary>
        public static string ToHMS(this DateTime argument)
        {
            char hyphen = '-';
            char underscore = '_';

            StringBuilder builder = new StringBuilder();

            builder.Append(argument.Hour.ToString("00"));
            builder.Append(hyphen);
            builder.Append(argument.Minute.ToString("00"));
            builder.Append(hyphen);
            builder.Append(argument.Second.ToString("00"));
            builder.Append(underscore);
            builder.Append(argument.Millisecond.ToString("000"));

            return builder.ToString();
        }


        /// <summary>
        /// Constructs a DateTime using input formatted as in the
        /// method ToYMDHMS or using the special strings "now"
        /// and "today".  Input is case insensitive.
        /// 
        /// Allows the string "now" to return DateTime.Now.
        /// 
        /// Allows the string "today" to return the first moment
        /// of today, that is, DateTime.Now adjusted so that the
        /// hours, minutes, seconds, and milliseconds are 0.
        /// 
        /// Further allows "today-N" where N is a positive integer
        /// string value.  This will yield N days before the first
        /// moment of today.
        /// 
        /// Aside from the special strings, the input should
        /// consist only of digits, hyphens, and underscores just
        /// as in ToYMDHMS.  Parsing stops and uses what has been
        /// read so far if the input fails to match the pattern
        /// expected.  Unread fields use the following defaults:
        /// 
        ///     year = DateTime.Now.Year
        ///     month = 1
        ///     day = 1
        ///     hour = 0
        ///     minute = 0
        ///     second = 0
        ///     millisecond = 0
        /// 
        /// In particular, FromYMDHMS("") returns the first moment
        /// of the current year.
        /// 
        /// Input fields are forced to satisfy the following rules:
        /// 
        ///     1 LE year
        ///     1 LE month LE 12
        ///     1 LE day LE 31
        ///     0 LE hour LE 23
        ///     0 LE minute LE 59
        ///     0 LE second LE 59
        ///     0 LE millisecond LE 999
        /// 
        /// The symbol LE is used for "less than or equal" because
        /// this comment is XML and does not allow the real symbol
        /// for "less than".
        /// 
        /// Finally, for months that do not allow 31 days, the day
        /// is forced downward into the correct range.
        /// </summary>
        public static DateTime FromYMDHMS(string YMDHMS)
        {
            DateTime Now = DateTime.Now;

            int year = Now.Year;
            int month = 1;
            int day = 1;
            int hour = 0;
            int minute = 0;
            int second = 0;
            int millisecond = 0;

            if (StringTools.IsTrivial(YMDHMS))
                goto returnStatement;

            YMDHMS = YMDHMS.ToLower();

            if (YMDHMS == "now")
                return DateTime.Now;


            int length = YMDHMS.Length;
            int position = 0;

            char c;
            char hyphen = '-';
            char underscore = '_';


            if (length >= 5)
            {
                string chunk = YMDHMS.Substring(0, 5);

                if (chunk == "today")
                {
                    month = Now.Month;
                    day = Now.Day;

                    DateTime Today = new DateTime(year, month, day, 0, 0, 0, 0);

                    if (length == 5)
                        return Today;

                    position = 5;

                    c = YMDHMS[position];

                    if (c == hyphen)
                        position++;
                    else
                        return Today;

                    int delta = GetNextInt(YMDHMS, length, ref position, 0, int.MaxValue);

                    return Today.AddDays(-delta);
                }
            }


            // handle year
            year = GetNextInt(YMDHMS, length, ref position, 1, int.MaxValue);

            if (position >= length)
                goto returnStatement;

            c = YMDHMS[position];

            if (c == hyphen)
                position++;
            else
                goto returnStatement;


            // handle month
            month = GetNextInt(YMDHMS, length, ref position, 1, 12);

            if (position >= length)
                goto returnStatement;

            c = YMDHMS[position];

            if (c == hyphen)
                position++;
            else
                goto returnStatement;


            // handle day
            day = GetNextInt(YMDHMS, length, ref position, 1, 31);

            if (position >= length)
                goto returnStatement;

            c = YMDHMS[position];

            if (c == underscore)
                position++;
            else
                goto returnStatement;


            // handle hour
            hour = GetNextInt(YMDHMS, length, ref position, 0, 23);

            if (position >= length)
                goto returnStatement;

            c = YMDHMS[position];

            if (c == hyphen)
                position++;
            else
                goto returnStatement;


            // handle minute
            minute = GetNextInt(YMDHMS, length, ref position, 0, 59);

            if (position >= length)
                goto returnStatement;

            c = YMDHMS[position];

            if (c == hyphen)
                position++;
            else
                goto returnStatement;


            // handle second
            second = GetNextInt(YMDHMS, length, ref position, 0, 59);

            if (position >= length)
                goto returnStatement;

            c = YMDHMS[position];

            if (c == underscore)
                position++;
            else
                goto returnStatement;


            // handle millisecond
            millisecond = GetNextInt(YMDHMS, length, ref position, 0, 999);


            returnStatement:

            DateTime datetime = DateTime.Now;

            while (true)
            {
                try
                {
                    datetime =
                        new DateTime(year, month, day, hour, minute, second, millisecond);
                    
                    break;
                }
                catch
                {
                    day--;
                }

                if (day < 1)
                {
                    datetime = DateTime.Now;
                    break;
                }
            }

            return datetime;
        }


        private static int GetNextInt(string s, int length, ref int position, int min, int max)
        {
            if (position >= length)
                return min;

            int x = 0;
            int zero = (int)'0';

            while (position < length)
            {
                char c = s[position];

                if (char.IsDigit(c))
                {
                    x *= 10;
                    x += ((int)c - zero);

                    position++;
                }
                else
                    break;
            }

            if (x < min)
                return min;

            if (x > max)
                return max;

            return x;
        }
    }
}
