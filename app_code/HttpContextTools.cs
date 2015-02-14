/*
    File: ~/app_code/HttpContextTools.cs
    
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
using System.Text;

namespace edu.neu.ccis.rasala
{

    public class HttpContextTools
    {
        /// <summary>
        /// Constant mask that signals a servable file
        /// </summary>
        public const int SERVABLE = 8;


        /// <summary>
        /// Servable extensions
        /// 
        /// Currently: .aspx, .htm, .html
        /// </summary>
        static string[] servable_extensions =
        {
            ".aspx", ".htm", ".html"
        };


        /// <summary>
        /// Hash table with servable extensions
        /// </summary>
        static Dictionary<string, int> servable_extensions_hash =
            new Dictionary<string, int>();


        /// <summary>
        /// Initializes hash tables
        /// </summary>
        static HttpContextTools()
        {
            foreach (string ext in servable_extensions)
                servable_extensions_hash.Add(ext, SERVABLE);
        }


        /// <summary>
        /// Assuming that the given webpath is a string with
        /// names separated by slash characters and possibly
        /// a leading tilde, returns the substring up to and
        /// including the final slash character.
        /// 
        /// If webpath is null returns the empty string.
        /// 
        /// If webpath does not contain slash returns webpath.
        /// </summary>
        public static string GetDirectoryPath(string webpath)
        {
            if (StringTools.IsTrivial(webpath))
                return "";

            int spot = webpath.LastIndexOf(FileTools.slash);

            return (spot >= 0)
                ? webpath.Substring(0, spot + 1)
                : webpath;
        }


        /// <summary>
        /// Assuming that the given webpath is a string with
        /// names separated by slash characters and possibly
        /// a leading tilde, returns the substring after the
        /// final slash character.
        /// 
        /// This method assumes that the filename is the last
        /// portion of the string after the final slash.
        /// 
        /// Hence, extra stuff such as a tail, a query string,
        /// or a fragment identifier must already be stripped.
        /// 
        /// If webpath is null returns the empty string.
        /// 
        /// If webpath does not contain slash returns webpath.
        /// </summary>
        public static string GetFileName(string webpath)
        {
            if (StringTools.IsTrivial(webpath))
                return "";

            int spot = webpath.LastIndexOf(FileTools.slash);

            return (spot >= 0)
                ? webpath.Substring(spot + 1)
                : webpath;
        }


        /// <summary>
        /// Returns context.Request.FilePath
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string GetWebPath(HttpContext context)
        {
            if (context == null)
                return "";

            return context.Request.FilePath;
        }


        /// <summary>
        /// Uses context.Request.FilePath
        /// 
        /// Returns the web path directory by removing
        /// the trailing file path.
        /// 
        /// Includes a trailing slash in the directory.
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string GetWebDirectoryPath(HttpContext context)
        {
            if (context == null)
                return "";

            return GetDirectoryPath(context.Request.FilePath);
        }


        /// <summary>
        /// Uses context.Request.FilePath
        /// 
        /// Returns the trailing file name in the web path.
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string GetWebFileName(HttpContext context)
        {
            if (context == null)
                return "";

            return GetFileName(context.Request.FilePath);
        }


        /// <summary>
        /// Returns context.Request.ApplicationPath
        /// 
        /// Discussion:
        /// 
        /// Consider a URL of the form
        /// 
        ///   scheme://domain.../path-down-to-application.../application-specifics
        /// 
        /// Then the application path is the substring:
        /// 
        ///   /path-down-to-application...
        /// 
        /// Usually, this string will not end with a slash.
        /// 
        /// However, this string may consist of a single slash character
        /// in case there is no path-down-to-application.
        /// 
        /// Therefore, one cannot depend on the absence of the slash
        /// at the end of the string.
        /// 
        /// See: GetApplicationPathWithSlash
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string GetApplicationPath(HttpContext context)
        {
            if (context == null)
                return "";

            return context.Request.ApplicationPath;
        }


        /// <summary>
        /// Uses context.Request.ApplicationPath
        /// 
        /// Returns context.Request.ApplicationPath with a trailing
        /// slash appended if the path does not end in a slash.
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string GetApplicationPathWithSlash(HttpContext context)
        {
            if (context == null)
                return "";

            string s = context.Request.ApplicationPath;

            if (StringTools.IsTrivial(s))
                return "";

            int n = s.Length - 1;

            char slash = FileTools.slash;

            if (s[n] == slash)
                return s;
            else
                return s + slash;
        }


        /// <summary>
        /// Returns the tilde path corresponding to the
        /// given context.
        /// 
        /// How this works:
        /// 
        /// 1. Obtains filePath = context.Request.FilePath
        /// 2. Obtains appPath = GetApplicationPathWithSlash
        /// 3. Removes appPath from filePath
        /// 4. Adds ~/ to the front of what is left
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string GetTildePath(HttpContext context)
        {
            if (context == null)
                return "";

            string filePath = context.Request.FilePath;
            string appPath = GetApplicationPathWithSlash(context);

            int n = appPath.Length;

            return FileTools.tildeStart + filePath.Substring(n);
        }


        /// <summary>
        /// Returns the tilde directory path corresponding to the
        /// given context.
        /// 
        /// How this works:
        /// 
        /// 1. Uses GetTildePath
        /// 2. Removes the file name from the rear via GetDirectoryPath.
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string GetTildeDirectoryPath(HttpContext context)
        {
            if (context == null)
                return "";

            return GetDirectoryPath(GetTildePath(context));
        }


        /// <summary>
        /// Merges the application path with the tilde path.
        /// 
        /// How this works:
        ///   1. Remove ~/ from front of tilde path
        ///   2. Prepend GetApplicationPathWithSlash
        /// 
        /// This merged path is a valid URL on the site for
        /// serving site content.
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string MakeMergedPath
            (HttpContext context, string tildePath)
        {
            if (context == null)
                return "";

            if (StringTools.IsTrivial(tildePath))
                return "";

            if (!tildePath.StartsWith(SourceTools.tildeStart))
                return "";

            // remove ~/
            string shortPath = tildePath.Substring(2);

            // merge
            return GetApplicationPathWithSlash(context) + shortPath;
        }


        /// <summary>
        /// Return the start of the url for the request
        /// associated with the given context.
        /// 
        /// In Microsoft terms, subtracts Url - RawUrl,
        /// viewed as strings.  This yields the leading
        /// portion of the url.
        /// 
        /// In practice, the url start consists of:
        /// 
        ///   * The URL scheme such as "http"
        ///   * The standard separator "://"
        ///   * The domain information including port
        ///     and username:password if supplied.
        /// 
        /// If an error occurs returns the empty string.
        /// 
        /// Technical Note:
        /// 
        ///   The RawUrl is a string property.
        ///   The Url property is of type Uri.
        ///   
        /// We actually subtract the string RawUrl from
        /// the string Url.ToString() to obtain the
        /// start of the url.
        /// </summary>
        public static string GetUrlStart
            (HttpContext context)
        {
            if (context == null)
                return "";

            string rawUrl = context.Request.RawUrl.ToString();

            string url = context.Request.Url.ToString();

            int m = rawUrl.Length;

            int n = url.Length;

            return url.Substring(0, n - m);
        }


        /// <summary>
        /// Returns true if the filename extension indicates
        /// that the file is servable.
        /// </summary>
        /// <param name="filename">The file name to test</param>
        public static bool IsServable(string filename)
        {
            if (filename == null)
                return false;

            int index = filename.LastIndexOf('.');

            if (index == -1)
                return false;

            string ext = filename.Substring(index).ToLower();

            int category;

            if (servable_extensions_hash.TryGetValue(ext, out category))
                if (category == SERVABLE)
                    return true;

            return false;
        }


        /// <summary>
        /// Returns the list of servable files
        /// in the same directory
        /// as the context web directory.
        /// </summary>
        /// <param name="context">Web site HttpContext object</param>
        public static List<string> MakeServableList(HttpContext context)
        {
            if (context == null)
                return new List<string>();

            string directorypath = FileTools.GetDirectoryPath(context);

            List<string> filelist =
                SourceTools.MakeFileList(directorypath, FileTools.ALL);

            List<string> servablelist = new List<string>();

            foreach (string name in filelist)
            {
                if (IsServable(name))
                    servablelist.Add(name);
            }

            return servablelist;
        }


        /// <summary>
        /// Returns the list of files
        /// that reside in the context web directory
        /// and that match the given file type.
        /// 
        /// The filetype should take one of seven values:
        ///   1: Text files
        ///   2: Image files
        ///   3: Viewable files, that is, text and image files
        ///   4: Non-viewable files
        ///   5: Text files plus non-viewable files
        ///   6: Image files plus non-viewable files
        ///   7: All files
        /// </summary>
        /// <param name="context">Web site HttpContext object</param>
        /// <param name="fileType">The filetype mask</param>
        public static List<string> MakeFileList
            (HttpContext context, int fileType)
        {
            if (context == null)
                return new List<string>();

            string directorypath = FileTools.GetDirectoryPath(context);

            return SourceTools.MakeFileList(directorypath, fileType);
        }


        /// <summary>
        /// Returns the list of subdirectories
        /// in the context web directory.
        /// </summary>
        /// <param name="context">Web site HttpContext object</param>
        /// <param name="onlyPublic">If true restrict to public subdirectories</param>
        public static List<string> MakeSubdirectoryList
            (HttpContext context, bool onlyPublic)
        {
            List<string> list = new List<string>();

            if (context == null)
                return list;

            string path = FileTools.GetDirectoryPath(context);

            DirectoryInfo thisdir = new DirectoryInfo(path);

            DirectoryInfo[] subdirs = thisdir.GetDirectories();

            foreach (DirectoryInfo di in subdirs)
            {
                string name = di.Name;

                if (onlyPublic)
                {
                    if (!SourceTools.IsPublic(name))
                        continue;
                }

                list.Add(name);
            }

            return list;
        }


        /// <summary>
        /// Returns the style sheet link code to reference
        /// the given css path with the given media setting.
        /// 
        /// If the path is a tilde path, use the context to
        /// create the correct url via MakeMergedPath.
        /// 
        /// Otherwise, assume the path is a correct url as is.
        /// Further, in this case, one may pass null for the
        /// context parameter.
        /// 
        /// If the media string is null or empty, it is ignored.
        /// 
        /// If an error occurs returns the empty string.
        /// </summary>
        public static string MakeStyleSheetLink
            (HttpContext context, string path, string media)
        {
            if (StringTools.IsTrivial(path))
                return "";

            if (path.StartsWith(SourceTools.tildeStart))
            {
                if (context == null)
                    return "";

                path = MakeMergedPath(context, path);
            }

            StringBuilder builder = new StringBuilder();

            builder.Append(HTML_Tools.open_css_link);
            builder.Append(path);

            if (!StringTools.IsTrivial(media))
            {
                builder.Append(HTML_Tools.media_css);
                builder.Append(media);
            }

            builder.Append(HTML_Tools.shut_css_link);

            return builder.ToString();
        }


        /// <summary>
        /// Returns references to the CSS used in SourceTools.
        /// 
        /// The context parameter must be non-null.
        /// </summary>
        public static string MakeStyleSheetLinks
            (HttpContext context)
        {
            return MakeStyleSheetLink(context, SourceTools.sourceCSS,  null)
                + MakeStyleSheetLink(context, SourceTools.printCSS, "print");
        }


        /// Returns the script code to reference the Javascript
        /// file at the given path.
        /// 
        /// If the path is a tilde path, use the context to
        /// create the correct url via MakeMergedPath.
        /// 
        /// Otherwise, assume the path is a correct url as is.
        /// Further, in this case, one may pass null for the
        /// context parameter.
        /// 
        /// If an error occurs returns the empty string.
        public static string MakeJavascriptReference
            (HttpContext context, string path)
        {
            if (StringTools.IsTrivial(path))
                return "";

            if (path.StartsWith(SourceTools.tildeStart))
            {
                if (context == null)
                    return "";

                path = MakeMergedPath(context, path);
            }

            StringBuilder builder = new StringBuilder();

            builder.Append(HTML_Tools.script_file_1);
            builder.Append(path);
            builder.Append(HTML_Tools.script_file_2);

            return builder.ToString();
        }


        /// <summary>
        /// Returns references to the Javascript used in SourceTools.
        /// 
        /// The context parameter must be non-null.
        /// </summary>
        public static string MakeJavascriptReferences
            (HttpContext context)
        {
            return MakeJavascriptReference(context, Javascript.jQuery)
                 + MakeJavascriptReference(context, SourceTools.modernizrJS)
                 + MakeJavascriptReference(context, SourceTools.sourceJS);
        }
    }
}