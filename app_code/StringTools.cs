using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace edu.neu.ccis.rasala
{
    public class StringTools
    {
        /**
         * Returns true if the given string
         * is null or of length 0.
         */
        public static bool IsTrivial(string s)
        {
            return (s == null) || (s.Length == 0);
        }


        /**
         * Returns true if the given list
         * is null or has count 0.
         */
        public static bool IsTrivial<T>(IList<T> list)
        {
            return (list == null) || (list.Count == 0);
        }


        /**
         * Uses a StringBuilder to concatenate the strings in the list
         * data and then returns the result.
         *
         * If the given data is null, returns the empty string.
         * 
         * If the given data is non-null, then each non-null item in
         * data will be concatenated.
         * 
         * Hence there is no need to worry about null data or null items.
         */
        public static string Build(IList<string> data)
        {
            if (data == null)
                return "";

            StringBuilder builder = new StringBuilder();

            foreach (string s in data)
                if (s != null)
                    builder.Append(s);

            return builder.ToString();
        }


        /**
         * Uses a StringBuilder to concatenate the strings in the list
         * data with the given separator and then returns the result.
         * 
         * If the given data is null, returns the empty string.
         * 
         * If the given data is non-null, then each non-null item in
         * data will be concatenated and then followed by the separator.
         * 
         * Hence there is no need to worry about null data or null items.
         */
        public static string Build(IList<string> data, string separator)
        {
            if (data == null)
                return "";

            if (IsTrivial(separator))
                return Build(data);

            StringBuilder builder = new StringBuilder();

            foreach (string s in data)
                if (s != null)
                {
                    builder.Append(s);
                    builder.Append(separator);
                }

            return builder.ToString();
        }


        /**
         * Returns a Range list consisting of the line ranges
         * for the consecutive lines in the content.
         * 
         * The end of a line is signaled by:
         *   1. A newline \n
         *   2. A carriage return \r
         *   3. The end of the content string
         * 
         * In these cases, the start of the next line is found
         * as follows:
         *   1. For \n, the next character after \n.
         *   2. For \r:
         *        If the next character is \n,
         *          then after the \n
         *          else after the \r.
         *   3. No next line.
         * 
         * 
         * Note that the regex pattern ^.*$ ends lines only at
         * \n or at the end of the content string.  This means
         * that any \r characters will be viewed as part of a
         * line rather than as part of a line break.
         * 
         * This does not correspond to what will be seen by an
         * end user if the content is displayed on an html web
         * page using the pre tag.
         * 
         * This method is designed to properly handle both \r
         * and \n and the pair \r\n as a single line break.
         */
        public static List<Range> LineRangeList(string content)
        {
            List<Range> list = new List<Range>();

            if (StringTools.IsTrivial(content))
                return list;

            int start = 0;
            int length = content.Length;

            while (start < length)
            {
                int index = start;

                while ((index < length)
                       && (content[index] != '\n')
                       && (content[index] != '\r'))
                    index++;

                list.Add(new Range(start, index - start));

                start = index + 1;

                if (start >= length)
                    break;

                // check for \r\n pair
                if ((content[index] == '\r') && (content[start] == '\n'))
                    start++;
            }

            return list;
        }


        /**
         * Returns a list of the lines in the given content.
         * 
         * If trim is false, all lines are returned as is.
         * 
         * If trim is true, then:
         *   Trim each line.
         *   Add a trimmed line to the list only if it is
         *   non-empty.
         *   
         * Utilizes linerangelist to define the line breaks.
         */
        public static List<string> LineList(string content, bool trim)
        {
            List<string> list = new List<string>();

            List<Range> rangeList = LineRangeList(content);

            int n = rangeList.Count;

            for (int i = 0; i < n; i++)
            {
                Range range = rangeList[i];

                string item = content.Substring(range.start, range.length);

                if (trim)
                {
                    item = item.Trim();

                    if (item.Length > 0)
                        list.Add(item);
                }
                else
                {
                    list.Add(item);
                }
            }

            return list;
        }


        /*
         * This function may be used to parse a query string for
         * a complex embedded parameter but it may also be used
         * for other purposes.
         * 
         * 
         * Returns an embedded parameter in a source string that
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
         * first character and its next occurence in the source
         * string.
         * 
         * If unescape is true then the method Unescape below
         * will replace the source with its unescaped version
         * before finding the embedded parameter.
         * 
         * 
         * If an error occurs, returns an empty string.
         * 
         * 
         * For example, if source is
         * 
         *     http://www.abc.com/seek.htm?url="http://www.xyz.com?a=1&b=2"
         * 
         * then the call
         * 
         *     FindParameter(source, "url=", true)
         * 
         * returns:
         * 
         *     http://www.xyz.com?a=1&b=2
         *
         * As you can see, this method of specifying and finding a
         * parameter allows a full url to be specified as a query
         * parameter to another url.
         */
        public static string FindParameter
            (string source, string param, bool unescape)
        {
            if (IsTrivial(source) || IsTrivial(param))
                return "";

            if (unescape)
                source = Unescape(source);

            try
            {
                int a = source.IndexOf(param);

                if (a >= 0)
                {
                    source = source.Substring(a + param.Length);

                    if (source.Length > 0)
                    {
                        char c = source[0];
                        source = source.Substring(1);

                        a = source.IndexOf(c);

                        if (a >= 0)
                        {
                            return source.Substring(0, a);
                        }
                    }

                }
            }
            catch { }
        
            return "";
        }


        /*
         * This method will construct a new string
         * by traversing the source and replacing
         * triplets of the form
         * 
         *     %xy
         * 
         * where x and y are hex digit characters
         * with the corresponding character in the
         * range 0 ... 255.
         * 
         * If % occurs but is not followed by two
         * hex digit characters then it is passed
         * through as is.
         * 
         * 
         * This method should not be needed since
         * there is the method:
         * 
         *     Uri.UnescapeDataString
         * 
         * However, the Uri.UnescapeDataString method
         * throws an exception if % occurs but is not 
         * followed by two hex digit characters.
         * 
         * This exception is undocumented in the MSDN
         * documentation.
         */
        public static string Unescape(string source)
        {
            if (IsTrivial(source))
                return source;

            char percent = '%';

            int i = 0;
            int n = source.Length;

            StringBuilder builder = new StringBuilder();

            while (i < n)
            {
                char c = source[i];

                if ((c != percent) || ((i + 2) >= n))
                {
                    builder.Append(c);
                    i++;

                    continue;
                }

                char d = source[i + 1];
                char e = source[i + 2];

                int p = HexValue(d);
                int q = HexValue(e);

                if ((p < 0) || (q < 0))
                {
                    builder.Append(c);
                    i++;

                    continue;
                }

                char a = (char)(16 * p + q);

                builder.Append(a);
                i += 3;
            }

            return builder.ToString();
        }


        /*
         * Returns the integer value between 0 ... 15
         * of a hex digit character or else returns -1.
         */
        public static int HexValue(char c)
        {
            if (('0' <= c) && (c <= '9'))
                return (int)c - (int)'0';

            if (('A' <= c) && (c <= 'F'))
                return (int)c - (int)'A' + 10;

            if (('a' <= c) && (c <= 'f'))
                return (int)c - (int)'a' + 10;

            return -1;
        }


        /** Returns the text of an Enum. */
        public static string textual(Enum e)
        {
            return e.ToString();
        }


        /**
         * Returns the value of the underlying type
         * of an Enum entity converted to a string.
         */
        public static string numeric(Enum e)
        {
            Type undertype = Enum.GetUnderlyingType(e.GetType());

            return Convert.ChangeType(e, undertype).ToString();
        }


        // Regular expression for any of single quote, double quote,
        // less than, greater than, ampersand.

        private static Regex hasHtmlSpecialRegex = new Regex("\"|'|<|>|&");

        // Regex for tab
        private static Regex hasTabRegex = new Regex("\t");

        // Regex for carriage return
        private static Regex hasReturnRegex = new Regex("\r");

        // Regex for line feed
        private static Regex hasLinefeedRegex = new Regex("\n");



        /**
         * Returns the special html encoding of c if c is one of
         * the following:
         * 
         *      single quote:   "&apos;"
         *      double quote:   "&quot;"
         *      less than:      "&lt;"
         *      greater than:   "&gt;"
         *      ampersand:      "&amp;"
         * 
         * Otherwise returns c transformed into a string.
         */
        public static string HtmlEncode(char c)
        {
            switch (c)
            {
                case '\'': return "&apos;";

                case '\"': return "&quot;";

                case '<': return "&lt;";

                case '>': return "&gt;";

                case '&': return "&amp;";

                default: return c.ToString();
            }
        }


        /**
         * Returns true if c is single quote, double quote,
         * less than, greater than, or ampersand.
         * 
         * Otherwise, returns false.
         */
        public static bool isHtmlSpecial(char c)
        {
            switch (c)
            {
                case '\'':
                case '\"':
                case '<':
                case '>':
                case '&':
                    return true;

                default:
                    return false;
            }
        }


        /**
         * Returns a replacement for the given string s using
         * the following character substitutions:
         * 
         *      single quote:   "&apos;"
         *      double quote:   "&quot;"
         *      less than:      "&lt;"
         *      greater than:   "&gt;"
         *      ampersand:      "&amp;"
         * 
         * If the string s does not contain any of the special
         * characters, it is returned as is.
         * 
         * The advantage of encoding both single quote and
         * double quote is that the returned string may then
         * be safely quoted with either single or double quotes.
         */
        public static string HtmlEncode(string s)
        {
            if (IsTrivial(s))
                return s;

            if (!hasHtmlSpecialRegex.IsMatch(s))
                return s;

            StringBuilder builder = new StringBuilder();

            int length = s.Length;

            for (int i = 0; i < length; i++)
            {
                char c = s[i];

                if (isHtmlSpecial(c))
                {
                    builder.Append(HtmlEncode(c));
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }


        /*
         * Returns a string with the tabs in s converted to blanks
         * based on the given tabsize.
         * 
         * If omitted, tabsize defaults to 4.
         * 
         * Returns the original string if:
         *   s is trivial
         *   s does not contain a tab character
         *   The given tabsize is less than or equal to 0
         *  
         * This method simply converts tabs and does no other task.
         * 
         * If tabs are converted then the return string normalizes
         * the line breaks to '\n'.
         */
        public static string TabsToBlanks
            (string s, int tabsize = 4)
        {
            if (IsTrivial(s))
                return s;

            if (tabsize <= 0)
                return s;

            if (!hasTabRegex.IsMatch(s))
                return s;

            StringBuilder builder = new StringBuilder();

            int length = s.Length;

            List<Range> linerangelist = StringTools.LineRangeList(s);

            int linecount = linerangelist.Count;
            int lineindex = 0;

            while (lineindex < linecount)
            {
                // process the line at the lineindex
                Range linerange = linerangelist[lineindex];

                int start = linerange.start;
                int limit = linerange.limit;

                // increment lineindex here
                lineindex++;

                // index is the character position in the string
                int index = start;

                // position is the character position on the line
                // after any earlier replacement of tabs by blanks
                int position = 0;

                while (index < limit)
                {
                    char c = s[index];

                    if (c == '\t')
                    {
                        int spaces = tabsize - (position % tabsize);

                        for (int i = 0; i < spaces; i++)
                            builder.Append(' ');

                        position += spaces;
                    }
                    else
                    {
                        builder.Append(c);

                        position++;
                    }

                    index++;
                }

                if (limit < length)
                    builder.Append('\n');
            }

            return builder.ToString();
        }


        /**
         * Prepares the given string s for presentation on an html
         * page.
         * 
         * Html encodes the special characters as follows:
         * 
         *      single quote:   "&apos;"
         *      double quote:   "&quot;"
         *      less than:      "&lt;"
         *      greater than:   "&gt;"
         *      ampersand:      "&amp;"
         * 
         * Replaces tabs by blanks relative to the given tabsize.
         *   Default: tabsize = 4
         * 
         * Prefaces each line with a line number if desired.
         *   Default: showlinenumbers = false
         * 
         * Line numbers displayed start at 1 to follow the pattern
         * used in Visual Studio.
         * 
         * Will return the original string s if s does not need any
         * of the following transformations:
         *   html encoding
         *   tabs to blanks
         *   line numbering
         *   
         * If any transformations take place then the return string
         * normalizes the line breaks to '\n'.
         * 
         * The result of this method should be displayed using
         * preformatted text, that is, via a pre-tag or via the css
         * setting white-space:pre.
         */
        public static string TextToHtml
            (string s, int tabsize = 4, bool showlinenumbers = false)
        {
            if (IsTrivial(s))
                return s;

            bool replaceHtml = hasHtmlSpecialRegex.IsMatch(s);

            bool replaceTabs = (tabsize > 0) && (hasTabRegex.IsMatch(s));

            bool needsWork = replaceHtml || replaceTabs || showlinenumbers;

            if (!needsWork)
                return s;

            StringBuilder builder = new StringBuilder();

            int length = s.Length;

            List<Range> linerangelist = StringTools.LineRangeList(s);

            int linecount = linerangelist.Count;
            int lineindex = 0;

            while (lineindex < linecount)
            {
                // process the line at the lineindex
                Range linerange = linerangelist[lineindex];

                int start = linerange.start;
                int limit = linerange.limit;

                // increment lineindex here
                // since visible line numbers start at 1

                lineindex++;

                // if showlinenumbers is true, add the line number

                if (showlinenumbers)
                {
                    builder.Append(lineindex.ToString("000000  "));
                }

                // index is the character position in the string
                int index = start;

                // position is the character position on the line
                // after any earlier replacement of tabs by blanks
                //
                // characters used for the line number are ignored
                int position = 0;

                while (index < limit)
                {
                    char c = s[index];

                    if (c == '\t')
                    {
                        if (tabsize > 0)
                        {
                            int spaces = tabsize - (position % tabsize);

                            for (int i = 0; i < spaces; i++)
                                builder.Append(' ');

                            position += spaces;
                        }
                        else
                        {
                            builder.Append(c);

                            position++;
                        }
                    }
                    else
                    {
                        if (isHtmlSpecial(c))
                        {
                            builder.Append(HtmlEncode(c));
                        }
                        else
                        {
                            builder.Append(c);
                        }

                        position++;
                    }

                    index++;
                }

                if (limit < length)
                    builder.Append('\n');
            }

            return builder.ToString();
        }


        public const string highlightopen = @"<span class='highlight'>";

        public const string highlightshut = @"</span>";


        /**
         * Prepares the given string s for presentation on an html
         * page.
         * 
         * Enhances the method TextToHtml by permitting ranges of
         * the text to be highlighted.  Each range is placed in a
         * span-tag with css class 'highlight'.  Thus, the nature
         * of the highlighting is controlled by the css on the
         * page.
         * 
         * Html encodes the special characters as follows:
         * 
         *      single quote:   "&apos;"
         *      double quote:   "&quot;"
         *      less than:      "&lt;"
         *      greater than:   "&gt;"
         *      ampersand:      "&amp;"
         * 
         * Replaces tabs by blanks relative to the given tabsize.
         *   Default: tabsize = 4
         * 
         * Prefaces each line with a line number if desired.
         *   Default: showlinenumbers = true
         * 
         * Permits the caller to decide whether to show all lines or
         * to show only the lines with highlights.
         *   Default: showall = false
         * 
         * The reason for these defaults is that this method is used
         * to show the results of a search.  In this situation, the
         * user is most interested in seeing the lines that contain
         * search results and not all other lines.  By setting
         *   showlinenumbers = true
         *   showall = false
         * the lines that contain search results are identified and
         * no other lines are displayed.
         * 
         * Line numbers displayed start at 1 to follow the pattern
         * used in Visual Studio.
         * 
         * There is an interesting edge case if no ranges are given
         * to be highlighted.  In that case:
         * 
         *   If showall=false, there is nothing to show and this
         *   method returns an empty string.
         *   
         *   If showall=true, this method simply calls TextToHtml.
         * 
         * If any transformations take place then the return string
         * normalizes the line breaks to '\n'.
         * 
         * The result of this method should be displayed using
         * preformatted text, that is, via a pre-tag or via the css
         * setting white-space:pre.
         */
        public static string HighlightMarkup
            (string s,
             List<Range> highlightlist,
             int tabsize = 4,
             bool showlinenumbers = true,
             bool showall = false)
        {
            if (IsTrivial(s))
                return s;

            if (IsTrivial(highlightlist))
            {
                if (showall)
                    return TextToHtml(s, tabsize, showlinenumbers);
                else
                    return "";
            }

            StringBuilder builder = new StringBuilder();

            int length = s.Length;

            // Set up for line processing

            List<Range> linerangelist = StringTools.LineRangeList(s);

            int linecount = linerangelist.Count;
            int lineindex = 0;

            // Set up for highlight processing

            int highcount = highlightlist.Count;
            int highindex = 0;

            // data for dealing with the highlight overlap range list
            // for hightlight ranges than intersect a line range

            // overlap: the list of highlight range overlaps
            // overlapcount = overlap.Count

            // next: next highlight overlap range to process
            // nextrange = overlap[next]

            // open: nextrange.start
            // shut: nextrange.limit

            List<Range> overlap = null;
            int overlapcount = 0;

            int next = 0;
            Range nextrange;

            int open = -1;
            int shut = -1;


            while (lineindex < linecount)
            {
                // process the line at the lineindex
                Range linerange = linerangelist[lineindex];

                int start = linerange.start;
                int limit = linerange.limit;

                // increment lineindex here
                // since visible line numbers start at 1
                //
                // this guarantees that lineindex is updated
                // if this line will be skipped later because
                // it has no highlights

                lineindex++;


                // construct the overlap highlight ranges that
                // intersect this line range, if any

                overlap = null;
                overlapcount = 0;

                if (highindex < highcount)
                {
                    while ((highindex < highcount) &&
                           (highlightlist[highindex].limit < start))
                    {
                        highindex++;
                    }

                    while ((highindex < highcount) &&
                           (highlightlist[highindex].start < limit))
                    {
                        Range? r = Range.Intersect(linerange, highlightlist[highindex]);

                        if (r.HasValue)
                        {
                            Range rv = r.Value;

                            if (rv.length > 0)
                            {
                                if (overlap == null)
                                    overlap = new List<Range>();

                                overlap.Add(rv);
                            }

                            if (highlightlist[highindex].limit <= limit)
                            {
                                highindex++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (overlap != null)
                    overlapcount = overlap.Count;

                // skip this line if
                //   showall is false
                // and
                //   there is no highlight to do

                if ((!showall) && (overlapcount == 0))
                    continue;


                // if showlinenumbers is true, add the line number
                // at this stage of the processing

                if (showlinenumbers)
                {
                    builder.Append(lineindex.ToString("000000  "));
                }


                // index is the character position in the string
                int index = start;

                // position is the character position on the line
                // after any earlier replacement of tabs by blanks
                //
                // characters used for the line number are ignored
                int position = 0;


                // get set to process highlights
                next = 0;

                if (next < overlapcount)
                {
                    nextrange = overlap[next];
                    open = nextrange.start;
                    shut = nextrange.limit;
                }
                else
                {
                    open = -1;
                    shut = -1;
                }


                while (index < limit)
                {
                    // step 1: deal with highlight at this index

                    if (index == shut)
                    {
                        builder.Append(highlightshut);

                        next++;

                        if (next < overlapcount)
                        {
                            nextrange = overlap[next];
                            open = nextrange.start;
                            shut = nextrange.limit;
                        }
                        else
                        {
                            open = -1;
                            shut = -1;
                        }
                    }

                    if (index == open)
                    {
                        builder.Append(highlightopen);
                    }

                    // step 2: deal with character at this index

                    char c = s[index];

                    if (c == '\t')
                    {
                        if (tabsize > 0)
                        {
                            int spaces = tabsize - (position % tabsize);

                            for (int i = 0; i < spaces; i++)
                                builder.Append(' ');

                            position += spaces;
                        }
                        else
                        {
                            builder.Append(c);

                            position++;
                        }
                    }
                    else
                    {
                        if (isHtmlSpecial(c))
                        {
                            builder.Append(HtmlEncode(c));
                        }
                        else
                        {
                            builder.Append(c);
                        }

                        position++;
                    }

                    index++;
                }

                if (limit == shut)
                    builder.Append(highlightshut);

                if (limit < length)
                    builder.Append('\n');
            }

            return builder.ToString();
        }


        /*
         * Prepares the given string s for presentation on an html
         * page.
         * 
         * In this method the string s must be a single line, that
         * is, it may not contain carriage return or line feed.
         * 
         * Enhances the method TextToHtml by permitting ranges of
         * the text to be highlighted.  Each range is placed in a
         * span-tag with css class 'highlight'.  Thus, the nature
         * of the highlighting is controlled by the css on the
         * page.
         * 
         * Html encodes the special characters as follows:
         * 
         *      single quote:   "&apos;"
         *      double quote:   "&quot;"
         *      less than:      "&lt;"
         *      greater than:   "&gt;"
         *      ampersand:      "&amp;"
         * 
         * Replaces tabs by blanks relative to the given tabsize.
         *   Default: tabsize = 4
         * 
         * Prefaces the line with a line number if desired.
         *   Default: showlinenumber = true
         * 
         * The caller must pass the exact line number to be printed
         * since that cannot be determined by this method.  The
         * default value of 0 is therefore a placeholder and allows
         * the use of a "named parameter" in the parameter list:
         * 
         *   linenumber: ...
         * 
         * 
         * The purpose of this method is to allow the highlighting of
         * select lines in very large files that cannot be read into
         * memory all at once.  Such files may be read line by line
         * and those lines that need to be sent to a page may be
         * provided with highlights and/or a line number.
         */
        public static string HighlightMarkupForOneLine
            (string s,
             List<Range> highlightlist,
             int tabsize = 4,
             bool showlinenumber = true,
             long linenumber = 0)
        {
            if (s == null)
                return "";

            StringBuilder builder = new StringBuilder();

            // Set up for line processing

            int start = 0;
            int limit = s.Length;

            // Set up for highlight processing

            int highcount = IsTrivial(highlightlist)
                ? 0
                : highlightlist.Count;

            // next: next highlight range to process
            // nextrange = highlightlist[next]

            // open: nextrange.start
            // shut: nextrange.limit

            int next = 0;
            Range nextrange;

            int open = -1;
            int shut = -1;


            // if showlinenumber is true, add the line number
            // at this stage of the processing

            if (showlinenumber)
            {
                builder.Append(linenumber.ToString("000000000  "));
            }


            // index is the character position in the string
            int index = start;

            // position is the character position on the line
            // after any earlier replacement of tabs by blanks
            //
            // characters used for the line number are ignored
            int position = 0;


            // get set to process highlights
            next = 0;

            if (next < highcount)
            {
                nextrange = highlightlist[next];
                open = nextrange.start;
                shut = nextrange.limit;
            }
            else
            {
                open = -1;
                shut = -1;
            }


            while (index < limit)
            {
                // step 1: deal with highlight at this index

                if (index == shut)
                {
                    builder.Append(highlightshut);

                    next++;

                    if (next < highcount)
                    {
                        nextrange = highlightlist[next];
                        open = nextrange.start;
                        shut = nextrange.limit;
                    }
                    else
                    {
                        open = -1;
                        shut = -1;
                    }
                }

                if (index == open)
                {
                    builder.Append(highlightopen);
                }

                // step 2: deal with character at this index

                char c = s[index];

                if (c == '\t')
                {
                    if (tabsize > 0)
                    {
                        int spaces = tabsize - (position % tabsize);

                        for (int i = 0; i < spaces; i++)
                            builder.Append(' ');

                        position += spaces;
                    }
                    else
                    {
                        builder.Append(c);

                        position++;
                    }
                }
                else
                {
                    if (isHtmlSpecial(c))
                    {
                        builder.Append(HtmlEncode(c));
                    }
                    else
                    {
                        builder.Append(c);
                    }

                    position++;
                }

                index++;
            }

            if (limit == shut)
                builder.Append(highlightshut);

            return builder.ToString();
        }
    }
}