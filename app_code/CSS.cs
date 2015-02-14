using System;
using System.Web;
using System.Web.UI;
using System.Text;

namespace edu.neu.ccis.rasala
{

    public class CSS
    {
        /// <summary>
        /// Open css link tag
        /// </summary>
        public const string open_css_link =
            HTML_Tools.open_css_link;

        /// <summary>
        /// Shut css link tag
        /// </summary>
        public const string shut_css_link =
            HTML_Tools.shut_css_link;

        /// <summary>
        /// Open inline css tag
        /// </summary>
        public const string open_css_inline =
            HTML_Tools.open_css_inline;

        /// <summary>
        /// Open inline css tag before media attribute
        /// </summary>
        public const string open_css_inline_1 =
            HTML_Tools.open_css_inline_1;

        /// <summary>
        /// Open inline css tag after media attribute
        /// </summary>
        public const string open_css_inline_2 =
            HTML_Tools.open_css_inline_2;

        /// <summary>
        /// Shut inline css tag
        /// </summary>
        public const string shut_css_inline =
            HTML_Tools.shut_css_inline;

        /// <summary>
        /// Insert for css media attribute
        /// </summary>
        public const string media_css =
            HTML_Tools.media_css;


        /// <summary>
        /// Loads a CSS file reference onto the page
        /// to the file at the given url
        /// with the given CSS media attribute.
        /// 
        /// The url may be:
        /// 
        ///   1. An absolute url "http://" or "https://"
        ///   2. A tilde url "~/"
        ///   3. A url relative to the given page
        /// 
        /// The media parameter may be used to give
        /// the CCS media attribute.  The parameter
        /// is ignored if it is null or empty.
        /// </summary>
        /// <param name="page">The page on which to load</param>
        /// <param name="url">The url of the file</param>
        /// <param name="media">The CSS media attribute if any</param>
        public static void LoadFileReference(Page page, string url, string media)
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

                builder.Append(open_css_link);
                builder.Append(resolvedUrl);

                if ((media != null) && (media.Length > 0))
                {
                    builder.Append(media_css);
                    builder.Append(media);
                }

                builder.Append(shut_css_link);

                string contents = builder.ToString();

                clientscriptmanager.RegisterClientScriptBlock
                    (type, url, contents, false);
            }
        }


        /// <summary>
        /// Loads the CSS file contents onto the page
        /// using the given url which must refer to this site.
        /// 
        /// If the url starts with "http://" or "https://"
        /// then this method calls LoadFileReference instead.
        /// 
        /// The media parameter may be used to give
        /// the CCS media attribute.  The parameter
        /// is ignored if it is null or empty or if
        /// addStyleTag is false.
        /// </summary>
        /// <param name="page">The page on which to load</param>
        /// <param name="url">The url of the file</param>
        /// <param name="media">The CSS media attribute if any</param>
        /// <param name="addStyleTag">If true then add script tags</param>
        public static void LoadFileContents
            (Page page, string url, string media, bool addStyleTag)
        {
            if (url.StartsWith(FileTools.httpStart))
            {
                LoadFileReference(page, url, media);
                return;
            }

            if (url.StartsWith(FileTools.httpsStart))
            {
                LoadFileReference(page, url, media);
                return;
            }

            ClientScriptManager clientscriptmanager = page.ClientScript;
            Type type = page.GetType();

            if (!clientscriptmanager.IsClientScriptBlockRegistered(type, url))
            {
                string filepath = page.MapPath(url);
                string contents = FileTools.GetFileAsText(filepath);
                LoadScriptString(page, url, contents, media, addStyleTag);
            }
        }


        /// <summary>
        /// Loads the given css onto this page with the given key
        /// to prevent multiple loads by different method calls.
        /// 
        /// The media parameter may be used to give
        /// the CCS media attribute.  The parameter
        /// is ignored if it is null or empty or if
        /// addStyleTag is false.
        /// </summary>
        /// <param name="page">The page on which to load</param>
        /// <param name="key">The key to prevent multiple loads</param>
        /// <param name="css">The css text to load</param>
        /// <param name="media">The CSS media attribute if any</param>
        /// <param name="addStyleTag">If true then add script tags</param>
        public static void LoadScriptString
            (Page page, string key, string css, string media, bool addStyleTag)
        {
            ClientScriptManager clientscriptmanager = page.ClientScript;
            Type type = page.GetType();

            if (!clientscriptmanager.IsClientScriptBlockRegistered(type, key))
            {
                string contents = "";

                if (addStyleTag)
                {
                    StringBuilder builder = new StringBuilder();

                    builder.Append(open_css_inline_1);

                    if ((media != null) && (media.Length > 0))
                    {
                        builder.Append(media_css);
                        builder.Append(media);
                    }

                    builder.Append(open_css_inline_2);
                    builder.Append(css);
                    builder.Append(shut_css_inline);

                    contents = builder.ToString();
                }
                else
                {
                    contents = css;
                }

                clientscriptmanager.RegisterClientScriptBlock
                    (type, key, contents, false);
            }
        }

    }
}
