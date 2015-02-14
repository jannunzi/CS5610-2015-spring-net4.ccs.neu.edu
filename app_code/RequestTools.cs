/*
    File: ~/app_code/RequestTools.cs
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.Text;
using System.Collections.Generic;
using System.Web;

namespace edu.neu.ccis.rasala
{
    /*
     * Tools to deal with the HttpRequest class,
     * specifically with url's and query strings.
     */
    public class RequestTools
    {
        /*
         * Returns
         * 
         *     request.Url.OriginalString
         * 
         * Note: As currently implemented by Microsoft, the
         * above string does not contain a fragment that
         * begins with the character '#'.
         */
        public static string OriginalUrl(HttpRequest request)
        {
            return request.Url.OriginalString;
        }


        /*
         * Returns the unescaped version of
         *
         *     request.Url.OriginalString
         *
         * using the method
         * 
         *     StringTools.Unescape
         */
        public static string Url(HttpRequest request)
        {
            return StringTools.Unescape(request.Url.OriginalString);
        }


        /*
         * Uses url = Url(request).
         * 
         * If url does not contain '?' then returns url.
         * 
         * Otherwise, returns the substring of url up to
         * but not including '?'.
         */
        public static string PlainUrl(HttpRequest request)
        {
            string url = Url(request);

            int qmark = url.IndexOf('?');

            if (qmark < 0)
                return url;

            return url.Substring(0, qmark);
        }


        /*
         * Uses url = Url(request), that is, it uses the
         * unescaped version of:
         * 
         *     request.Url.OriginalString
         * 
         * If url does not contain '?' then returns "".
         * 
         * Otherwise, returns as the query string the string
         * found as follows within url as follows:
         * 
         *     ...?query_string
         * 
         * Assumes that there is no fragment, #fragment, at
         * the end of the url.  This assumption is based on
         * current Microsoft convention.
         */
        public static string Query(HttpRequest request)
        {
            string url = Url(request);

            int qmark = url.IndexOf('?');

            if (qmark < 0)
                return "";

            return url.Substring(qmark + 1);
        }


        /*
         * Returns the result of splitting Query(request)
         * with the '&' character.
         * 
         * The string array returned has length 0 if and
         * only if Query(request) is an empty string.
         */
        public static string[] QueryParts(HttpRequest request)
        {
            string query = Query(request);

            if (query.Length == 0)
                return new string[0];

            return query.Split('&');
        }


        /*
         * Parses given string as if it is one part in a query
         * string.
         * 
         * If the given string contains "=" then returns in key
         * the substring before "=" and in val the substring
         * after "=".
         * 
         * Otherwise, returns "" in key and the given string in
         * val.
         */
        public static void ParseQueryPart
            (string querypart, out string key, out string val)
        {
            key = "";
            val = "";

            if (StringTools.IsTrivial(querypart))
                return;

            int spot = querypart.IndexOf("=");

            if (spot == -1)
            {
                val = querypart;
            }
            else
            {
                key = querypart.Substring(0, spot);
                val = querypart.Substring(spot + 1);
            }
        }


        /*
         * Returns an embedded parameter in the query string that
         * is introduced by a string specified here as param.
         * 
         * Further the embedded parameter must be contained in a
         * pair of matching bounding characters such as:
         * 
         *     "  '  |
         * 
         * The bounding character may not appear in the embedded
         * parameter.
         * 
         * The bounding character is determined as the first
         * character that occurs after param.
         * 
         * The embedded parameter is the substring between that
         * first character and its next occurence in the query
         * string.
         * 
         * 
         * For example, if the full url is:
         * 
         *     http://www.abc.com/seek.htm?url="http://www.xyz.com?a=1&b=2"
         * 
         * then the call:
         * 
         *     RequestTools.EmbeddedQueryParam(request, "url=")
         * 
         * returns:
         * 
         *     http://www.xyz.com?a=1&b=2
         * 
         * 
         * This method uses StringTools.FindParameter.
         * 
         * 
         * Note that this method works in situations where the
         * simple application of the method QueryParts will fail.
         */
        public static string EmbeddedQueryParam
            (HttpRequest request, string param)
        {
            string query = Query(request);

            return StringTools.FindParameter(query, param, false);
        }
    }
}