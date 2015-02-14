using System;
using System.IO;
using System.Web;
using System.Net;

namespace edu.neu.ccis.rasala
{
    public class SimpleProxy : IHttpHandler
    {

        /*
         * Returns the HttpWebResponse from executing a
         * WebRequest using the given url.
         * 
         * May return null if a serious error occurs.
         * 
         * May return a response with a status code that
         * is other than 200 for OK.
         * 
         * 
         * This static method may be used standalone if
         * you wish to process a web response in C# code
         * on the server.
         */
        public static HttpWebResponse GetResponse(string url)
        {
            HttpWebResponse response = null;

            if (StringTools.IsTrivial(url))
                return response;

            try
            {
                Uri uri = new Uri(url);

                HttpWebRequest request =
                    WebRequest.Create(uri) as HttpWebRequest;

                request.Date = DateTime.Now;

                response =
                    request.GetResponse() as HttpWebResponse;
            }
            catch
            {

            }

            return response;
        }


        /*
         * Returns the string content from executing a
         * WebRequest using the given url provide that
         * the content type is reconized as text by the
         * method Http.ResponseCategory.
         * 
         * If the string content is compressed, it will
         * be uncompressed before it is returned.
         * 
         * Will return null if
         *   the response is null
         *     or
         *   the response content type is not a type
         *   that normally contains text
         * 
         * Will return an empty string if
         *   the response content type is a text type
         *     and
         *   there happens to be no response content
         * 
         * 
         * This static method may be used standalone if
         * you wish to process a web response in C# code
         * on the server.
         */
        public static string GetResponseContent(string url)
        {
            HttpWebResponse response = GetResponse(url);

            if (response == null)
                return null;

            if (Http.ResponseCategory(response) != FileTools.TEXT)
                return null;

            return Http.GetUncompressedTextContent(response);
        }


        /*
         * Returns the string content from executing a
         * WebRequest using the given url provide that
         * the content type is reconized as text by the
         * method Http.ResponseCategory.
         * 
         * If the string content is compressed, it will
         * be uncompressed before it is returned.
         * 
         * The status code is returned as an out parameter.
         * The content type is returned as an out parameter.
         * 
         * 
         * Will return null if
         *   the response is null
         *   in this case the content type is set to ""
         *   
         * Will return null if
         *   the response content type is not a type
         *   that normally contains text
         * 
         * Will return an empty string if
         *   the response content type is a text type
         *     and
         *   there happens to be no response content
         * 
         * 
         * This static method may be used standalone if
         * you wish to process a web response in C# code
         * on the server.
         */
        public static string GetResponseContent
            (string url,
             out HttpStatusCode statusCode,
             out string contentType)
        {
            HttpWebResponse response = GetResponse(url);

            statusCode = HttpStatusCode.NotFound;
            contentType = "";

            if (response == null)
                return null;

            statusCode = response.StatusCode;
            contentType = response.ContentType;

            if (Http.ResponseCategory(response) != FileTools.TEXT)
                return null;

            return Http.GetUncompressedTextContent(response);
        }


        /*
         * Returns an embedded parameter with introduction string
         * param if one exists within the query string of the
         * given url.
         * 
         * The conventions of
         * 
         *     StringTools.FindParameter
         * 
         * should be followed.
         */
        public static string GetEmbeddedParameter
            (string url, string param, bool unescape)
        {
            if (StringTools.IsTrivial(url) || StringTools.IsTrivial(param))
                return "";
            
            int qmark = url.IndexOf('?');

            if (qmark < 0)
                return "";

            string query = url.Substring(qmark + 1);

            if (query.Length == 0)
                return "";

            return StringTools.FindParameter(query, param, unescape);
        }


        /*
         * The main method of this IHttpHandler.
         */
        public void ProcessRequest(HttpContext context)
        {
            string url = context.Request.Url.OriginalString;

            string embeddedUrl = GetEmbeddedParameter(url, "url=", false);

            HttpWebResponse response = null;

            if (!StringTools.IsTrivial(embeddedUrl))
                response = GetResponse(embeddedUrl);

            if (response == null)
            {
                return;
            }

            if (Http.ResponseCategory(response) != FileTools.TEXT)
            {
                return;
            }

            HttpStatusCode httpStatus = response.StatusCode;
            context.Response.StatusCode = (int) httpStatus;

            string content = Http.GetUncompressedTextContent(response);

            if (Http.IsXml(response))
            {
                context.Response.ContentType = "text/xml";
            }
            else
            {
                context.Response.ContentType = "text/plain";
            }
            
            context.Response.Write(content);
        }


        /*
         * The required IsReusable property.
         * 
         * Here returns false to avoid reuse.
         */
        public bool IsReusable { get { return false; } }

    }
}