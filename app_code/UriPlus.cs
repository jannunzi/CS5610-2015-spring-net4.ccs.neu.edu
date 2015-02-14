using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Runtime.Serialization;

namespace edu.neu.ccis.rasala
{
    /// <summary>
    /// Summary description for UriPlus.
    /// 
    /// Extends Uri with additional properties that provide a finer level
    /// of parsing of a Uri string.
    /// </summary>
    public class UriPlus : Uri
    {
        public UriPlus(string uriString)
            : base(uriString) { }


        public UriPlus(string uriString, UriKind uriKind)
            : base(uriString, uriKind) { }


        public UriPlus(Uri baseUri, string relativeUri)
            : base(baseUri, relativeUri) { }


        public UriPlus(Uri baseUri, Uri relativeUri)
            : base(baseUri, relativeUri) { }


        protected UriPlus(
            SerializationInfo serializationInfo,
            StreamingContext streamingContext)

            : base(serializationInfo, streamingContext) { }


        private string[][] parsedsegments = null;

        private string filename = null;

        private string extension = null;

        private string[] parsedquery = null;


        /// <summary>
        /// Helper method to construct the parsed segments.
        /// </summary>
        private void makeParsedSegments()
        {
            if (parsedsegments != null)
                return;

            string[] segments = Segments;

            int N = segments.Length;

            parsedsegments = new string[N][];

            char[] split = { ';' };

            for (int i = 0; i < N; i++)
            {
                parsedsegments[i] = segments[i].Split(split);

                // deal with placement of slash
                int P = parsedsegments[i].Length;

                if (P > 1)
                {
                    int Q = P - 1;
                    int R = parsedsegments[i][Q].Length - 1;

                    if (R >= 0)
                    {
                        if (parsedsegments[i][Q][R] == '/')
                        {
                            parsedsegments[i][Q] =
                                parsedsegments[i][Q].Remove(R);

                            parsedsegments[i][0] += '/';
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Helper method to copy the parsed segments data.
        /// </summary>
        private string[][] copyParsedSegments()
        {
            makeParsedSegments();

            int N = parsedsegments.Length;

            string[][] copy = new string[N][];

            for (int i = 0; i < N; i++)
            {
                int L = parsedsegments[i].Length;

                copy[i] = new string[L];

                for (int j = 0; j < L; j++)
                    copy[i][j] = parsedsegments[i][j];
            }

            return copy;
        }


        /// <summary>
        /// The Segments property returns a 1-dimensional array of
        /// string that contains the segments of the Uri.
        /// 
        /// Each segment consists either of a slash or of a string
        /// of the form:
        ///     "xyz;param1;param2;..."
        /// or
        ///     "xyz;param1;param2;.../"
        /// 
        /// 
        /// In the property, ParsedSegments, each such a segment is
        /// expanded into a string[].
        /// 
        /// The string "/" expands to { "/" }.
        /// 
        /// A string of the first form above expands to:
        ///     { "xyz", "param1", "param2", ... }
        /// 
        /// A string of the second form above expands to:
        ///     { "xyz/", "param1", "param2", ... }
        /// 
        /// Notice that, in the last situation, we move the slash
        /// to the initial portion of the segment.
        /// </summary>
        public string[][] ParsedSegments
        {
            get
            {
                return copyParsedSegments();
            }
        }


        /// <summary>
        /// This property examines the string fn in column 0 of the
        /// last row of the ParsedSegments array.
        /// 
        /// If fn does not end in a slash, then fn is returned;
        /// otherwise "" is returned.
        /// </summary>
        public string FileName
        {
            get
            {
                if (filename != null)
                    return filename;

                makeParsedSegments();

                int N = parsedsegments.Length - 1;

                if (N >= 0)
                {
                    string fn = parsedsegments[N][0];

                    int L = fn.Length - 1;

                    if ((L >= 0) || (fn[L] != '/'))
                        filename = fn;
                }

                return filename;
            }
        }


        /// <summary>
        /// If FileName contains a period character, then this property
        /// returns the subtring of FileName including the last period
        /// and all subsequent characters; the characters are converted
        /// to lower case prior to being returned.
        /// 
        /// If FileName does not contain a period character, returns "".
        /// </summary>
        public string FileExtension
        {
            get
            {
                if (extension != null)
                    return extension;

                extension = "";

                string fn = FileName;

                int L = fn.LastIndexOf('.');

                if (L >= 0)
                {
                    extension = fn.Substring(L).ToLower();
                }

                return extension;
            }
        }


        private void makeParsedQuery()
        {
            if (parsedquery != null)
                return;

            string query = Query;

            // No query or query is "?" with no data
            if ((query == null) || (query.Length <= 1))
            {
                parsedquery = new string[0];
                return;
            }

            query = query.Substring(1);

            char[] split = { '&' };

            parsedquery = query.Split(split);
        }


        private string[] copyParsedQuery()
        {
            makeParsedQuery();

            int N = parsedquery.Length;

            string[] copy = new string[N];

            for (int i = 0; i < N; i++)
                copy[i] = parsedquery[i];

            return copy;
        }


        public string[] ParsedQuery
        {
            get
            {
                return copyParsedQuery();
            }
        }
    }
}