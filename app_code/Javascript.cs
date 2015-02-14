using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Text;

namespace edu.neu.ccis.rasala
{
    public class Javascript
    {
        /// <summary>
        /// Open for Javascript file reference
        /// </summary>
        public const string script_file_1 =
            HTML_Tools.script_file_1;

        /// <summary>
        /// Shut for Javascript file reference
        /// </summary>
        public const string script_file_2 =
            HTML_Tools.script_file_2;

        /// <summary>
        /// Open for Javascript inline text insert
        /// </summary>
        public const string script_inline_1 =
            HTML_Tools.script_inline_1;

        /// <summary>
        /// Shut for Javascript inline text insert
        /// </summary>
        public const string script_inline_2 =
            HTML_Tools.script_inline_2;


        public static string jQuery = ConfigurationManager.AppSettings["jQuery"];


        /// <summary>
        /// Loads a reference to jQuery from googlecode.
        /// </summary>
        /// <param name="page">The page on which to load jQuery</param>
        public static void LoadJQueryReference(Page page)
        {
            LoadFileReference(page, jQuery);
        }


        /// <summary>
        /// Loads a Javascript file reference onto the page
        /// to the file at the given url.
        /// 
        /// The url may be:
        /// 
        ///   1. An absolute url "http://" or "https://"
        ///   2. A tilde url "~/"
        ///   3. A url relative to the given page
        ///   
        /// </summary>
        /// <param name="page">The page on which to load</param>
        /// <param name="url">The url of the file</param>
        public static void LoadFileReference(Page page, string url)
        {
            ClientScriptManager clientscriptmanager = page.ClientScript;
            Type type = page.GetType();

            if (!clientscriptmanager.IsClientScriptBlockRegistered(type, url))
            {
                string pageTildePath =
                    FileTools.GetTildePath(page);

                string resolvedUrl =
                    FileTools.GetResolvedUrl(pageTildePath, url);

                StringBuilder builder = new StringBuilder();

                builder.Append(script_file_1);
                builder.Append(resolvedUrl);
                builder.Append(script_file_2);

                string contents = builder.ToString();

                clientscriptmanager.RegisterClientScriptBlock
                    (type, url, contents, false);
            }
        }


        /// <summary>
        /// Loads the Javascript file contents onto the page
        /// using the given url which must refer to this site.
        /// 
        /// If the url starts with "http://" or "https://"
        /// then this method calls LoadFileReference instead.
        /// </summary>
        /// <param name="page">The page on which to load</param>
        /// <param name="url">The url of the file</param>
        /// <param name="addScriptTag">If true then add script tags</param>
        public static void LoadFileContents
            (Page page, string url, bool addScriptTag)
        {
            if (url.StartsWith(FileTools.httpStart))
            {
                LoadFileReference(page, url);
                return;
            }

            if (url.StartsWith(FileTools.httpsStart))
            {
                LoadFileReference(page, url);
                return;
            }

            ClientScriptManager clientscriptmanager = page.ClientScript;
            Type type = page.GetType();

            if (!clientscriptmanager.IsClientScriptBlockRegistered(type, url))
            {
                string filepath = page.MapPath(url);
                string contents = FileTools.GetFileAsText(filepath);
                LoadScriptString(page, url, contents, addScriptTag);
            }
        }


        /// <summary>
        /// Loads the given script onto this page with the given key
        /// to prevent multiple loads by different method calls.
        /// </summary>
        /// <param name="page">The page on which to load</param>
        /// <param name="key">The key to prevent multiple loads</param>
        /// <param name="script">The script to load</param>
        /// <param name="addScriptTag">If true then add script tags</param>
        public static void LoadScriptString
            (Page page, string key, string script, bool addScriptTag)
        {
            ClientScriptManager clientscriptmanager = page.ClientScript;
            Type type = page.GetType();

            if (!clientscriptmanager.IsClientScriptBlockRegistered(type, key))
            {
                string contents = "";

                if (addScriptTag)
                {
                    StringBuilder builder = new StringBuilder();

                    builder.Append(script_inline_1);
                    builder.Append(script);
                    builder.Append("\n");
                    builder.Append(script_inline_2);

                    contents = builder.ToString();
                }
                else
                {
                    contents = script;
                }

                clientscriptmanager.RegisterClientScriptBlock
                    (type, key, contents, false);
            }
        }

    }
}
