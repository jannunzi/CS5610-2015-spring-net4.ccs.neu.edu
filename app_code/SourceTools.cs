/*
    File: ~/app_code/SourceTools.cs
    
    Copyright 2011,
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
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;

namespace edu.neu.ccis.rasala
{
    public class SourceTools
    {
        /// <summary>
        /// Directory names that should not be served
        /// by sources server
        /// </summary>
        static string[] secure =
            { "hidden", "secret", "secure", "generatedimages" };


        public const char slash = '/';
        public const char backslash = '\\';
        public const char colon = ':';
        public const string tildeStart = FileTools.tildeStart;

        /// <summary>
        /// The tilde path of the CSS
        /// for the pages that use the tools in this class.
        /// 
        /// "~/source/source.css"
        /// </summary>
        public const string sourceCSS = "~/source/source.css";

        /// <summary>
        /// The tilde path of the CSS
        /// for printing
        /// for the pages that use the tools in this class.
        /// 
        /// "~/source/print.css"
        /// </summary>
        public const string printCSS = "~/source/print.css";


        /// <summary>
        /// The tilde path for modernizr.
        /// 
        /// "~/javascript/modernizr.min.js"
        /// </summary>
        public const string modernizrJS = "~/javascript/modernizr.min.js";

        /// <summary>
        /// The tilde path of the Javascript
        /// for pages that use the tools in this class.
        /// 
        /// "~/source/source.js"
        /// </summary>
        public const string sourceJS = "~/source/source.js";


        /// <summary>
        /// The tilde path of the file view page.
        /// </summary>
        public const string FileViewTildePath =
            "~/fileview/Default.aspx";

        /// <summary>
        /// The tilde path of the internal file view page.
        /// </summary>
        public const string InternalFileViewTildePath =
            "~/fileview/all/Default.aspx";


        /// <summary>
        /// The tilde path of the statistics page.
        /// </summary>
        public const string StatisticsTildePath =
            "~/statistics/Default.aspx";


        /// <summary>
        /// String for a regular expression that will match
        /// a character that should NOT appear in a tilde path:
        ///   < > : " \ ? *
        /// Used to check the validity of a tilde path.
        /// </summary>
        public const string TildePathExcludeList = "[<>:\"\\?*]";


        /// <summary>
        /// The markup used in statistics when the directory
        /// has no files.
        /// </summary>
        public const string NoFile =
            "<pre style='color:red'>No file in this directory</pre>";

        /// <summary>
        /// The markup used when no path is provided.
        /// </summary>
        public const string NoPath =
@"<p style='color:red'>
The tilde path was not provided.
</p>";

        /// <summary>
        /// The markup used when no path is provided in the query string.
        /// </summary>
        public const string NoPathInQuery =
@"<p style='color:red'>
The tilde path was not provided as the query string.
</p>";

        /// <summary>
        /// The markup used when the path provided may not be served.
        /// </summary>
        public const string NotToServe =
@"<p style='color:red'>
The tilde path provided
does not represent a tilde path that may be served.
</p>";

        /// <summary>
        /// The markup used when the file provided in the query string
        /// may not be served.
        /// </summary>
        public const string NotToServeInQuery =
@"<p style='color:red'>
The tilde path provided in the query string
does not represent a tilde path that may be served.
</p>";


        /// <summary>
        /// The markup used when the file is not viewable.
        /// </summary>
        public const string NotViewable =
            "<p style='color:red'>This file is not viewable.</p>";


        /// <summary>
        /// The markup used when the file is not a text file.
        /// </summary>
        public const string NotTextFile =
            "<p style='color:red'>This file is not a text file.</p>";


        /// <summary>
        /// Part 1 of the string to create the download button markup.
        /// </summary>
        public const string Download1 =
            "<p>\n"
            + "<input type='button' class='downloadbutton' value='Download File' />\n"
            + "<span class='hidden'>";

        /// <summary>
        /// Part 2 of the string to create the download button markup.
        /// </summary>
        public const string Download2 =
            "</span>\n</p>";


        /// <summary>
        /// Part 1 of download button markup failure.
        /// </summary>
        public const string DownloadFailure1 =
            "<p>Failed to create download markup</p>";

        /// <summary>
        /// Part 2 of download button markup failure.
        /// </summary>
        public const string DownloadFailure2 =
            "<p>For file <code>";

        /// <summary>
        /// Part 3 of download button markup failure.
        /// </summary>
        public const string DownloadFailure3 =
            "</code></p>";


        /// <summary>
        /// Error message if there is a problem
        /// with an XML file or an XML string
        /// that contains download file list information.
        /// </summary>
        public const string DownloadFileListError =
            "<p style='color:red'>Download File List Error</p>";


        /// <summary>
        /// Load references to:
        ///     ~/source/source.css
        ///     ~/source/print.css
        /// The file ~/print.css is constrained by media='print'.
        /// </summary>
        public static void LoadCSS(Page page)
        {
            CSS.LoadFileReference(page, sourceCSS,  null);
            CSS.LoadFileReference(page, printCSS, "print");
        }


        /// <summary>
        /// Load references to:
        ///     jQuery
        ///     modernizr
        ///     ~/source/source.js
        /// </summary>
        public static void LoadJavascript(Page page)
        {
            Javascript.LoadJQueryReference(page);
            Javascript.LoadFileReference(page, modernizrJS);
            Javascript.LoadFileReference(page, sourceJS);
        }


        /// <summary>
        /// Load the CCS and Javascript required for the markup
        /// generated by this class to work properly.
        /// 
        /// This method is implemented via the calls:
        ///     LoadCSS(page);
        ///     LoadJavascript(page);
        /// </summary>
        public static void LoadCSSandJavascript(Page page)
        {
            LoadCSS(page);
            LoadJavascript(page);
        }


        /// <summary>
        /// Returns true if the given directory name is not
        /// in the internal list of secure directory names.
        /// 
        /// Assumes that the name does not contain any characters
        /// that might be part of a full path name.
        /// </summary>
        /// <param name="directoryName">The directory name</param>
        public static bool IsPublic(string directoryName)
        {
            foreach (string s in secure)
            {
                if (s.Equals(directoryName))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Returns true if the directory path should be
        /// publically served by sources server.
        /// 
        /// The directory path is an absolute path in the
        /// file system.
        /// 
        /// If fulltest is true, the path is fully tested.
        /// 
        /// Each name in the path is extracted and converted to
        /// lower case.  If the name matches one of the strings
        /// in the array secure then this method returns false.
        /// If no match is found then this method returns true.
        /// 
        /// If fulltest is false, then only the rightmost name
        /// in the path is tested.  This usage is appropriate
        /// if the directory path is being generated by a
        /// recursive process that will already have tested the
        /// earlier names in the path.
        /// </summary>
        /// <param name="directoryPath">
        ///     The directory path</param>
        /// <param name="fulltest">
        ///     If true, test all names in the path</param>
        public static bool IsPublic(string directoryPath, bool fulltest)
        {
            if (directoryPath == null)
                return false;

            string path = directoryPath;

            int n = path.Length;

            if (n == 0)
                return true;

            if ((path[n - 1] == backslash) || (path[n - 1] == colon))
            {
                n--;
                path = path.Substring(0, n);
            }

            if (n == 0)
                return true;

            int m = n - 1;

            while ((m >= 0) && (path[m] != backslash) && (path[m] != colon))
                m--;

            m++;

            string name = (m == 0)
                ? path
                : path.Substring(m);

            name = name.ToLower();

            foreach (string s in secure)
                if (s.Equals(name))
                    return false;

            if (fulltest)
                if (m > 0)
                    return IsPublic(path.Substring(0, m), true);

            return true;
        }


        /// <summary>
        /// Returns the full directory list of directories on
        /// the web site with the given rootpath.
        /// 
        /// The rootpath is an absolute path to the web site
        /// root and may be obtained using one of the methods
        /// FileTools.GetRoot.
        /// 
        /// This method returns absolute paths.
        /// </summary>
        public static List<string> FullDirectoryList(string rootpath)
        {
            return MakeDirectoryList(rootpath, false);
        }


        /// <summary>
        /// Returns the public directory list of directories
        /// on the web site with the given rootpath.
        /// 
        /// The directories returned are the ones for which
        /// the method IsPublic returns true.
        /// 
        /// The rootpath is an absolute path to the web site
        /// root and may be obtained using one of the methods
        /// FileTools.GetRoot.
        /// 
        /// This method returns absolute paths.
        /// </summary>
        public static List<string> PublicDirectoryList(string rootpath)
        {
            return MakeDirectoryList(rootpath, true);
        }


        /// <summary>
        /// Returns a directory list of directories on the
        /// web site with the given rootpath.
        /// 
        /// If onlyPublic is true then only public directories
        /// are returned.
        /// 
        /// If onlyPublic is false then all directoires are
        /// returned.
        /// 
        /// The rootpath is an absolute path to the web site
        /// root and may be obtained using one of the methods
        /// FileTools.GetRoot.
        /// 
        /// This method returns absolute paths.
        /// </summary>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static List<string> MakeDirectoryList
            (string rootpath, bool onlyPublic)
        {
            List<string> list = new List<string>();

            if (rootpath != null)
                MakeDirectoryListHelper(rootpath, list, onlyPublic);

            return list;
        }


        /// <summary>
        /// Recursive helper method for FullDirectoryList and
        /// PublicDirectoryList.
        /// 
        /// This method assumes that the directory path is valid
        /// and that the list argument is non-null.
        /// 
        /// The parameter onlyPublic determines whether to yield
        /// all directories or only the public ones.
        /// </summary>
        /// <param name="directoryPath">
        ///     The current directory in the recursive descent</param>
        /// <param name="list">
        ///     The list accumulating directories paths</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        static void MakeDirectoryListHelper
            (string directoryPath, List<string> list, bool onlyPublic)
        {
            int n = directoryPath.Length;
            bool ok = (directoryPath[n - 1] == backslash);

            string path = (ok) ? directoryPath : (directoryPath + backslash);

            if (onlyPublic)
                if (!IsPublic(path, false))
                    return;

            list.Add(path);

            DirectoryInfo thisdir = new DirectoryInfo(path);

            DirectoryInfo[] subdirs = thisdir.GetDirectories();

            foreach (DirectoryInfo di in subdirs)
                MakeDirectoryListHelper(di.FullName, list, onlyPublic);
        }


        /// <summary>
        /// Returns the list of files in the directory given by
        /// the directory absolute file path.
        /// 
        /// If viewable is true then returns only files known
        /// to be text or image files via their extension.
        /// </summary>
        /// <param name="directoryPath">
        ///     The directory file path</param>
        /// <param name="viewable">
        ///     If true return text or image files;
        ///     otherwise all files.</param>
        public static List<string> MakeFileList
            (string directoryPath, bool viewable)
        {
            if (viewable)
                return MakeFileList(directoryPath, FileTools.VIEWABLE);
            else
                return MakeFileList(directoryPath, FileTools.ALL);
        }


        /// <summary>
        /// Returns the list of files in the directory given by
        /// the directory absolute file path.
        /// 
        /// The fileType is one of
        ///   FileTools.TEXT
        ///   FileTools.IMAGE
        ///   FileTools.OTHER
        /// or an arithmetic-or combination of these constants,
        /// for example,
        ///   FileTools.VISIBLE
        ///   FileTools.ALL
        /// </summary>
        /// <param name="directoryPath">
        ///     The directory file path</param>
        /// <param name="fileType">
        ///     One of "TEXT", "IMAGE", "OTHER",
        ///     or an arithmetic-or combination.</param>
        public static List<string> MakeFileList
            (string directoryPath, int fileType)
        {
            List<string> filelist = new List<string>();

            if (directoryPath == null)
                return filelist;

            DirectoryInfo thisdir = new DirectoryInfo(directoryPath);

            FileInfo[] files = thisdir.GetFiles("*");

            foreach (FileInfo file in files)
            {
                string fileName = file.ToString();

                if ((FileTools.GetFileCategory(fileName) & fileType) == 0)
                    continue;

                filelist.Add(fileName);
            }

            return filelist;
        }


        /// <summary>
        /// Returns the list of directories short names that
        /// should not be publically served in sources server
        /// or related tools.
        /// 
        /// At present this list has the names:
        /// 
        ///   hidden
        ///   secret
        ///   secure
        ///   generatedimages
        /// 
        /// The last name is included to avoid serving the many
        /// images generated by the Deep Zoom Composer.  These
        /// images may be seen using the AutoDeepZoomViewer.
        /// </summary>
        public static List<string> GetDoNotServeList()
        {
            List<string> list = new List<string>();

            foreach (string s in secure)
            {
                list.Add(s);
            }

            return list;
        }


        /// <summary>
        /// Returns false if:
        ///   1. The tildePath is null.
        ///   2. The tildePath does not start with "~/".
        ///   3. The tildePath contains a character that cannot
        ///      occur in a Windows tilde file path, namely:
        ///        < > : " \ ? *
        /// 
        /// Furthermore, if onlyPublic is true, then returns false
        /// if any directory in the tildePath is a case insenstive
        /// match to one of the names returned by the method
        ///   GetDoNotServeList().
        /// 
        /// If none of these problems occurs then returns true.
        /// 
        /// Note that this method does not check whether the path
        /// exists.  That is done in the methods
        /// 
        ///   OKtoServeFile
        ///   OKtoServeDirectory
        ///   
        /// since different tests for existence must be made.
        /// 
        /// This method plays the role of a helper method for the
        /// two methods above.
        /// </summary>
        /// <param name="tildePath">
        ///     The tilde path to test</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static bool OKtoServe(string tildePath, bool onlyPublic)
        {
            if (tildePath == null)
                return false;

            if (!tildePath.StartsWith(tildeStart))
                return false;

            Regex exclude = new Regex(TildePathExcludeList);

            Match match = exclude.Match(tildePath);

            if (match.Success)
                return false;

            if (onlyPublic)
            {
                tildePath = tildePath.ToLower();

                string[] split = tildePath.Split(slash);

                int n = split.Length;

                // skip leading tilde

                for (int i = 1; i < n; i++)
                {
                    foreach (string skip in secure)
                    {
                        if (split[i].Equals(skip))
                            return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Returns true if:
        ///   1. OKtoServe returns true on this tilde path.
        ///   2. This tilde path represents a file that exists.
        /// 
        /// This design ensures that if the file should not be
        /// served then no information is given about whether
        /// or not the file exists.
        /// 
        /// The server object is needed because the tilde file
        /// path must be converted to an absolute file path via
        /// MapPath.
        /// </summary>
        /// <param name="page">
        ///     The server used for MapPath</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path to test</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static bool OKtoServeFile
            (HttpServerUtility server, string tildeFilePath, bool onlyPublic)
        {
            if (!OKtoServe(tildeFilePath, onlyPublic))
                return false;

            try
            {
                string filePath = server.MapPath(tildeFilePath);
                FileInfo info = new FileInfo(filePath);
                long bytes = info.Length;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Returns true if:
        ///   1. OKtoServe returns true on this tilde path.
        ///   2. This tilde path represents a directory that exists.
        /// 
        /// This design ensures that if the directory should not be
        /// served then no information is given about whether or not
        /// the directory exists.
        /// 
        /// If the tilde directory path does not end in /, this will
        /// be appended prior to testing.
        /// 
        /// The server object is needed because the tilde directory
        /// path must be converted to an absolute directory path via
        /// MapPath.
        /// </summary>
        /// <param name="page">
        ///     The server used for MapPath</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path to test</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static bool OKtoServeDirectory
            (HttpServerUtility server, string tildeDirectoryPath, bool onlyPublic)
        {
            if (!OKtoServe(tildeDirectoryPath, onlyPublic))
                return false;

            try
            {
                int n = tildeDirectoryPath.Length;

                if (tildeDirectoryPath[n - 1] != slash)
                    tildeDirectoryPath = tildeDirectoryPath + slash;

                string path = server.MapPath(tildeDirectoryPath);

                DirectoryInfo directory = new DirectoryInfo(path);

                int m = directory.GetFiles().Length;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Create the HTML markup to view the file if possible.
        /// 
        /// If the file is a text file then
        /// the markup will be enclosed in pre tags.
        /// 
        /// Precondition: The page that calls this method must:
        /// 
        ///   Be able to guarantee that the file is OK to serve
        ///   in context.
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path</param>
        /// <param name="numberLines">
        ///     Whether or not to number lines in a text file</param>
        public static string FileViewMarkup
            (Page page, string tildeFilePath, bool numberLines)
        {
            int category = FileTools.GetFileCategory(tildeFilePath);

            if (category == FileTools.OTHER)
                return NotViewable;

            StringBuilder builder = new StringBuilder();

            if (category == FileTools.TEXT)
            {
                string filePath = page.MapPath(tildeFilePath);
                string text = FileTools.GetFileAsText(filePath);
                string html = StringTools.TextToHtml(text, 4, numberLines);
                builder.Append(HTML_Tools.open_pre);
                builder.Append(html);
                builder.Append(HTML_Tools.shut_pre);
            }
            else
            {
                string imageURL = FileTools.GetRelativePath(page, tildeFilePath);

                builder.Append("<img src='");
                builder.Append(imageURL);
                builder.Append("' alt='");
                builder.Append(tildeFilePath);
                builder.Append("' title='");
                builder.Append(tildeFilePath);
                builder.Append("' />\n");
            }

            return builder.ToString();
        }


        /// <summary>
        /// Create a relative link from the given page to the
        /// file view page using the given tilde file path as
        /// the file to be viewed.
        /// 
        /// The markup text is the text that should be shown to
        /// the user in the link.  This should represent the
        /// tilde file path with HTML markup.
        /// 
        /// If the markup text is null, the tilde file path is
        /// used as the markup text.
        /// 
        /// Precondition: The page that calls this method must:
        /// 
        ///   Be able to guarantee that the file is OK to serve
        ///   in context.
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path</param>
        /// <param name="markupText">
        ///     The optional markup text is the tilde file path
        ///     with HTML markup</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        public static string FileViewLinkMarkup
            (Page page, string tildeFilePath, string markupText, bool onlyPublic)
        {
            if (StringTools.IsTrivial(markupText))
                markupText = tildeFilePath;

            StringBuilder builder = new StringBuilder();

            string fileViewURL = onlyPublic
                ? FileTools.GetRelativePath(page, FileViewTildePath)
                : FileTools.GetRelativePath(page, InternalFileViewTildePath);

            builder.Append("<a href ='");
            builder.Append(fileViewURL);
            builder.Append("?");
            builder.Append(tildeFilePath);
            builder.Append("' target='_blank'>");
            builder.Append(markupText);
            builder.Append("</a>");

            return builder.ToString();
        }


        /// <summary>
        /// Returns the markup for a file name header line.
        /// 
        /// The file name is given by the file tilde path.
        /// 
        /// The optional markup text is the file tilde path
        /// with HTML markup.
        /// 
        /// If the bool makeFileViewLink is true and if the
        /// file is a text or image file then use file view
        /// markup to create a link from the name to the
        /// file view page.
        /// 
        /// This markup will be enclosed in outer pre tags.
        /// 
        /// Precondition if makeFileViewLink is true:
        /// 
        ///   The page that calls this method must be able
        ///   to guarantee that the file is OK to serve in
        ///   context.
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path</param>
        /// <param name="markupText">
        ///     The optional markup text is the tilde file path
        ///     with HTML markup</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        /// <param name="makeFileViewLink">
        ///     Whether or not to make a file view link in header line</param>
        public static string HeaderLineMarkup
            (Page page, string tildeFilePath, string markupText,
             bool onlyPublic, bool makeFileViewLink)
        {
            StringBuilder builder = new StringBuilder();

            // Add the tildeFilePath or markupText inside a pre-tag
            //
            // Make this text a link to fileview if makeFileViewLink
            // is true and this file is viewable

            builder.Append(HTML_Tools.open_pre);

            if (StringTools.IsTrivial(markupText))
                markupText = tildeFilePath;

            int category = FileTools.GetFileCategory(tildeFilePath);

            bool viewable = category != FileTools.OTHER;

            if (makeFileViewLink && viewable)
                markupText =
                    FileViewLinkMarkup
                        (page, tildeFilePath, markupText, onlyPublic);

            builder.Append(markupText);

            builder.Append(HTML_Tools.shut_pre);

            builder.Append("\n");

            // Add a launch link if this file is servable
            //
            // Use tildeFilePath as the link text

            bool servable = HttpContextTools.IsServable(tildeFilePath);

            if (servable)
            {
                string launchURL = FileTools.GetRelativePath(page, tildeFilePath);

                builder.Append("<p>Launch ");
                builder.Append("<a href ='");
                builder.Append(launchURL);
                builder.Append("' target='_blank'>");
                builder.Append("<code>");
                builder.Append(tildeFilePath);
                builder.Append("</code>");
                builder.Append("</a>");
                builder.Append("</p>\n");
            }


            if (makeFileViewLink && !viewable)
            {
                builder.Append(NotViewable);
            }

            return builder.ToString();
        }


        /// <summary>
        /// Create the HTML markup to supply
        ///   * a file header line with optional file view link
        ///   * the file statistics
        ///   * the download button
        ///   * optional comment markup
        /// 
        /// If headerLine is true, the markup opens with the tilde
        /// file path or with the provided markup text.
        /// 
        /// The optional markup text is the tilde file path with
        /// HTML markup to be used in the headerLine.
        /// 
        /// Moreover, if the file is viewable and makeFileViewLink
        /// is true, then this header line provides a link to view
        /// the file.
        /// 
        /// If statistics is true, the markup shows a table with
        /// the file statistics.
        /// 
        /// If download is true, the markup for a download button
        /// is created.
        /// 
        /// If the string comment is not trivial then it is added
        /// to the markup at the end.
        /// 
        /// 
        /// The out statistics parameters are always computed.
        /// 
        /// If the caller wishes to simply obtain the statistics
        /// parameters then set the following bool parameters to
        /// false:
        ///   headerLine
        ///   makeFileViewLink
        ///   statistics
        ///   download
        /// Also, set the comment string to null.  Then no work
        /// will be done on markup and the return value will be
        /// the empty string.
        /// 
        /// 
        /// Preconditions: For nice markup to occur, the page that
        /// calls this method must:
        /// 
        ///   1. Have executed the call
        /// 
        ///          SourceTools.LoadCSS(this);
        ///     
        ///      during the initial call to PageLoad
        ///      or during an override to OnPreRender.
        /// 
        ///   2. If the download parameter is true, then
        ///      have executed the call
        /// 
        ///          SourceTools.LoadJavascript(this);
        ///     
        ///      during the initial call to PageLoad
        ///      or during an override to OnPreRender.
        /// 
        /// In addition, the page must:
        /// 
        ///   3. Be able to guarantee that the file is OK to serve
        ///      in context.
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path</param>
        /// <param name="markupText">
        ///     The optional markup text is the tilde file path
        ///     with HTML markup</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        /// <param name="headerLine">
        ///     Whether or not to show the header line</param>
        /// <param name="makeFileViewLink">
        ///     Whether or not to make a file view link in the header line</param>
        /// <param name="statistics">
        ///     Whether or not to show file statistics</param>
        /// <param name="download">
        ///     Whether or not to create a download button</param>
        /// <param name="comment">
        ///     Optional HTML markup to be appended to the
        ///     auto-generated markup constructed by this method</param>
        /// <param name="bytes">
        ///     out: Bytes in the file</param>
        /// <param name="created">
        ///     out: File created DateTime</param>
        /// <param name="modified">
        ///     out: File modified DateTime</param>
        public static string StatisticsMarkup
            (Page page,
                string tildeFilePath,
                string markupText,
                bool onlyPublic,
                bool headerLine,
                bool makeFileViewLink,
                bool statistics,
                bool download,
                string comment,
                out long bytes,
                out DateTime? created,
                out DateTime? modified)
        {
            // Get file info parameters
            string filePath = page.MapPath(tildeFilePath);

            FileInfo info = new FileInfo(filePath);

            bytes = info.Length;
            created = info.CreationTime;
            modified = info.LastWriteTime;


            // Check to see if markup is to be created
            bool appendComment = !StringTools.IsTrivial(comment);

            bool createMarkup =
                headerLine || statistics || download || appendComment;

            if (!createMarkup)
                return "";


            // Create markup
            StringBuilder builder = new StringBuilder();

            if (headerLine)
            {
                builder.Append
                    (HeaderLineMarkup
                        (page, tildeFilePath, markupText,
                         onlyPublic, makeFileViewLink));
            }

            if (statistics)
            {
                string a = "";
                string b = "";

                builder.Append("<table class='filedata'>\n");

                a = bytes.ToString();
                HTML_Tools.AppendTableRow(builder, "Bytes", a);

                a = created.Value.ToYMD();
                b = created.Value.ToHMS();

                HTML_Tools.AppendTableRow(builder, "Created", a, b);

                a = modified.Value.ToYMD();
                b = modified.Value.ToHMS();

                HTML_Tools.AppendTableRow(builder, "Modified", a, b);

                int category = FileTools.GetFileCategory(tildeFilePath);

                if (category == FileTools.IMAGE)
                {
                    int width = 0;
                    int height = 0;

                    if (category == FileTools.IMAGE)
                    {
                        using (FileStream stream =
                            new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            try
                            {
                                Bitmap bitmap = new Bitmap(stream);
                                width = bitmap.Width;
                                height = bitmap.Height;
                            }
                            catch { }
                        }
                    }

                    a = "" + width + " &times; " + height;

                    HTML_Tools.AppendTableRow(builder, "Dimensions", a);
                }

                builder.Append("</table>\n");
            }

            if (download && onlyPublic)
            {
                builder.Append(Download1);
                builder.Append(tildeFilePath);
                builder.Append(Download2);
            }

            if (appendComment)
            {
                builder.Append("\n<div>");
                builder.Append(comment);
                builder.Append("</div>\n");
            }

            return builder.ToString();
        }


        /// <summary>
        /// Returns the statistics markup for an entire directory.
        /// 
        /// The tilde directory path will be truncated if need be
        /// to obtain a path that ends in slash.
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="tildeDirectoryPath">
        ///     The tilde directory path</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        /// <param name="viewable">
        ///     If true examine only viewable files</param>
        /// <param name="showfiles">
        ///     If true create individual file markup</param>
        /// <param name="summarize">
        ///     If true create summary statistics markup</param>
        /// <param name="fileCount">
        ///     out: The count of files in the directory</param>
        /// <param name="totalBytes">
        ///     out: The total bytes in all files in the directory</param>
        /// <param name="createdFirst">
        ///     out: The earliest creation date</param>
        /// <param name="createdLast">
        ///     out: The most recent creation date</param>
        /// <param name="modifiedFirst">
        ///     out: The earliest modification date</param>
        /// <param name="modifiedLast">
        ///     out: The most recent modification date</param>
        public static string StatisticsMarkupForDirectory
            (Page page,
                string tildeDirectoryPath,
                bool onlyPublic,
                bool viewable,
                bool showfiles,
                bool summarize,
                out int fileCount,
                out long totalBytes,
                out DateTime? createdFirst,
                out DateTime? createdLast,
                out DateTime? modifiedFirst,
                out DateTime? modifiedLast)
        {
            int fileType = viewable ? FileTools.VIEWABLE : FileTools.ALL;

            return StatisticsMarkupForDirectory
                (page,
                    tildeDirectoryPath,
                    onlyPublic,
                    fileType,
                    showfiles,
                    summarize,
                    out fileCount,
                    out totalBytes,
                    out createdFirst,
                    out createdLast,
                    out modifiedFirst,
                    out modifiedLast);
        }


        /// <summary>
        /// Returns the statistics markup for an entire directory.
        /// 
        /// The tilde directory path will be truncated if need be
        /// to obtain a path that ends in slash.
        /// 
        /// The fileType is one of
        ///   FileTools.TEXT
        ///   FileTools.IMAGE
        ///   FileTools.OTHER
        /// or an arithmetic-or combination of these constants,
        /// for example,
        ///   FileTools.VISIBLE
        ///   FileTools.ALL
        /// 
        /// This function may be used to obtain data about the
        /// files in the directory without creating markup.
        /// 
        /// The individual file markup will be created only if
        /// showfiles is true.
        /// 
        /// The summary markup will be created only if summarize
        /// is true.
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="tildeDirectoryPath">
        ///     The tilde directory path</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        /// <param name="fileType">
        ///     One of
        ///         FileTools.TEXT
        ///         FileTools.IMAGE
        ///         FileTools.OTHER",
        ///     or an arithmetic-or combination such as
        ///         FileTools.VISIBLE
        ///         FileTools.ALL
        /// </param>
        /// <param name="showfiles">
        ///     If true create individual file markup</param>
        /// <param name="summarize">
        ///     If true create summary statistics markup</param>
        /// <param name="fileCount">
        ///     out: The count of files in the directory</param>
        /// <param name="totalBytes">
        ///     out: The total bytes in all files in the directory</param>
        /// <param name="createdFirst">
        ///     out: The earliest creation date</param>
        /// <param name="createdLast">
        ///     out: The most recent creation date</param>
        /// <param name="modifiedFirst">
        ///     out: The earliest modification date</param>
        /// <param name="modifiedLast">
        ///     out: The most recent modification date</param>
        public static string StatisticsMarkupForDirectory
            (Page page,
                string tildeDirectoryPath,
                bool onlyPublic,
                int fileType,
                bool showfiles,
                bool summarize,
                out int fileCount,
                out long totalBytes,
                out DateTime? createdFirst,
                out DateTime? createdLast,
                out DateTime? modifiedFirst,
                out DateTime? modifiedLast)
        {
            // Initialize out parameters
            fileCount = 0;
            totalBytes = 0;

            createdFirst = null;
            createdLast = null;

            modifiedFirst = null;
            modifiedLast = null;


            // Trivial returns
            if (page == null)
                return NoFile;

            if (StringTools.IsTrivial(tildeDirectoryPath))
                return NoFile;

            if (!tildeDirectoryPath.StartsWith(tildeStart))
                return NoFile;


            // Make sure tilde directory path ends in slash
            int n = tildeDirectoryPath.Length;
            int m = n - 1;

            while (tildeDirectoryPath[m] != slash)
                --m;

            if (m < (n - 1))
            {
                tildeDirectoryPath = tildeDirectoryPath.Substring(0, m + 1);
            }


            // Attempt to extract files in the directory

            List<string> fileNameList = null;

            try
            {
                string directoryPath = page.MapPath(tildeDirectoryPath);
                fileNameList = MakeFileList(directoryPath, fileType);
            }
            catch (Exception)
            {
                return NoFile;
            }

            fileCount = fileNameList.Count;

            if (fileCount == 0)
                return NoFile;


            StringBuilder builder = new StringBuilder();
            StringBuilder summarybuilder = new StringBuilder();

            // Traverse files
            //
            // If showfiles is true
            //     create and append statistics markup for each file
            //
            // In all cases, accumulate summary data

            foreach (string fileName in fileNameList)
            {
                string tildeFilePath = tildeDirectoryPath + fileName;

                long bytes = 0;

                DateTime? created = null;
                DateTime? modified = null;

                // Find file information
                // If showfiles is true then create markup
                string markup =
                    SourceTools.StatisticsMarkup
                        (page,
                            tildeFilePath,
                            null,
                            onlyPublic,
                            showfiles,
                            showfiles,
                            showfiles,
                            showfiles,
                            null,
                            out bytes,
                            out created,
                            out modified);

                // If showfiles is true then append markup
                if (showfiles)
                {
                    builder.Append(markup);

                    builder.Append(HTML_Tools.hr);
                }

                // Accumulate file information in summary parameters
                if (bytes > 0)
                {
                    totalBytes += bytes;

                    if (createdFirst.HasValue)
                    {
                        if (created.Value.CompareTo(createdFirst.Value) < 0)
                            createdFirst = created;

                        if (created.Value.CompareTo(createdLast.Value) > 0)
                            createdLast = created;

                        if (modified.Value.CompareTo(modifiedFirst.Value) < 0)
                            modifiedFirst = modified;

                        if (modified.Value.CompareTo(modifiedLast.Value) > 0)
                            modifiedLast = modified;
                    }
                    else
                    {
                        createdFirst = created;
                        createdLast = created;

                        modifiedFirst = modified;
                        modifiedLast = modified;
                    }
                }
            }

            // If summarize is true then append summary markup
            // to summarybuilder
            if (summarize)
            {
                AppendStatisticsSummary(
                    summarybuilder,
                    fileCount,
                    totalBytes,
                    createdFirst,
                    createdLast,
                    modifiedFirst,
                    modifiedLast);
            }
            
            // Show summary before details
            return summarybuilder.ToString() + builder.ToString();
        }


        /// <summary>
        /// Appends to the given builder the markup for an HTML
        /// table which displays the information in the rest of
        /// the parameters.
        /// </summary>
        /// <param name="builder">
        ///     The builder to which the summary is appended</param>
        /// <param name="fileCount">
        ///     The count of files processed</param>
        /// <param name="totalBytes">
        ///     The total bytes in all files processed</param>
        /// <param name="createdFirst">
        ///     The earliest creation date</param>
        /// <param name="createdLast">
        ///     The most recent creation date</param>
        /// <param name="modifiedFirst">
        ///     The earliest modification date</param>
        /// <param name="modifiedLast">
        ///     The most recent modification date</param>
        public static void AppendStatisticsSummary(
            StringBuilder builder,
            int fileCount,
            long totalBytes,
            DateTime? createdFirst,
            DateTime? createdLast,
            DateTime? modifiedFirst,
            DateTime? modifiedLast)
        {
            if (totalBytes == 0)
                return;

            builder.Append("<table class='filedata'>\n");

            string a = "";
            string b = "";

            a = fileCount.ToString();
            b = totalBytes.ToString();

            HTML_Tools.AppendTableRow(builder, "Total Files", a);
            HTML_Tools.AppendTableRow(builder, "Total Bytes", b);

            a = createdFirst.Value.ToYMD();
            b = createdFirst.Value.ToHMS();

            HTML_Tools.AppendTableRow(builder, "Created First", a, b);

            a = createdLast.Value.ToYMD();
            b = createdLast.Value.ToHMS();

            HTML_Tools.AppendTableRow(builder, "Created Last", a, b);

            a = modifiedFirst.Value.ToYMD();
            b = modifiedFirst.Value.ToHMS();

            HTML_Tools.AppendTableRow(builder, "Modified First", a, b);

            a = modifiedLast.Value.ToYMD();
            b = modifiedLast.Value.ToHMS();

            HTML_Tools.AppendTableRow(builder, "Modified Last", a, b);
 
            builder.Append("</table>\n");

            builder.Append(HTML_Tools.hr);
        }


        /// <summary>
        /// Returns the statistics markup for an entire site.
        /// 
        /// If showlinks is true, the markup contains a list
        /// of links to the statistics server initialized to
        /// each particular directory.
        /// 
        /// In addition, the summary statistics for the
        /// directory are shown below the corresponding link.
        /// 
        /// If summarize is true, cumulative summary satistics
        /// are shown for the entire site.
        /// 
        /// The out parameters always return valid data for the
        /// entire site even if no markup is generated.
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="onlyPublic">
        ///     Whether or not to restrict to public directories</param>
        /// <param name="showlinks">
        ///     If true create links to statistics server for
        ///     each directory and show directory statistics</param>
        /// <param name="summarize">
        ///     If true create summary statistics markup for
        ///     the entire site</param>
        /// <param name="fileCount">
        ///     out: The count of files in the site</param>
        /// <param name="totalBytes">
        ///     out: The total bytes in all files in the site</param>
        /// <param name="createdFirst">
        ///     out: The earliest creation date</param>
        /// <param name="createdLast">
        ///     out: The most recent creation date</param>
        /// <param name="modifiedFirst">
        ///     out: The earliest modification date</param>
        /// <param name="modifiedLast">
        ///     out: The most recent modification date</param>
        public static string StatisticsMarkupForSite
            (Page page,
                bool onlyPublic,
                bool showlinks,
                bool summarize,
                out int fileCount,
                out long totalBytes,
                out DateTime? createdFirst,
                out DateTime? createdLast,
                out DateTime? modifiedFirst,
                out DateTime? modifiedLast)
        {
            // Initialize out parameters
            fileCount = 0;
            totalBytes = 0;

            createdFirst = null;
            createdLast = null;

            modifiedFirst = null;
            modifiedLast = null;

            // Define corresponding parameters for individual directories
            int d_fileCount = 0;
            long d_totalBytes = 0;

            DateTime? d_createdFirst = null;
            DateTime? d_createdLast = null;

            DateTime? d_modifiedFirst = null;
            DateTime? d_modifiedLast = null;

            // Find the list of tilde directory paths
            string rootpath = FileTools.GetRoot(page);

            string rootoffset = FileTools.GetRootOffset(page);

            List<string> directoryList =
                MakeDirectoryList(rootpath, onlyPublic);

            List<string> tildeDirectoryList =
                FileTools.GetTildePaths(rootpath, directoryList);

            // Begin to accumulate statistics and markup
            StringBuilder builder = new StringBuilder();
            StringBuilder summarybuilder = new StringBuilder();

            string markup = "";

            string statisticsPath =
                rootoffset + StatisticsTildePath.Substring(2);

            bool d_showfiles = false;
            bool d_summarize = showlinks;

            foreach (string tildeDirectoryPath in tildeDirectoryList)
            {
                markup = StatisticsMarkupForDirectory(
                    page,
                    tildeDirectoryPath,
                    onlyPublic,
                    FileTools.ALL,
                    d_showfiles,
                    d_summarize,
                    out d_fileCount,
                    out d_totalBytes,
                    out d_createdFirst,
                    out d_createdLast,
                    out d_modifiedFirst,
                    out d_modifiedLast);

                if (showlinks)
                {
                    AppendStatisticsLink(
                        builder,
                        tildeDirectoryPath,
                        statisticsPath,
                        markup);
                }

                // Accumulate file information in summary parameters
                if (d_totalBytes == 0)
                {
                    continue;
                }

                if (totalBytes == 0)
                {
                    fileCount = d_fileCount;
                    totalBytes = d_totalBytes;
                    createdFirst = d_createdFirst;
                    createdLast = d_createdLast;
                    modifiedFirst = d_modifiedFirst;
                    modifiedLast = d_modifiedLast;
                }
                else
                {
                    fileCount += d_fileCount;
                    totalBytes += d_totalBytes;

                    if (d_createdFirst.Value.CompareTo(createdFirst.Value) < 0)
                        createdFirst = d_createdFirst;

                    if (d_createdLast.Value.CompareTo(createdLast.Value) > 0)
                        createdLast = d_createdLast;

                    if (d_modifiedFirst.Value.CompareTo(modifiedFirst.Value) < 0)
                        modifiedFirst = d_modifiedFirst;

                    if (d_modifiedLast.Value.CompareTo(modifiedLast.Value) > 0)
                        modifiedLast = d_modifiedLast;
                }
            }

            // If summarize is true then append summary markup
            // to summarybuilder
            if (summarize)
            {
                AppendStatisticsSummary(
                    summarybuilder,
                    fileCount,
                    totalBytes,
                    createdFirst,
                    createdLast,
                    modifiedFirst,
                    modifiedLast);
            }

            // Show summary before details
            return summarybuilder.ToString() + builder.ToString();
        }


        /// <summary>
        /// Append a link constructed as follows.
        /// 
        /// The href begins with the relative path statisticPath
        /// and ends with a query string with tildeDirectoryPath.
        /// 
        /// The link text is tildeDirectoryPath.
        /// 
        /// The link opens in a new tab/window.
        /// 
        /// After the link is appended, the markup is appended.
        /// 
        /// Finally, a horizontal rule is appended.
        /// </summary>
        /// <param name="builder">
        ///     The builder to which the html is appended</param>
        /// <param name="tildeDirectoryPath">
        ///     The target directory</param>
        /// <param name="statisticsPath">
        ///     The relative path to statistics server</param>
        /// <param name="markup">
        ///     The additional markup</param>
        public static void AppendStatisticsLink(
            StringBuilder builder,
            string tildeDirectoryPath,
            string statisticsPath,
            string markup)
        {
            builder.Append("\n<p><a href='");
            builder.Append(statisticsPath);
            builder.Append("?");
            builder.Append(tildeDirectoryPath);
            builder.Append("' target='_blank'><code>");
            builder.Append(tildeDirectoryPath);
            builder.Append("</code></a></p>\n");
            builder.Append(markup);
        }


        /// <summary>
        /// Create the HTML markup for a download button that
        /// corresponds to the file with the given tilde file
        /// path.
        /// 
        /// Preconditions: The page that calls this method must:
        /// 
        ///   1. Have executed the call
        /// 
        ///          SourceTools.LoadCSSandJavascript(this);
        ///      
        ///      or the equivalent pair of calls
        ///      
        ///          SourceTools.LoadCSS(this);
        ///          SourceTools.LoadJavascript(this);
        ///        
        ///      during the initial call to PageLoad. 
        /// 
        ///   2. Be able to guarantee that the file is OK to serve
        ///      as a publically accessible file.
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="tildeFilePath">
        ///     The tilde file path</param>
        public static string DownloadButtonMarkup
            (Page page, string tildeFilePath)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Download1);
            builder.Append(tildeFilePath);
            builder.Append(Download2);

            return builder.ToString();
        }


        /// <summary>
        /// Create the HTML markup to make a file list with Download
        /// buttons for facilitating the download of muliple files.
        /// 
        /// The method requires the tilde file path of an XML file
        /// that is structured along the lines of:
        /// 
        ///     ~/app_data/download_template.xml
        /// 
        /// The XML file should normally be in ~/app_data or in
        /// some subdirectory thereof.
        /// 
        /// All tilde paths within the XML file
        /// must refer to publically accessible directories.
        /// 
        /// Precondition: The page that calls this method must:
        /// 
        /// Have executed the call
        /// 
        ///     SourceTools.LoadCSSandJavascript(this);
        ///      
        /// or the equivalent pair of calls
        ///      
        ///     SourceTools.LoadCSS(this);
        ///     SourceTools.LoadJavascript(this);
        ///        
        /// during the initial call to PageLoad. 
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="xmlTildeFilePath">
        ///     The tilde path for the XML file
        ///     with the download file list information</param>
        public static string DownloadFileListMarkup
            (Page page, string xmlTildeFilePath)
        {
            try
            {
                string xmlPath = page.MapPath(xmlTildeFilePath);
                string xmlString = FileTools.GetFileAsText(xmlPath);

                return DownloadFileListMarkupUsingXmlString(page, xmlString);
            }
            catch (Exception)
            {
                return DownloadFileListError;
            }
        }


        /// <summary>
        /// Create the HTML markup to make a file list with Download
        /// buttons for facilitating the download of muliple files.
        /// 
        /// The method requires a string with XML that is structured
        /// along the lines of:
        /// 
        ///     ~/app_data/download_template.xml
        /// 
        /// All tilde paths within the XML string
        /// must refer to publically accessible directories.
        /// 
        /// Precondition: The page that calls this method must:
        /// 
        /// Have executed the call
        /// 
        ///     SourceTools.LoadCSSandJavascript(this);
        ///      
        /// or the equivalent pair of calls
        ///      
        ///     SourceTools.LoadCSS(this);
        ///     SourceTools.LoadJavascript(this);
        ///        
        /// during the initial call to PageLoad. 
        /// </summary>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="xmlString">
        ///     The XML string
        ///     with the download file list information</param>
        public static string DownloadFileListMarkupUsingXmlString
            (Page page, string xmlString)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlString);

                XmlNode homenode = document.ChildNodes[1];
                XmlNodeList childnodes = homenode.ChildNodes;

                StringBuilder builder = new StringBuilder();

                foreach (XmlNode node in childnodes)
                {
                    string name = node.Name.ToLower();

                    if (name.Equals("discussion"))
                    {
                        builder.Append("\n<p>");
                        builder.Append(node.InnerText);
                        builder.Append("</p>\n");
                        builder.Append("<hr />");
                        continue;
                    }

                    if (name.Equals("filelist"))
                    {
                        XmlNodeList filenodes = node.ChildNodes;

                        foreach (XmlNode filenode in filenodes)
                            ProcessFileNode(builder, page, filenode);
                    }
                }

                return builder.ToString();
            }
            catch (Exception)
            {
                return DownloadFileListError;
            }
        }


        /// <summary>
        /// Helper method of the DownloadFileListMarkup methods.
        /// 
        /// All tilde paths within the XML file node
        /// must refer to publically accessible directories.
        /// </summary>
        /// <param name="builder">
        ///     The string builder that accumulates markup</param>
        /// <param name="page">
        ///     The page that will use the markup</param>
        /// <param name="filenode">
        ///     The XmlNode for one file to download</param>
        static void ProcessFileNode
            (StringBuilder builder, Page page, XmlNode filenode)
        {
            string name = filenode.Name.ToLower();

            if (!name.Equals("file"))
                return;

            XmlNodeList datanodes = filenode.ChildNodes;

            string tildepath = "";
            string comment = "";

            foreach (XmlNode data in datanodes)
            {
                name = data.Name.ToLower();

                if (name.Equals("tildepath"))
                    tildepath = data.InnerText;

                if (name.Equals("comment"))
                    comment = data.InnerText;
            }

            if (tildepath.Length == 0)
            {
                builder.Append(DownloadFailure1);
                builder.Append(NoPath);
            }
            else
            {
                long bytes = 0;
                DateTime? created = null;
                DateTime? modified = null;

                try
                {
                    builder.Append
                        (StatisticsMarkup
                            (page, tildepath, null,
                             true, true, true, true, true,
                             comment,
                             out bytes,
                             out created, out modified));
                }
                catch (Exception)
                {
                    builder.Append(DownloadFailure1);
                    builder.Append(DownloadFailure2);
                    builder.Append(tildepath);
                    builder.Append(DownloadFailure3);
                }
            }

            builder.Append("<hr />");
        }
    }
}