/*
    File: ~/app_code/FileTools.cs
    
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

namespace edu.neu.ccis.rasala
{
    public class FileTools
    {
        /// <summary>
        /// This constant mask signals a text file
        /// </summary>
        public const int TEXT = 1;

        /// <summary>
        /// This constant mask signals an image file
        /// </summary>
        public const int IMAGE = 2;

        /// <summary>
        /// This constant mask signals a file that is not
        /// known to be either a text or an image file
        /// </summary>
        public const int OTHER = 4;

        /// <summary>
        /// This constant signals a viewable file, that
        /// is, a text file or an image file.
        /// </summary>
        public const int VIEWABLE = TEXT | IMAGE;

        /// <summary>
        /// This constant captures all files.
        /// </summary>
        public const int ALL = TEXT | IMAGE | OTHER;


        public const char slash = '/';

        public const char backslash = '\\';


        public const string httpStart = "http://";

        public const string httpsStart = "https://";

        public const string tildeStart = "~/";

        public const string upOneStart = "../";


        static string[] text_extensions =
        {
            ".txt",
            ".cs",
            ".vb",
            ".c",
            ".h",
            ".cpp",
            ".jsl",
            ".java",
            ".js",
            ".vbs",
            ".wsf",
            ".htm",
            ".html",
            ".hta",
            ".htc",
            ".css",
            ".xml",
            ".xsd",
            ".xsp",
            ".xsl",
            ".xslt",
            ".asp",
            ".aspx",
            ".asmx",
            ".ascx",
            ".asax",
            ".ashx",
            ".config",
            ".wsdl",
            ".disco",
            ".discomap",
            ".sitemap",
            ".master",
            ".sql",
            ".xaml",
            ".csproj",
            ".user",
            ".svg"
        };


        static string[] image_extensions =
        {
            ".bmp",
            ".jpg",
            ".jpeg",
            ".gif",
            ".png",
            ".ico"
        };


        static Dictionary<string, int> extensions_hash =
            new Dictionary<string, int>();


        /// <summary>
        /// Initializes extensions_hash.
        /// </summary>
        static FileTools()
        {
            foreach (string ext in text_extensions)
                extensions_hash.Add(ext, TEXT);

            foreach (string ext in image_extensions)
                extensions_hash.Add(ext, IMAGE);
        }


        /// <summary>
        /// Returns a category TEXT, IMAGE, OTHER for the given
        /// filename depending on its file extension.
        ///
        /// If there is a problem with the filename, returns
        /// OTHER.
        /// </summary>
        /// <param name="filename">The file name to test</param>
        /// <returns>The file category</returns>
        public static int GetFileCategory(string filename)
        {
            if (filename == null)
                return OTHER;

            int index = filename.LastIndexOf('.');

            if (index == -1)
                return OTHER;

            string ext = filename.Substring(index).ToLower();

            int category;

            if (extensions_hash.TryGetValue(ext, out category))
                return category;

            return OTHER;
        }


        /// <summary>
        /// Returns the file extensions that this toolkit
        /// recognizes as text files.
        /// </summary>
        /// <returns>A list of recognized text extensions</returns>
        public static List<string> GetTextExtensions()
        {
            List<string> list = new List<string>();

            foreach (string s in text_extensions)
            {
                list.Add(s);
            }

            return list;
        }


        /// <summary>
        /// Returns the file extensions that this toolkit
        /// recognizes as image files.
        /// 
        /// This list is restricted to those image files
        /// that are known to be directly displayable in
        /// a web browser.
        /// </summary>
        /// <returns>A list of recognized image extensions</returns>
        public static List<string> GetImageExtensions()
        {
            List<string> list = new List<string>();

            foreach (string s in image_extensions)
            {
                list.Add(s);
            }

            return list;
        }


        /// <summary>
        /// Returns the root directory of the web site
        /// that contains this Page object.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="page">A web site Page object</param>
        /// <returns>The root directory</returns>
        public static string GetRoot(Page page)
        {
            if (page == null)
                return "";

            return page.MapPath(tildeStart);
        }


        /// <summary>
        /// Returns the root directory of the web site
        /// using an HttpContext object.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="server">An HttpContext object</param>
        /// <returns>The root directory</returns>
        public static string GetRoot(HttpContext context)
        {
            if (context == null)
                return "";

            return context.Server.MapPath(tildeStart);
        }


        /// <summary>
        /// Returns the root directory of the web site
        /// using an HttpServerUtility object.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="server">An HttpServerUtility object</param>
        /// <returns>The root directory</returns>
        public static string GetRoot(HttpServerUtility server)
        {
            if (server == null)
                return "";

            return server.MapPath(tildeStart);
        }


        /// <summary>
        /// Returns the parent of the root directory of the web site
        /// that contains this page.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="page">A web site Page object</param>
        /// <returns>The directory of the parent of the root</returns>
        public static string GetRootParent(Page page)
        {
            if (page == null)
                return "";

            return GetRootParent(page.Server);
        }


        /// <summary>
        /// Returns the parent of the root directory of the web site
        /// using an HttpContext object.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="server">An HttpContext object</param>
        /// <returns>The directory of the parent of the root</returns>
        public static string GetRootParent(HttpContext context)
        {
            if (context == null)
                return "";

            return GetRootParent(context.Server);
        }


        /// <summary>
        /// Returns the parent of the root directory of the web site
        /// using an HttpServerUtility object.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="server">An HttpServerUtility object</param>
        /// <returns>The directory of the parent of the root</returns>
        public static string GetRootParent(HttpServerUtility server)
        {
            if (server == null)
                return "";

            string root = GetRoot(server);

            int length = root.Length;

            int index = length - 2;

            while ((index >= 0) && (root[index] != backslash))
                index--;

            if (index < 0)
                return "";

            return root.Substring(0, index + 1);
        }


        /// <summary>
        /// Returns the directory path
        /// corresponding to this page object
        /// and adds a trailing back slash if need be.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="page">A web site Page object</param>
        /// <returns>The associated directory</returns>
        public static string GetDirectoryPath(Page page)
        {
            if (page == null)
                return "";

            return GetDirectoryPath(page.Server);
        }


        /// <summary>
        /// Returns the directory path
        /// corresponding to this context object
        /// and adds a trailing back slash if need be.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="page">A web site HttpContext object</param>
        /// <returns>The associated directory</returns>
        public static string GetDirectoryPath(HttpContext context)
        {
            if (context == null)
                return "";

            return GetDirectoryPath(context.Server);
        }


        /// <summary>
        /// Returns the directory path
        /// corresponding to this server object
        /// and adds a trailing back slash if need be.
        /// 
        /// Returns an empty string if an error occurs.
        /// </summary>
        /// <param name="page">A web site HttpServerUtility object</param>
        /// <returns>The associated directory</returns>
        public static string GetDirectoryPath(HttpServerUtility server)
        {
            if (server == null)
                return "";

            string result = server.MapPath("");

            int n = result.Length;

            if (n > 0)
                if (result[n - 1] != backslash)
                    result += backslash;

            return result;
        }


        /// <summary>
        /// Returns the string obtained from the given file path
        /// by stripping the root path from the file path.
        /// 
        /// Returns an empty string if the given file path does
        /// not descend from the root path.
        /// 
        /// Note that the root path may be obtained from one of
        /// the GetRoot methods.
        /// </summary>
        /// <param name="rootpath">The web site root path</param>
        /// <param name="filepath">A file path in the web site</param>
        /// <returns>The file path from the root downwards</returns>
        public static string GetPathFromRoot(string rootpath, string filepath)
        {
            if ((rootpath == null) || (filepath == null))
                return "";

            int n = rootpath.Length;

            if (n == 0)
                return "";

            if (filepath.StartsWith(rootpath))
                return filepath.Substring(n);
            else
                return "";
        }


        /// <summary>
        /// Returns the string obtained from the given file path
        /// by stripping the root path from the file path
        /// and then replacing each backslash with a slash.
        /// 
        /// This method may be used to create relative URLs.
        /// 
        /// Returns an empty string if the given file path does
        /// not descend from the root path.
        /// 
        /// Note that the root path may be obtained from one of
        /// the GetRoot methods.
        /// </summary>
        /// <param name="rootpath">The web site root path</param>
        /// <param name="filepath">A file path in the web site</param>
        /// <returns>The file path from the root downwards using slash as the separator</returns>
        public static string GetPathFromRootWithSlash(string rootpath, string filepath)
        {
            return BackSlashToSlash(GetPathFromRoot(rootpath, filepath));
        }


        /// Returns the string obtained from the given file path
        /// by stripping the root path from the file path,
        /// then replacing each backslash with a slash,
        /// then adding "~/" in front of the result.
        /// 
        /// This method may be used to create tilde paths that
        /// may later be fed to some variation of Server.MapPath
        /// or may be used to create relative URLs.
        /// 
        /// Returns "~/" if the given file path does not descend
        /// from the root path.
        /// 
        /// Note that the root path may be obtained from one of
        /// the GetRoot methods.
        /// </summary>
        /// <param name="rootpath">The web site root file path</param>
        /// <param name="filepath">A file path in the web site</param>
        /// <returns>The tilde path corresponding to the file path</returns>
        public static string GetTildePath(string rootpath, string filepath)
        {
            return tildeStart + GetPathFromRootWithSlash(rootpath, filepath);
        }


        /// <summary>
        /// From the given web site root path and the given
        /// list of absolute file paths, create and return the
        /// corresponding list of tilde paths using the method
        /// GetTildePath.
        /// 
        /// Note that the root path may be obtained from one of
        /// the GetRoot methods.
        /// </summary>
        /// <param name="rootpath">The web site root file path</param>
        /// <param name="filepaths">A list of file paths in the web site</param>
        /// <returns></returns>
        public static List<string> GetTildePaths(string rootpath, List<string> filepaths)
        {
            List<string> tildepaths = new List<string>();

            foreach (string path in filepaths)
                tildepaths.Add(GetTildePath(rootpath, path));

            return tildepaths;
        }


        /// <summary>
        /// Returns the path to the directory of this page
        /// as a tilde path.
        /// 
        /// Returns "~/" if the given page is null.
        /// </summary>
        /// <param name="page">A web site Page object</param>
        public static string GetTildePath(Page page)
        {
            if (page == null)
                return tildeStart;

            string rootpath = GetRoot(page);
            string pagepath = GetDirectoryPath(page);
            return GetTildePath(rootpath, pagepath);
        }


        /// <summary>
        /// Returns page.Request.FilePath
        /// 
        /// Returns "" if the given page is null.
        /// </summary>
        /// <param name="page">A web site Page object</param>
        public static string GetFilePath(Page page)
        {
            if (page == null)
                return "";

            return page.Request.FilePath;
        }


        /// <summary>
        /// Returns the file name extracted from
        /// page.Request.FilePath
        /// 
        /// Returns "" if the given page is null.
        /// </summary>
        /// <param name="page">A web site Page object</param>
        public static string GetFileName(Page page)
        {
            if (page == null)
                return "";

            string filepath = page.Request.FilePath;

            int n = filepath.Length;
            int m = n - 1;

            while ((m >= 0) && (filepath[m] != slash))
                --m;

            ++m;

            return filepath.Substring(m);
        }


        /// <summary>
        /// Returns the full tilde path of the page, that is:
        ///     GetTildePath(page) + GetFileName(page)
        ///     
        /// This permits a page to identify itself as a full
        /// tilde path.
        /// </summary>
        /// <param name="page">A web site Page object</param>
        public static string GetTildePathWithFileName(Page page)
        {
            return GetTildePath(page) + GetFileName(page);
        }


        /// <summary>
        /// Returns a string consisting of concatenations of
        /// strings of the form ../ that make the relative
        /// offset of the page back to the root page.
        /// 
        /// This method may be used to create relative URLs.
        /// </summary>
        /// <param name="page">A web site Page object</param>
        /// <returns>The relative offset string</returns>
        public static string GetRootOffset(Page page)
        {
            if (page == null)
                return "";

            string rootpath = GetRoot(page);
            string pagepath = GetDirectoryPath(page);

            string result = "";

            string filepath = GetPathFromRoot(rootpath, pagepath);

            int n = filepath.Length;

            for (int i = 0; i < n; i++)
                if (filepath[i] == backslash)
                    result += upOneStart;

            return result;
        }


        /// <summary>
        /// Returns either an absolute url or a url relative to
        /// the source directory tilde path.
        /// 
        /// Steps:
        /// 
        /// 1. If the url begins with "http://"  or "https://" then 
        /// it is returned.
        /// 
        /// 2. If the url begins with "~/" then it is resoved via a
        /// call to GetRelativePath.
        /// 
        /// 3. Otherwise, the url is assumed to be a path relative
        /// to the source and is returned.
        /// 
        /// There is no additional error checking.
        /// </summary>
        /// <param name="sourceTildePath">Source directory tilde path</param>
        /// <param name="url">Url to resolve</param>
        public static string GetResolvedUrl
            (string sourceTildePath, string url)
        {
            if (url == null)
                return "";

            if (url.StartsWith(httpStart))
                return url;

            if (url.StartsWith(httpsStart))
                return url;

            if (url.StartsWith(tildeStart))
                return GetRelativePath(sourceTildePath, url);

            return url;
        }


        /// <summary>
        /// Returns either an absolute url or a url relative to
        /// the source directory tilde path.
        /// 
        /// The target directory tilde path plays a role in the
        /// resolution since if the url is initially a relative
        /// url it is assumed to be relative to the target and
        /// not the source.  Hence, a url relative to the
        /// source must be computed.
        /// 
        /// Steps:
        /// 
        /// 1. If the url begins with "http://"  or "https://" then 
        /// it is returned.
        /// 
        /// 2. If the url begins with "~/" then it is resoved via a
        /// call to GetRelativePath using the source tilde path and
        /// ignoring the target tilde path.
        /// 
        /// 3. Otherwise, the url is assumed to be a path relative
        /// to the target.  The url is converted to a tilde path by
        /// simultaneously removing "../" sequences from the url and
        /// removing corresponding tail directories from the target
        /// tilde path.
        /// 
        /// 4. Once this is done, the rest of the target tilde path
        /// is concatenated with the rest of the url to form a new
        /// tilde path.  The url is resolved using GetRelativePath
        /// with the source path and the new tilde path.
        /// 
        /// There is no additional error checking.
        /// </summary>
        /// <param name="sourceTildePath">Source directory tilde path</param>
        /// <param name="url">Url to resolve</param>
        public static string GetResolvedUrl
            (string sourceTildePath, string targetTildePath, string url)
        {
            if (url == null)
                return "";

            if (url.StartsWith(httpStart))
                return url;

            if (url.StartsWith(httpsStart))
                return url;

            if (url.StartsWith(tildeStart))
                return GetRelativePath(sourceTildePath, url);

            targetTildePath = FileTools.GetTildeDirectory(targetTildePath);

            while (url.StartsWith(upOneStart))
            {
                bool ok = GoToTildeDirectoryAbove(ref targetTildePath);

                if (!ok)
                    return "";

                url = url.Substring(3);
            }

            string target = targetTildePath + url;

            return GetRelativePath(sourceTildePath, target);
        }


        /// <summary>
        /// Returns the relative URL from the source tilde path location
        /// to the target tilde path resource.
        /// 
        /// If either path is null or does not start with "~/" then
        /// returns an empty string to indicate an error.
        /// 
        /// Finds the longest directory subpath in the source that is a
        /// match for part of the target.  Then manufactures a relative
        /// URL using a StringBuilder as follows.
        /// 
        /// Appends the prefix "../" for each slash in the source that
        /// occurs after the match position.
        /// 
        /// Then appends the portion of the target resource that follows
        /// the match.
        /// 
        /// Note that any information in the source after the last slash
        /// is ignored.
        /// 
        /// In contrast all information in the target is used.
        /// </summary>
        /// <param name="sourceTildePath">tilde path of source location</param>
        /// <param name="targetTildePath">tilde path of target resource</param>
        public static string GetRelativePath(string sourceTildePath, string targetTildePath)
        {
            if (sourceTildePath == null)
                return "";

            if (targetTildePath == null)
                return "";

            if (!sourceTildePath.StartsWith(tildeStart))
                return "";

            if (!targetTildePath.StartsWith(tildeStart))
                return "";

            // find first position of a mismatch
            int position = 2;
            int slength = sourceTildePath.Length;
            int tlength = targetTildePath.Length;

            while ((position < slength) && (position < tlength)
                && (sourceTildePath[position] == targetTildePath[position]))
            {
                position++;
            }

            // find directory path in source that matches target
            string match = sourceTildePath.Substring(0, position);
            int last = match.LastIndexOf(slash);

            // make relative path
            StringBuilder builder = new StringBuilder();

            int start = last + 1;

            for (int i = start; i < slength; i++)
            {
                if (sourceTildePath[i] == slash)
                    builder.Append(upOneStart);
            }

            builder.Append(targetTildePath.Substring(start));

            return builder.ToString();
        }


        /// <summary>
        /// Shorthand for:
        ///     GetRelativePath(GetTildePath(page), targetTildePath)
        /// 
        /// If page is null, then GetTildePath(page) returns "~/".
        /// In this case, this method returns the result of removing
        /// "~/" from targetTildePath.
        /// </summary>
        /// <param name="page">the source page</param>
        /// <param name="targetTildePath">tilde path of target resource</param>
        public static string GetRelativePath(Page page, string targetTildePath)
        {
            return GetRelativePath(GetTildePath(page), targetTildePath);
        }


        /// <summary>
        /// Returns the tilde directory component of the tilde path.
        /// 
        /// Returns "~/" if the tilde path is null or does not start
        /// with "~/".
        /// 
        /// The string returned will always contain "~/" and will
        /// end in a slash.
        /// </summary>
        /// <param name="tildePath">A tilde path to a resource</param>
        /// <returns></returns>
        public static string GetTildeDirectory(string tildePath)
        {
            if (tildePath == null)
                return tildeStart;

            if (!tildePath.StartsWith(tildeStart))
                return tildeStart;

            int last = tildePath.LastIndexOf(slash);

            if (last == (tildePath.Length - 1))
                return tildePath;
            else
                return tildePath.Substring(0, last + 1);
        }


        /// <summary>
        /// The given tilde path is converted to a tilde directory
        /// path using GetTildeDirectory.
        /// 
        /// If that tilde directory path turns out to be "~/" then
        /// this method returns false;
        /// 
        /// Otherwise, this method strips the trailing directory
        /// from the current tilde directory path and returns true.
        /// </summary>
        /// <param name="tildePath">A tilde path to a resource</param>
        /// <returns></returns>
        public static bool GoToTildeDirectoryAbove(ref string tildePath)
        {
            tildePath = GetTildeDirectory(tildePath);

            if (tildePath == tildeStart)
                return false;

            tildePath = tildePath.Substring(0, tildePath.Length - 1);

            int last = tildePath.LastIndexOf(slash);

            tildePath = tildePath.Substring(0, last + 1);

            return true;
        }


        /// <summary>
        /// Returns the resource component of a url path, that is,
        /// the content after the final slash.
        /// 
        /// Returns "" if the url path is null.
        /// 
        /// Returns the entire url path if it does not contain a slash.
        /// 
        /// Assumes there are no path decorations such as query strings.
        /// </summary>
        /// <param name="urlPath">A url path to a resource</param>
        /// <returns></returns>
        public static string GetResource(string urlPath)
        {
            if (urlPath == null)
                return "";

            int last = urlPath.LastIndexOf(slash);

            if (last < 0)
                return urlPath;
            else
                return urlPath.Substring(last + 1);
        }


        /// <summary>
        /// Returns a string created from the given source string
        /// by replacing backslash with slash.
        /// </summary>
        /// <param name="source">The source string</param>
        /// <returns>The replacement string</returns>
        public static string BackSlashToSlash(string source)
        {
            if (source == null)
                return null;

            StringBuilder builder = new StringBuilder(source);
            builder.Replace(backslash, slash);
            return builder.ToString();
        }


        /// <summary>
        /// Returns the contents of the file
        /// with the given file path
        /// as a text string.
        /// 
        /// Returns an empty string on errors.
        /// </summary>
        /// <param name="filepath">The file path</param>
        /// <returns>The file contents as text</returns>
        public static string GetFileAsText(string filepath)
        {
            if (filepath == null)
                return "";

            try
            {
                using (StreamReader reader = new StreamReader(filepath))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }


        /// <summary>
        /// Returns a list of trimmed non-empty lines in a file.
        /// 
        /// The file name may be given by a web relative file path
        /// to the given page or by a tilde file path.
        /// 
        /// This method is a generalization of a method originally
        /// in the Teaching Preferences web application.
        /// 
        /// If an exception occurs, a list with 0 items will be
        /// returned.
        /// </summary>
        /// <param name="page">The page calling this method</param>
        /// <param name="filename">The web page relative file name</param>
        /// <returns>The list of the non-empty lines</returns>
        public static List<string> FileData(Page page, string filename)
        {
            return FileDataAbsolutePath(page.MapPath(filename), true);
        }


        /// <summary>
        /// Returns a list of lines in a file.
        /// 
        /// If trim is true then all lines are trimmed and any lines
        /// that then have length 0 are discarded.
        /// 
        /// If trim is false then all lines are added unchanged.
        /// 
        /// The file name may be given by a web file relative path
        /// to the given page or by a tilde file path.
        /// 
        /// This method is a generalization of a method originally
        /// in the Teaching Preferences web application.
        /// 
        /// If an exception occurs, a list with 0 items will be
        /// returned.
        /// </summary>
        /// <param name="page">The page calling this method</param>
        /// <param name="filename">The web page relative file name</param>
        /// <returns>The list of the non-empty lines</returns>
        public static List<string> FileData(Page page, string filename, bool trim)
        {
            return FileDataAbsolutePath(page.MapPath(filename), trim);
        }


        /// <summary>
        /// Returns a list of trimmed non-empty lines in a file.
        /// 
        /// The file path is an absolute path.
        /// 
        /// This method is a generalization of a method originally
        /// in the Teaching Preferences web application.
        /// 
        /// If an exception occurs, a list with 0 items will be
        /// returned.
        /// </summary>
        /// <param name="filepath">The absolute file path</param>
        /// <returns>The list of the non-empty lines</returns>
        public static List<string> FileDataAbsolutePath(string filepath)
        {
            return FileDataAbsolutePath(filepath, true);
        }


        /// <summary>
        /// Returns a list of lines in a file.
        /// 
        /// If trim is true then all lines are trimmed and any lines
        /// that then have length 0 are discarded.
        /// 
        /// If trim is false then all lines are added unchanged.
        /// 
        /// The file path is an absolute path.
        /// 
        /// This method is a generalization of a method originally
        /// in the Teaching Preferences web application.
        /// 
        /// If an exception occurs, a list with 0 items will be
        /// returned.
        /// 
        /// In August 2009, refactored to use StringTools.LineList.
        /// </summary>
        /// <param name="filepath">The absolute file path</param>
        /// <returns>The list of the non-empty lines</returns>
        public static List<string> FileDataAbsolutePath(string filepath, bool trim)
        {
            string content = GetFileAsText(filepath);

            return StringTools.LineList(content, trim);
        }

    }
}