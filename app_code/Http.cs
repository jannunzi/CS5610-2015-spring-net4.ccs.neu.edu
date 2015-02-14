using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace edu.neu.ccis.rasala
{

    public class Http
    {

        /// <summary>
        /// Returns a string array with the two standard HTTP
        /// schemes: "http" and "https".
        /// </summary>
        public static string[] GetHttpSchemes()
        {
            return new string[] { "http", "https" };
        }


        /// <summary>
        /// Returns a string array with all Http headers both
        /// for request and response.
        /// 
        /// Valid on 2011-04-12.
        /// </summary>
        public static string[] GetHttpHeaders()
        {
            return new string[]
                {
                    "Accept",
                    "Accept-Charset",
                    "Accept-Encoding",
                    "Accept-Language",
                    "Accept-Ranges",
                    "Age",
                    "Allow",
                    "Authorization",
                    "Cache-Control",
                    "Connection",
                    "Content-Base",
                    "Content-Disposition",
                    "Content-Encoding",
                    "Content-Language",
                    "Content-Length",
                    "Content-Location",
                    "Content-MD5",
                    "Content-Range",
                    "Content-Type",
                    "Cookie",
                    "Date",
                    "ETag",
                    "Expect",
                    "Expires",
                    "From",
                    "Host",
                    "If-Match",
                    "If-Modified-Since",
                    "If-None-Match",
                    "If-Range",
                    "If-Unmodified-Since",
                    "Last-Modified",
                    "Link",
                    "Location",
                    "Max-Forwards",
                    "P3P",
                    "Pragma",
                    "Proxy-Authenticate",
                    "Proxy-Authorization",
                    "Range",
                    "Referer",
                    "Refresh",
                    "Retry-After",
                    "Server",
                    "Set-Cookie",
                    "TE",
                    "Trailer",
                    "Transfer-Encoding",
                    "Upgrade",
                    "User-Agent",
                    "Vary",
                    "Via",
                    "Warn",
                    "Warning",
                    "WWW-Authenticate",

                };
        }


        /*
         * Return the common application/ media types that are text.
         * We ignore some obscure types.
         * 
         * Last updated: 2014-03-19
         */
        public static string[] GetApplicationMediaTypesThatAreText()
        {
            return new string[]
                {
                    "application/atom+xml",
                    "application/ecmascript",
                    "application/json",
                    "application/javascript",
                    "application/rdf+xml",
                    "application/rss+xml",
                    "application/soap+xml",
                    "application/xhtml+xml",
                    "application/xml",
                    "application/xml-dtd",
                    "application/xop+xml"
                };
        }

        /// <summary>
        /// Returns the response category as one of three values
        ///   FileTools.TEXT
        ///   FileTools.IMAGE
        ///   FileTools.OTHER
        /// 
        /// Tests the response object.
        /// 
        /// If response.ContentType determines that the value is
        /// TEXT or IMAGE then that value is returned.
        /// 
        /// Otherwise, OTHER is returned.
        /// 
        /// 
        /// Assumption: The response is the result of requesting
        /// some uri.
        /// </summary>
        /// <param name="response">The response to the request</param>
        public static int ResponseCategory(HttpWebResponse response)
        {
            int category = FileTools.OTHER;

            if (response == null)
                return category;

            string contentType = response.ContentType;

            if (!StringTools.IsTrivial(contentType))
            {
                if (contentType.StartsWith("text"))
                    category = FileTools.TEXT;
                else if (contentType.StartsWith("image"))
                    category = FileTools.IMAGE;
                else if (contentType.Contains("htm"))
                    category = FileTools.TEXT;
                else if (contentType.Contains("xml"))
                    category = FileTools.TEXT;
                else if (contentType.Contains("javascript"))
                    category = FileTools.TEXT;
                else if (contentType.Contains("json"))
                    category = FileTools.TEXT;
                else if (contentType.StartsWith("application"))
                {
                    string[] types =
                        GetApplicationMediaTypesThatAreText();

                    foreach (string t in types)
                    {
                        if (contentType.StartsWith(t))
                        {
                            category = FileTools.TEXT;
                            break;
                        }
                    }
                }
            }

            return category;
        }

        /// <summary>
        /// This method is obsolete because its uri parameter is
        /// no longer unused.
        /// 
        /// This method is retained for backward compatibility.
        /// 
        /// Returns the response category as one of three values
        ///   FileTools.TEXT
        ///   FileTools.IMAGE
        ///   FileTools.OTHER
        /// 
        /// Tests the response object.
        /// 
        /// If response.ContentType determines that the value is
        /// TEXT or IMAGE then that value is returned.
        /// 
        /// Otherwise, OTHER is returned.
        /// </summary>
        /// <param name="uri">The uri to request</param>
        /// <param name="response">The response to the request</param>
        public static int ResponseCategory
            (UriPlus uri, HttpWebResponse response)
        {
            return ResponseCategory(response);
        }

        /// <summary>
        /// Returns true if response.ContentType contains "htm".
        /// 
        /// This should pick out just the media types:
        ///   text/html
        ///   application/xhtml+xml
        /// </summary>
        /// <param name="response">The response to a request</param>
        public static bool IsHtml(HttpWebResponse response)
        {
            if (response != null)
            {
                string contentType = response.ContentType;

                if (!StringTools.IsTrivial(contentType))
                {
                    if (contentType.Contains("htm"))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if response.ContentType contains "xml".
        /// </summary>
        /// <param name="response">The response to a request</param>
        public static bool IsXml(HttpWebResponse response)
        {
            if (response != null)
            {
                string contentType = response.ContentType;

                if (!StringTools.IsTrivial(contentType))
                {
                    if (contentType.Contains("xml"))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if response.ContentType contains "json".
        /// </summary>
        /// <param name="response">The response to a request</param>
        public static bool IsJSON(HttpWebResponse response)
        {
            if (response != null)
            {
                string contentType = response.ContentType;

                if (!StringTools.IsTrivial(contentType))
                {
                    if (contentType.Contains("json"))
                        return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Returns true if response.ContentType contains "javascript".
        /// </summary>
        /// <param name="response">The response to a request</param>
        public static bool IsJavascript(HttpWebResponse response)
        {
            if (response != null)
            {
                string contentType = response.ContentType;

                if (!StringTools.IsTrivial(contentType))
                {
                    if (contentType.Contains("javascript"))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if response.ContentType contains "ecmascript".
        /// </summary>
        /// <param name="response">The response to a request</param>
        public static bool IsEcmascript(HttpWebResponse response)
        {
            if (response != null)
            {
                string contentType = response.ContentType;

                if (!StringTools.IsTrivial(contentType))
                {
                    if (contentType.Contains("ecmascript"))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the uncompressed content from a response that
        /// is supposed to return text based on its content type.
        /// 
        /// If the incoming data is already uncompressed then the
        /// content is returned as is.
        /// 
        /// Returns the uncompressed content that arrived in the
        /// form of data compressed by gzip or deflate.
        /// 
        /// Returns an empty string if the content type is not
        /// recognized as text.
        /// 
        /// Returns an error message if an exception is thrown.
        /// </summary>
        /// <param name="response">The response to a request</param>
        public static string GetUncompressedTextContent(HttpWebResponse response)
        {
            string content = "";

            if (ResponseCategory(response) != FileTools.TEXT)
                return content;

            string encoding = response.ContentEncoding ?? "";
            
            try
            {
                if (encoding == "")
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            content = reader.ReadToEnd();
                        }
                    }
                }
             }
             catch (Exception ex)
             {
                content = "Error in obtaining uncompressed data "
                    + ex.Message;
             }

             try
             {
                if (encoding == "gzip")
                {
                    var response_gzip = response.GetResponseStream();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (GZipStream uncompress =
                            new GZipStream
                                (response_gzip, CompressionMode.Decompress))
                        {
                            uncompress.CopyTo(stream);
                            content = Encoding.UTF8.GetString(stream.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                content = "Error in obtaining gzip compressed data "
                    + ex.Message;
            }

            try
            {
                if (encoding == "deflate")
                {
                    var response_deflate = response.GetResponseStream();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (DeflateStream uncompress =
                            new DeflateStream
                                (response_deflate, CompressionMode.Decompress))
                        {
                            uncompress.CopyTo(stream);
                            content = Encoding.UTF8.GetString(stream.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                content = "Error in obtaining deflate compressed data "
                    + ex.Message;
            }

            return content;
        }

    }
}
