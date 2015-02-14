using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;

namespace edu.neu.ccis.rasala
{
    public class SearchTools
    {

        /// <summary>
        /// Returns a list of Range objects representing
        /// the matches of the pattern in the content.
        /// 
        /// If isRegex is true,
        /// uses regular expression matching,
        /// otherwise uses plain text matching.
        /// 
        /// Matching begins at the start index.
        /// 
        /// Ignores case if ignoreCase is true.
        /// </summary>
        /// <param name="content">
        ///     The content to search</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="start">
        ///     The search start index</param>
        /// <param name="isRegex">
        ///     If true use regex matching</param>
        /// <param name="ignoreCase">
        ///     If true ignore case</param>
        public static List<Range> Search
            (string content,
             string pattern,
             int start,
             bool isRegex,
             bool ignoreCase)
        {
            if (isRegex)
                return RegexSearch
                    (content, pattern, start, ignoreCase);
            else
                return PlainSearch
                    (content, pattern, start, ignoreCase);
        }


        /// <summary>
        /// Returns a list of Range objects representing
        /// the matches of the pattern in the content
        /// using plain text matching.
        /// 
        /// Matching begins at the start index.
        /// 
        /// Ignores case if ignoreCase is true.
        /// </summary>
        /// <param name="content">
        ///     The content to search</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="start">
        ///     The search start index</param>
        /// <param name="ignoreCase">
        ///     If true ignore case</param>
        public static List<Range> PlainSearch
            (string content,
             string pattern,
             int start,
             bool ignoreCase)
        {
            List<Range> list = new List<Range>();

            if (StringTools.IsTrivial(content))
                return list;

            if (StringTools.IsTrivial(pattern))
                return list;

            StringComparison compare = ignoreCase
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            int contentLength = content.Length;

            int patternLength = pattern.Length;

            if (start < 0)
                start = 0;

            while (start < contentLength)
            {
                int index = content.IndexOf(pattern, start, compare);

                if (index < 0)
                    break;

                list.Add(new Range(index, patternLength));

                start = index + patternLength;
            }

            return list;
        }


        /// <summary>
        /// Returns a list of Range objects representing
        /// the matches of the pattern in the content
        /// using regular expression matching.
        /// 
        /// Matching begins at the start index.
        /// 
        /// Ignores case if ignoreCase is true.
        /// </summary>
        /// <param name="content">
        ///     The content to search</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="start">
        ///     The search start index</param>
        /// <param name="ignoreCase">
        ///     If true ignore case</param>
        public static List<Range> RegexSearch
            (string content,
             string pattern,
             int start,
             bool ignoreCase)
        {
            List<Range> list = new List<Range>();

            if (StringTools.IsTrivial(content))
                return list;

            if (StringTools.IsTrivial(pattern))
                return list;

            RegexOptions options = RegexOptions.Multiline;

            if (ignoreCase)
                options |= RegexOptions.IgnoreCase;

            Regex regex = new Regex(pattern, options);

            if (start < 0)
                start = 0;

            if (start < content.Length)
            {
                Match match = regex.Match(content, start);

                while (match.Success)
                {
                    list.Add(new Range(match.Index, match.Length));

                    match = match.NextMatch();
                }
            }

            return list;
        }


        /// <summary>
        /// Returns a list of Range objects representing
        /// the matches of the pattern in the tilde file
        /// path name.
        /// 
        /// If isRegex is true,
        /// uses regular expression matching,
        /// otherwise uses plain text matching.
        /// 
        /// Ignores case if ignoreCase is true.
        /// 
        /// Returns null if the page or tilde file path
        /// is null or if the file is not OK to serve.
        /// 
        /// Precondition: The page that calls this method must:
        /// 
        ///   Be able to guarantee that the file is OK to serve
        ///   in context.
        /// </summary>
        /// <param name="page">
        ///     The page object that uses this method</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="isRegex">
        ///     If true use regex matching</param>
        /// <param name="ignoreCase">
        ///     If true ignore case</param>
        public static List<Range> SearchFileName
            (string tildeFilePath,
             string pattern,
             bool isRegex,
             bool ignoreCase)
        {
            return Search(tildeFilePath, pattern, 0, isRegex, ignoreCase);
        }


        /// <summary>
        /// Returns a list of Range objects representing
        /// the matches of the pattern in the content of
        /// a text file with the given tilde file path.
        /// 
        /// If successful, returns a reference to the
        /// text file content via an out parameter.
        /// 
        /// If isRegex is true,
        /// uses regular expression matching,
        /// otherwise uses plain text matching.
        /// 
        /// Ignores case if ignoreCase is true.
        /// 
        /// Returns null if the file is not a text file.
        /// 
        /// Precondition: The page that calls this method must:
        /// 
        ///   Be able to guarantee that the file is OK to serve
        ///   in context.
        /// </summary>
        /// <param name="page">
        ///     The page object that uses this method</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="isRegex">
        ///     If true use regex matching</param>
        /// <param name="ignoreCase">
        ///     If true ignore case</param>
        /// <param name="content">
        ///     Out param: If the file is a text file
        ///     then returns a reference to its content;
        ///     otherwise returns null.</param>
        public static List<Range> SearchFileContent
            (Page page,
             string tildeFilePath,
             string pattern,
             bool isRegex,
             bool ignoreCase,
             out string content)
        {
            content = null;

            int category = FileTools.GetFileCategory(tildeFilePath);

            if (category != FileTools.TEXT)
                return null;

            string filePath = page.MapPath(tildeFilePath);
            content = FileTools.GetFileAsText(filePath);

            return Search(content, pattern, 0, isRegex, ignoreCase);
        }


        /// <summary>
        /// Returns the markup from the search 
        /// of a file given its tilde path.
        /// 
        /// Preconditions: The page that calls this method must:
        /// 
        ///   1. Have executed the call
        /// 
        ///        SourceTools.LoadCSSandJavascript(this);
        ///     
        ///      during the initial call to PageLoad. 
        /// 
        ///   2. Be able to guarantee that the file is OK to serve
        ///      in context.
        /// </summary>
        /// <param name="page">
        ///     The page calling this method</param>
        /// <param name="tildeFilePath">
        ///     The file to search</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="isRegex">
        ///     Is the pattern a regular expression?</param>
        /// <param name="ignoreCase">
        ///     Ignore case in the search?</param>
        /// <param name="statistics">
        ///     Include file statistics markup?</param>
        /// <param name="download">
        ///     Include download button markup?</param>
        /// <param name="showAllLines">
        ///     Show all lines in the file?</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static string SearchFileMarkup
            (Page page,
             string tildeFilePath,
             string pattern,
             bool isRegex,
             bool ignoreCase,
             bool statistics,
             bool download,
             bool showAllLines,
             bool onlyPublic)
        {
            string content = null;

            List<Range> nameMatches =
                SearchFileName(tildeFilePath, pattern, isRegex, ignoreCase);

            List<Range> contentMatches =
                SearchFileContent
                    (page, tildeFilePath, pattern,
                     isRegex, ignoreCase, out content);

            bool matchName = !StringTools.IsTrivial(nameMatches);

            bool matchContent = !StringTools.IsTrivial(contentMatches);

            if (!(matchName || matchContent))
                return "";

            StringBuilder builder = new StringBuilder();

            // markup for highlighted tilde file path
            // with possible file view link

            string markupText =
                matchName
                    ? StringTools.HighlightMarkup(tildeFilePath, nameMatches, 4, false, true)
                    : tildeFilePath;

            long bytes;
            DateTime? created;
            DateTime? modified;

            builder.Append
                (SourceTools.StatisticsMarkup
                    (page, tildeFilePath, markupText, onlyPublic,
                     true, true, statistics, download, null,
                     out bytes, out created, out modified));

            // markup for highlighted content

            if (matchContent)
            {
                builder.Append(HTML_Tools.open_pre);

                builder.Append
                    (StringTools.HighlightMarkup
                        (content, contentMatches, 4, true, showAllLines));
                
                builder.Append(HTML_Tools.shut_pre);

                builder.Append("\n");
            }

            return builder.ToString();
        }


        /// <summary>
        /// Returns the markup from the search 
        /// of a directory given its tilde path.
        /// 
        /// Preconditions: The page that calls this method must:
        /// 
        ///   1. Have executed the call
        /// 
        ///        SourceTools.LoadCSSandJavascript(this);
        ///     
        ///      during the initial call to PageLoad. 
        /// 
        ///   2. Be able to guarantee that the directory is OK to serve
        ///      in context.
        /// </summary>
        /// <param name="page">
        ///     The page calling this method</param>
        /// <param name="tildeDirectoryPath">
        ///     The directory to search</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="isRegex">
        ///     Is the pattern a regular expression?</param>
        /// <param name="ignoreCase">
        ///     Ignore case in the search?</param>
        /// <param name="statistics">
        ///     Include file statistics markup?</param>
        /// <param name="download">
        ///     Include download button markup?</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static string SearchDirectoryMarkup
            (Page page,
             string tildeDirectoryPath,
             string pattern,
             bool isRegex,
             bool ignoreCase,
             bool statistics,
             bool download,
             bool onlyPublic)
        {
            int n = tildeDirectoryPath.Length;

            if (tildeDirectoryPath[n - 1] != SourceTools.slash)
                tildeDirectoryPath = tildeDirectoryPath + SourceTools.slash;

            StringBuilder builder = new StringBuilder();

            string directoryPath = page.MapPath(tildeDirectoryPath);

            DirectoryInfo directory = new DirectoryInfo(directoryPath);

            FileInfo[] files = directory.GetFiles();

            int m = files.Length;

            for (int i = 0; i < m; i++)
            {
                string tildeFilePath = tildeDirectoryPath + files[i].Name;

                string markup = SearchFileMarkup(page, tildeFilePath, pattern,
                    isRegex, ignoreCase, statistics, download, false, onlyPublic);

                if (!StringTools.IsTrivial(markup))
                {
                    builder.Append(markup);
                    builder.Append("<hr />\n");
                }
            }

            return builder.ToString();
        }


        /// <summary>
        /// Returns the markup from the search 
        /// of either the public directories
        /// or all directories that start at
        /// tildeDirectoryPath.
        /// </summary>
        /// <param name="page">
        ///     The page calling this method</param>
        /// <param name="tildeDirectoryPath">
        ///     The root of the directory tree to search</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="isRegex">
        ///     Is the pattern a regular expression?</param>
        /// <param name="ignoreCase">
        ///     Ignore case in the search?</param>
        /// <param name="statistics">
        ///     Include file statistics markup?</param>
        /// <param name="download">
        ///     Include download button markup?</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static string SearchTreeMarkup
            (Page page,
             string tildeDirectoryPath,
             string pattern,
             bool isRegex,
             bool ignoreCase,
             bool statistics,
             bool download,
             bool onlyPublic)
        {
            StringBuilder builder = new StringBuilder();

            int n = tildeDirectoryPath.Length;

            if (tildeDirectoryPath[n - 1] != SourceTools.slash)
                tildeDirectoryPath = tildeDirectoryPath + SourceTools.slash;

            string directoryPath = page.MapPath(tildeDirectoryPath);

            string rootPath = FileTools.GetRoot(page);

            List<string> directoryList =
                SourceTools.MakeDirectoryList(directoryPath, onlyPublic);

            List<string> tildeDirectoryList =
                FileTools.GetTildePaths(rootPath, directoryList);

            foreach (string tdp in tildeDirectoryList)
            {
                string markup =
                    SearchDirectoryMarkup(page, tdp, pattern,
                        isRegex, ignoreCase, statistics, download, onlyPublic);

                if (!StringTools.IsTrivial(markup))
                    builder.Append(markup);
            }

            return builder.ToString();
        }


        /// <summary>
        /// Returns the markup from the search 
        /// of either the public directories
        /// or all directories.
        /// </summary>
        /// <param name="page">
        ///     The page calling this method</param>
        /// <param name="pattern">
        ///     The search pattern</param>
        /// <param name="isRegex">
        ///     Is the pattern a regular expression?</param>
        /// <param name="ignoreCase">
        ///     Ignore case in the search?</param>
        /// <param name="statistics">
        ///     Include file statistics markup?</param>
        /// <param name="download">
        ///     Include download button markup?</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static string SearchSiteMarkup
            (Page page,
             string pattern,
             bool isRegex,
             bool ignoreCase,
             bool statistics,
             bool download,
             bool onlyPublic)
        {
            StringBuilder builder = new StringBuilder();

            string rootPath = FileTools.GetRoot(page);

            List<string> directoryList =
                SourceTools.MakeDirectoryList(rootPath, onlyPublic);

            List<string> tildeDirectoryList =
                FileTools.GetTildePaths(rootPath, directoryList);

            foreach (string tildeDirectoryPath in tildeDirectoryList)
            {
                string markup =
                    SearchDirectoryMarkup(page, tildeDirectoryPath, pattern,
                        isRegex, ignoreCase, statistics, download, onlyPublic);

                if (!StringTools.IsTrivial(markup))
                    builder.Append(markup);
            }

            return builder.ToString();
        }

    }
}