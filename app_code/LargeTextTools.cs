/*
    File: ~/app_code/LargeTextTools.cs
    
    Copyright 2013,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Text;

namespace edu.neu.ccis.rasala
{

    public class LargeTextTools
    {

        /// <summary>
        /// Returns a string for an HTML table with
        ///     number of bytes
        ///     number of lines
        ///     creation date,
        ///     last modification date
        /// for the file with the given tildeFilePath.
        /// </summary>
        /// <param name="server">The server to convert to real file paths</param>
        /// <param name="tildeFilePath">The tilde file path</param>

        public static string TextFileStatistics(HttpServerUtility server, string tildeFilePath)
        {
            StringBuilder builder = new StringBuilder();
            string filePath = null;
            FileInfo info = null;

            if (TildeFilePathExistsAndIsText(server, tildeFilePath, ref filePath, ref info))
            {
                long bytes = info.Length;
                DateTime created = info.CreationTime;
                DateTime modified = info.LastWriteTime;

                long lines = 0;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string foobar = reader.ReadLine();
                        lines++;
                    }
                }

                TildeFilePathStatisticsTable(builder, bytes, lines, created, modified);
            }
            else
            {
                TildeFilePathErrorMessage(builder);
            }

            return builder.ToString();
        }


        /// <summary>
        /// Returns true if the tildeFilePath corresponds to a file that
        /// exists and is a text file.
        /// 
        /// In that case, the filePath and info parameters are initialized.
        /// </summary>
        /// <param name="server">The server to convert to real file paths</param>
        /// <param name="tildeFilePath">The tilde file path</param>
        /// <param name="filePath">The real file path</param>
        /// <param name="info">The FileInfo object</param>
        public static bool TildeFilePathExistsAndIsText
            (HttpServerUtility server,
             string tildeFilePath,
             ref string filePath,
             ref FileInfo info)
        {
            bool error = StringTools.IsTrivial(tildeFilePath);

            if (!error)
            {
                error = !tildeFilePath.StartsWith("~/");
            }

            if (!error)
            {
                int category = FileTools.GetFileCategory(tildeFilePath);
                error = category != FileTools.TEXT;
            }

            if (!error)
            {
                try
                {
                    filePath = server.MapPath(tildeFilePath);
                    info = new FileInfo(filePath);
                    long bytes = info.Length;
                }
                catch
                {
                    error = true;
                }
            }

            return !error;
        }


        /// <summary>
        /// Constructs an HTML table with the file statistics
        /// and stores that information into the builder.
        /// </summary>
        /// <param name="builder">The builder to accumulate data</param>
        /// <param name="bytes">File bytes</param>
        /// <param name="lines">File lines</param>
        /// <param name="created">File creation date</param>
        /// <param name="modified">File last modification date</param>
        public static void TildeFilePathStatisticsTable
            (StringBuilder builder,
             long bytes,
             long lines,
             DateTime created,
             DateTime modified)
        {
            string a = "";
            string b = "";

            builder.Append("<table class='filedata'>\n");

            a = bytes.ToString();
            HTML_Tools.AppendTableRow(builder, "Bytes", a);

            a = lines.ToString();
            HTML_Tools.AppendTableRow(builder, "Lines", a);

            a = created.ToYMD();
            b = created.ToHMS();

            HTML_Tools.AppendTableRow(builder, "Created", a, b);

            a = modified.ToYMD();
            b = modified.ToHMS();

            HTML_Tools.AppendTableRow(builder, "Modified", a, b);

            builder.Append("</table>\n<br />\n");
        }


        /// <summary>
        /// Return the HTML string to list consecutive lines in the file.
        /// 
        /// The returned string has all required HTML to plug into a page.
        /// </summary>
        /// <param name="server">The server to convert to real file paths</param>
        /// <param name="tildeFilePath">The tilde file path</param>
        /// <param name="StartLine">The start line for the list</param>
        /// <param name="MaxLines">The maximum number of lines to list</param>
        /// <param name="showLineNumbers">If true, show line numbers</param>
        public static string ListLines
            (HttpServerUtility server,
             string tildeFilePath,
             long StartLine,
             long MaxLines,
             bool showLineNumbers)
        {
            StringBuilder builder = new StringBuilder();
            string filePath = null;
            FileInfo info = null;

            if (TildeFilePathExistsAndIsText(server, tildeFilePath, ref filePath, ref info))
            {
                if (StartLine < 1)
                    StartLine = 1;

                if (MaxLines < 1)
                    MaxLines = 1;

                long LineNumber = 1;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Skip lines up to StartLine
                    while ((LineNumber < StartLine) && !reader.EndOfStream)
                    {
                        string foobar = reader.ReadLine();
                        LineNumber++;
                    }

                    if (reader.EndOfStream)
                    {
                        NoLinesErrorMessage(builder);
                    }
                    else
                    {
                        builder.Append("<pre>");

                        long FinishLine = StartLine + MaxLines - 1;

                        while ((LineNumber <= FinishLine) && !reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string result = StringTools.HighlightMarkupForOneLine
                                (line, null, 4, showLineNumbers, LineNumber);

                            builder.Append(result);
                            builder.Append("\n");

                            LineNumber++;
                        }

                        builder.Append("</pre>\n");
                    }
                }
            }
            else
            {
                TildeFilePathErrorMessage(builder);
            }

            return builder.ToString();
        }


        /// <summary>
        /// Searches for the given pattern in the file starting at the
        /// start line.
        /// 
        /// Will stop searching after max successful lines are emitted.
        /// 
        /// If max lines are never emitted then will search until the
        /// end of the file.  This may unfortunately be slow when the
        /// file is very large.
        /// </summary>
        /// <param name="server">The server to convert to real file paths</param>
        /// <param name="tildeFilePath">The tilde file path</param>
        /// <param name="pattern">The search pattern</param>
        /// <param name="isRegex">If true, the pattern is a regex</param>
        /// <param name="ignoreCase">If true, do case-insensitive search</param>
        /// <param name="StartLine">The start line for the list</param>
        /// <param name="MaxLines">The maximum number of lines to list</param>
        /// <param name="showLineNumbers">If true, show line numbers</param>
        /// <returns></returns>
        public static string ListLinesFromSearch
            (HttpServerUtility server,
             string tildeFilePath,
             string pattern,
             bool isRegex,
             bool ignoreCase,
             long StartLine,
             long MaxLines,
             bool showLineNumbers,
             bool showAllLines)
        {
            if (StringTools.IsTrivial(pattern))
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            string filePath = null;
            FileInfo info = null;

            if (TildeFilePathExistsAndIsText(server, tildeFilePath, ref filePath, ref info))
            {
                if (StartLine < 1)
                    StartLine = 1;

                if (MaxLines < 1)
                    MaxLines = 1;

                long LineNumber = 1;
                long FoundMatch = 0;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Skip lines up to StartLine
                    while ((LineNumber < StartLine) && !reader.EndOfStream)
                    {
                        string foobar = reader.ReadLine();
                        LineNumber++;
                    }

                    if (reader.EndOfStream)
                    {
                        NoLinesErrorMessage(builder);
                    }
                    else
                    {
                        builder.Append("<pre>");

                        while ((FoundMatch < MaxLines) && !reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            List<Range> range = SearchTools.Search
                                (line, pattern, 0, isRegex, ignoreCase);

                            if (showAllLines || (range.Count > 0))
                            {
                                string result = StringTools.HighlightMarkupForOneLine
                                    (line, range, 4, showLineNumbers, LineNumber);

                                builder.Append(result);
                                builder.Append("\n");

                                FoundMatch++;
                            }

                            LineNumber++;
                        }

                        if (FoundMatch > 0)
                        {
                            builder.Append("</pre>\n");
                        }
                        else
                        {
                            builder = new StringBuilder();
                            NoLinesErrorMessage(builder);
                        }

                    }
                }
            }
            else
            {
                TildeFilePathErrorMessage(builder);
            }

            return builder.ToString();
        }


        // Error string is the tilde file path does not exist
        // or is not a text file
        static string tildeFilePathError =
            "The tilde file path does not resolve to a text file";


        /// <summary>
        /// Adds the tilde file error message to the builder.
        /// </summary>
        public static void TildeFilePathErrorMessage(StringBuilder builder)
        {
            builder.Append("<p class='error'>");
            builder.Append(tildeFilePathError);
            builder.Append("</p>");
        }


        // Error stating that no lines can satisty the list constraints
        // or can match a given search pattern starting at start line.
        static string noLinesError =
            "No lines in the file meet the constraints";


        /// <summary>
        /// Adds the "no lines" error message to the builder.
        /// </summary>
        public static void NoLinesErrorMessage(StringBuilder builder)
        {
            builder.Append("<p class='error'>");
            builder.Append(noLinesError);
            builder.Append("</p>");
        }

    }

}