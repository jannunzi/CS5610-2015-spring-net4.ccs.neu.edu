/*
    File: ~/app_code/HTML_Tools.cs
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.IO;
using System.Text;

namespace edu.neu.ccis.rasala
{
    public static class HTML_Tools
    {
        /// <summary>
        /// HTML5 DOCTYPE
        /// </summary>
        public const string htmlDoctype = "<!DOCTYPE html>\n";


        /// <summary>
        /// text/html; charset='utf-8'
        /// </summary>
        public const string htmlContentType = "text/html; charset='utf-8'";


        /// <summary>
        /// Open html tag with lang='en'
        /// 
        /// To open html with another language use OpenTag
        /// or some similar mechanism.
        /// </summary>
        public const string open_html = "\n<html lang='en'>\n";

        /// <summary>
        /// Shut html tag
        /// </summary>
        public const string shut_html = "\n</html>\n";

        /// <summary>
        /// Open head tag
        /// </summary>
        public const string open_head = "\n<head>\n";

        /// <summary>
        /// Shut head tag
        /// </summary>
        public const string shut_head = "\n</head>\n";

        /// <summary>
        /// Open body tag
        /// </summary>
        public const string open_body = "\n<body>\n";

        /// <summary>
        /// Shut body tag
        /// </summary>
        public const string shut_body = "\n</body>\n";

        /// <summary>
        /// The standard meta tag: <meta charset='utf-8' />
        /// </summary>
        public const string standard_meta = "\n<meta charset='utf-8' />\n";
        
        /// <summary>
        /// Open title tag
        /// </summary>
        public const string open_title = "\n<title>";

        /// <summary>
        /// Shut title tag
        /// </summary>
        public const string shut_title = "</title>\n";


        /// <summary>
        /// Open css link tag for href
        /// </summary>
        public const string open_css_link =
            "\n<link rel='stylesheet' type='text/css' href='";

        /// <summary>
        /// Shut css link tag for href
        /// </summary>
        public const string shut_css_link =
            "' />\n";

        /// <summary>
        /// Open inline css tag
        /// </summary>
        public const string open_css_inline =
            "\n<style type='text/css'>\n";

        /// <summary>
        /// Open inline css tag before media attribute
        /// </summary>
        public const string open_css_inline_1 =
            "\n<style type='text/css";

        /// <summary>
        /// Open inline css tag after media attribute
        /// </summary>
        public const string open_css_inline_2 =
            "'>\n";

        /// <summary>
        /// Shut inline css tag
        /// </summary>
        public const string shut_css_inline =
            "\n</style>\n";

        /// <summary>
        /// Insert for css media attribute
        /// </summary>
        public const string media_css =
            "' media='";


        /// <summary>
        /// Open for Javascript file reference
        /// </summary>
        public const string script_file_1 =
            "\n<script type='text/javascript' src='";

        /// <summary>
        /// Shut for Javascript file reference
        /// </summary>
        public const string script_file_2 =
            "'></script>\n";

        /// <summary>
        /// Open for Javascript inline text insert
        /// </summary>
        public const string script_inline_1 =
            "\n<script type='text/javascript'>\n";

        /// <summary>
        /// Shut for Javascript inline text insert
        /// </summary>
        public const string script_inline_2 =
            "\n</script>\n";


        /// <summary>
        /// Open div
        /// </summary>
        public const string open_div = "\n<div>";

        /// <summary>
        /// Shut div
        /// </summary>
        public const string shut_div = "</div>\n";

        /// <summary>
        /// Open span
        /// </summary>
        public const string open_span = "<span>";

        /// <summary>
        /// Shut span
        /// </summary>
        public const string shut_span = "</span>";

        /// <summary>
        /// Open p
        /// </summary>
        public const string open_p = "\n<p>";

        /// <summary>
        /// Shut p
        /// </summary>
        public const string shut_p = "</p>\n";

        /// <summary>
        /// Open p tag with class='feedback'
        /// </summary>
        public const string open_p_feedback = "\n<p class='feedback'>";

        /// <summary>
        /// Open anchor tag
        /// </summary>
        public const string open_anchor =
            "<a href='";

        /// <summary>
        /// Middle section of anchor tag
        /// with no target
        /// </summary>
        public const string mid_anchor_plain =
            "'>";

        /// <summary>
        /// Middle section of anchor tag
        /// including target='_blank'
        /// </summary>
        public const string mid_anchor_blank =
            "' target='_blank'>";

        /// <summary>
        /// Shut anchor tag
        /// </summary>
        public const string shut_anchor =
            "</a>";


        /// <summary>
        /// Open pre tag
        /// </summary>
        public const string open_pre = "\n<pre>";

        /// <summary>
        /// Shut pre tag
        /// </summary>
        public const string shut_pre = "</pre>\n";

        /// <summary>
        /// Open pre tag with class='feedback'
        /// </summary>
        public const string open_pre_feedback =
            "\n<pre class='feedback'>";

        /// <summary>
        /// Open pre tag with class='feedback' and
        /// style='color:red'.
        /// </summary>
        public const string open_pre_feedback_red =
            "\n<pre class='feedback' style='color:red'>";

        /// <summary>
        /// Open code tag
        /// </summary>
        public const string open_code = "<code>";

        /// <summary>
        /// Shut code tag
        /// </summary>
        public const string shut_code = "</code>";

        /// <summary>
        /// Open code tag with class='feedback'
        /// </summary>
        public const string open_code_feedback = "<code class='feedback'>";


        /// <summary>
        /// Horizontal rule
        /// </summary>
        public const string hr = "\n<hr />\n";

        /// <summary>
        /// Line break
        /// </summary>
        public const string br = "\n<br />\n";


        /// <summary>
        /// Create the HTML page markup given:
        /// 
        ///     The language abbreviation for the html tag.
        ///     
        ///     The page title.
        ///     
        ///     Markup for the head.
        ///     
        ///     Attributes for the body.
        ///     
        ///     Markup for the body.
        /// 
        /// In the head, use <meta charset='utf-8' />.
        ///     
        /// If lang is trivial, use "en".
        /// 
        /// If any other parameter is trivial, add nothing for that
        /// parameter.
        /// </summary>
        public static string MakePageMarkup
            (string lang, string title,
             string headMarkup, string bodyAttributes, string bodyMarkup)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(htmlDoctype);

            if (StringTools.IsTrivial(lang))
            {
                builder.Append(open_html);
            }
            else
            {
                builder.Append("\n<html lang='");
                builder.Append(lang);
                builder.Append("'>\n");
            }

            // head

            builder.Append(open_head);
            builder.Append(standard_meta);

            if (!StringTools.IsTrivial(title))
            {
                builder.Append(open_title);
                builder.Append(title);
                builder.Append(shut_title);
            }

            if (!StringTools.IsTrivial(headMarkup))
                builder.Append(headMarkup);

            builder.Append(shut_head);

            // body

            if (StringTools.IsTrivial(bodyAttributes))
                builder.Append(open_body);
            else
                OpenTag(builder, "body", bodyAttributes);

            if (!StringTools.IsTrivial(bodyMarkup))
                builder.Append(bodyMarkup);

            builder.Append(shut_body);

            builder.Append(shut_html);

            return builder.ToString();
        }


        /*
         * Functions to build a generic HTML tag.
         */


        /// <summary>
        /// Adds an open tag with the given tag name to the
        /// given StringBuilder builder.
        /// 
        /// Adds the attributes list if non-trivial.
        /// 
        /// The attributes list should be a space-separated
        /// string of the form:
        /// 
        ///   attribute-name='attribute-value'
        /// 
        /// Does nothing if the tag name is trivial.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="tagname">The tag name</param>
        /// <param name="attributes">Optional attributes</param>
        public static void OpenTag
            (StringBuilder builder, string tagname, string attributes)
        {
            if (StringTools.IsTrivial(tagname))
                return;

            builder.Append("<");
            builder.Append(tagname);

            if (!StringTools.IsTrivial(attributes))
            {
                builder.Append(" ");
                builder.Append(attributes);
            }

            builder.Append(">");
        }


        /// <summary>
        /// Adds a shut tag with the given tag name to the
        /// given StringBuilder builder.
        /// 
        /// Adds a newline if requested.
        /// 
        /// Does nothing if the tag name is trivial.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="tagname">The tag name</param>
        /// <param name="newline">If true append newline</param>
        public static void ShutTag
            (StringBuilder builder, string tagname, bool newline)
        {
            if (StringTools.IsTrivial(tagname))
                return;

            builder.Append("</");
            builder.Append(tagname);
            builder.Append(">");

            if (newline)
                builder.Append("\n");
        }


        /// <summary>
        /// Adds an open tag with the given tag name to the
        /// given StringBuilder builder.
        /// 
        /// Then shuts the tag at the end with />
        /// 
        /// Adds the attributes list if non-trivial.
        /// 
        /// The attributes list should be a space-separated
        /// string of the form:
        /// 
        ///   attribute-name='attribute-value'
        /// 
        /// Adds a newline if requested.
        /// 
        /// Does nothing if the tag name is trivial.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="tagname">The tag name</param>
        /// <param name="attributes">Optional attributes</param>
        /// <param name="newline">If true append newline</param>
        public static void OpenShutTag
            (StringBuilder builder,
             string tagname, string attributes, bool newline)
        {
            if (StringTools.IsTrivial(tagname))
                return;

            builder.Append("<");
            builder.Append(tagname);

            if (!StringTools.IsTrivial(attributes))
            {
                builder.Append(" ");
                builder.Append(attributes);
            }

            builder.Append(" />");

            if (newline)
                builder.Append("\n");
        }


        public static string MakeATag
            (string href, string target, string text)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<a href='");
            builder.Append(href);

            if (!StringTools.IsTrivial(target))
            {
                builder.Append("' target='");
                builder.Append(target);
            }

            builder.Append("'>");
            builder.Append(text);
            builder.Append("</a>");

            return builder.ToString();
        }


        /*
         * Function to add a horizontal rule between
         * existing markup and new markup being added.
         */


        /// <summary>
        /// Adds a horizontal rule between the existing
        /// contents in the builder and the markup
        /// provided that both are non-trivial.
        /// </summary>
        public static void AppendMarkupWithHR
            (StringBuilder builder, string markup)
        {
            if (builder == null)
                return;

            if (StringTools.IsTrivial(markup))
                return;

            if (builder.Length > 0)
                builder.Append(HTML_Tools.hr);

            builder.Append(markup);
        }


        /*
         * Functions to build simple HTML tables.
         */


        /// <summary>
        /// Append the start of an HTML table with the given border,
        /// padding, and optional attributes
        /// to the given StringBuilder builder.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="border">The border size if positive</param>
        /// <param name="padding">The cellpadding size if positive</param>
        /// <param name="attributes">Optional attributes</param>
        public static void AppendTableStart
            (StringBuilder builder, int border, int padding, string attributes)
        {
            builder.Append("\n<table");

            if (border > 0)
            {
                builder.Append(" border='");
                builder.Append(border);
                builder.Append("'");
            }

            if (padding > 0)
            {
                builder.Append(" cellpadding='");
                builder.Append(padding);
                builder.Append("'");
            }

            if (!StringTools.IsTrivial(attributes))
            {
                builder.Append(" ");
                builder.Append(attributes);
            }

            builder.Append(">\n");
        }


        /// <summary>
        /// Append the end of an HTML table markup
        /// to the given StringBuilder builder.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        public static void AppendTableEnd
            (StringBuilder builder)
        {
            builder.Append("\n</table>\n");

        }


        /// <summary>
        /// Append an HTML table row to the given StringBuilder
        /// using the given arguments for the table cells.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="args">The row cell contents</param>
        public static void AppendTableRow
            (StringBuilder builder, params string[] args)
        {
            if ((args == null) || (args.Length == 0))
                return;

            int max = args.Length;

            builder.Append("\n<tr>\n");

            for (int i = 0; i < max; i++)
            {
                builder.Append("<td>");

                builder.Append((args[i] != null) ? args[i] : "");

                builder.Append("</td>\n");
            }

            builder.Append("</tr>\n");
        }


        /// <summary>
        /// Adds the HTML string for a horizontal table with the given items
        /// to the given StringBuilder builder.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="items">The items to install in the table</param>
        /// <param name="border">The border size if positive</param>
        /// <param name="padding">The cellpadding size if positive</param>
        /// <param name="attributes">Optional attributes</param>
        /// <returns></returns>
        public static void Hor_HTML_Table
            (StringBuilder builder,
             string[] items, int border, int padding, string attributes)
        {
            if (items == null)
                return;

            AppendTableStart(builder, border, padding, attributes);
            AppendTableRow(builder, items);
            AppendTableEnd(builder);
        }


        /// <summary>
        /// Returns the HTML string for a horizontal table with the given items.
        /// </summary>
        /// <param name="items">The items to install in the table</param>
        /// <param name="border">The border size if positive</param>
        /// <param name="padding">The cellpadding size if positive</param>
        /// <param name="attributes">Optional attributes</param>
        /// <returns></returns>
        public static string Hor_HTML_Table
            (string[] items, int border, int padding, string attributes)
        {
            StringBuilder builder = new StringBuilder();

            Hor_HTML_Table(builder, items, border, padding, attributes);

            return builder.ToString();
        }


        /// <summary>
        /// Adds the HTML string for a vertical table with the given items
        /// to the given StringBuilder builder.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="items">The items to install in the table</param>
        /// <param name="border">The border size if positive</param>
        /// <param name="padding">The cellpadding size if positive</param>
        /// <param name="attributes">Optional attributes</param>
        /// <returns></returns>
        public static void Ver_HTML_Table
            (StringBuilder builder,
             string[] items, int border, int padding, string attributes)
        {
            if (items == null)
                return;

            AppendTableStart(builder, border, padding, attributes);

            foreach (string s in items)
                AppendTableRow(builder, s);

            AppendTableEnd(builder);
        }


        /// <summary>
        /// Returns the HTML string for a vertical table with the given items.
        /// </summary>
        /// <param name="items">The items to install in the table</param>
        /// <param name="border">The border size if positive</param>
        /// <param name="padding">The cellpadding size if positive</param>
        /// <param name="attributes">Optional attributes</param>
        /// <returns></returns>
        public static string Ver_HTML_Table
            (string[] items, int border, int padding, string attributes)
        {
            StringBuilder builder = new StringBuilder();

            Ver_HTML_Table(builder, items, border, padding, attributes);

            return builder.ToString();
        }


        /// <summary>
        /// Adds the HTML string for a 2-dimensional table with the given items
        /// to the given StringBuilder builder.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="items">The items to install in the table</param>
        /// <param name="border">The border size if positive</param>
        /// <param name="padding">The cellpadding size if positive</param>
        /// <param name="attributes">Optional attributes</param>
        /// <returns></returns>
        public static void HTML_Table
            (StringBuilder builder,
             string[][] items, int border, int padding, string attributes)
        {
            if (items == null)
                return;

            AppendTableStart(builder, border, padding, attributes);

            foreach (string[] a in items)
                AppendTableRow(builder, a);

            AppendTableEnd(builder);
        }


        /// <summary>
        /// Returns the HTML string for a 2-dimensional table with the given items.
        /// </summary>
        /// <param name="items">The items to install in the table</param>
        /// <param name="border">The border size if positive</param>
        /// <param name="padding">The cellpadding size if positive</param>
        /// <param name="attributes">Optional attributes</param>
        /// <returns></returns>
        public static string HTML_Table
            (string[][] items, int border, int padding, string attributes)
        {
            StringBuilder builder = new StringBuilder();

            HTML_Table(builder, items, border, padding, attributes);

            return builder.ToString();
        }


        /*
         * Functions to build pre and code tags.
         */


        /// <summary>
        /// Adds x sandwiched between open and shut pre tags
        /// to the given StringBuilder builder.
        /// 
        /// If encode is true, first HTML encode x.
        /// 
        /// Does nothing if x is trivial.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="x">The text to sandwich</param>
        /// <param name="encode">If true HTML encode x</param>
        public static void pre
            (StringBuilder builder, string x, bool encode)
        {
            if (StringTools.IsTrivial(x))
                return;

            builder.Append(open_pre);

            if (encode)
                x = StringTools.HtmlEncode(x);

            builder.Append(x);

            builder.Append(shut_pre);
        }


        /// <summary>
        /// Returns x sandwiched between open and shut pre tags.
        /// 
        /// If encode is true, first HTML encode x.
        /// 
        /// Returns an empty string if x is trivial.
        /// </summary>
        /// <param name="x">The text to sandwich</param>
        /// <param name="encode">If true HTML encode x</param>
        public static string pre(string x, bool encode)
        {
            if (StringTools.IsTrivial(x))
                return "";

            x = encode ? StringTools.HtmlEncode(x) : x;

            return open_pre + x + shut_pre;
        }


        /// <summary>
        /// Returns x sandwiched between open and shut pre tags
        /// with no automatic HTML encoding.
        /// 
        /// Returns an empty string if x is trivial.
        /// </summary>
        /// <param name="x">The text to sandwich</param>
        public static string pre(string x)
        {
            if (StringTools.IsTrivial(x))
                return "";

            return open_pre + x + shut_pre;
        }


        /// <summary>
        /// Adds x sandwiched between open and shut code tags
        /// to the given StringBuilder builder.
        /// 
        /// If encode is true, first HTML encode x.
        /// 
        /// Does nothing if x is trivial.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="x">The text to sandwich</param>
        /// <param name="encode">If true HTML encode x</param>
        public static void code
            (StringBuilder builder, string x, bool encode)
        {
            if (StringTools.IsTrivial(x))
                return;

            builder.Append(open_code);

            if (encode)
                x = StringTools.HtmlEncode(x);

            builder.Append(x);

            builder.Append(shut_code);
        }


        /// <summary>
        /// Returns x sandwiched between open and shut code tags.
        /// 
        /// If encode is true, first HTML encode x.
        /// 
        /// Returns an empty string if x is trivial.
        /// </summary>
        /// <param name="x">The text to sandwich</param>
        /// <param name="encode">If true HTML encode x</param>
        public static string code(string x, bool encode)
        {
            if (StringTools.IsTrivial(x))
                return "";

            x = encode ? StringTools.HtmlEncode(x) : x;

            return open_code + x + shut_code;
        }


        /// <summary>
        /// Returns x sandwiched between open and shut code tags
        /// with no automatic HTML encoding.
        /// 
        /// Returns an empty string if x is trivial.
        /// </summary>
        /// <param name="x">The text to sandwich</param>
        public static string code(string x)
        {
            if (StringTools.IsTrivial(x))
                return "";

            return open_code + x + shut_code;
        }


        /*
         * Functions to provide feedback.
         */


        /// <summary>
        /// Adds feedback header and data markup constructed
        /// as follows to the given StringBuilder builder.
        /// 
        /// If the feedback header is non-trivial then the
        /// header is inserted in a p-tag with CSS class
        /// 'feedback'.
        /// 
        /// If the feedback data is non-trivial then the
        /// data is HTML-encoded and inserted in a pre-tag
        /// with CSS class 'feedback'.
        /// 
        /// If the feedback data is null then the constant
        /// string
        /// 
        ///   data is null
        /// 
        /// is inserted in a pre-tag with CSS class 'feedback'
        /// and with color styled as red.
        /// 
        /// If the feedback data is empty then the constant
        /// string
        /// 
        ///   data is empty
        /// 
        /// is inserted in a pre-tag with CSS class 'feedback'
        /// and with color styled as red.
        /// 
        /// See the const string FeedbackCSS.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="header">The feedback header</param>
        /// <param name="data">The feedback data</param>
        public static void FeedbackMarkup
            (StringBuilder builder, string header, string data)
        {
            FeedbackHeader(builder, header);
            FeedbackData(builder, data);
        }


        /// <summary>
        /// Returns feedback header and data markup constructed
        /// as follows.
        /// 
        /// If the feedback header is non-trivial then the
        /// header is inserted in a p-tag with CSS class
        /// 'feedback'.
        /// 
        /// If the feedback data is non-trivial then the
        /// data is HTML-encoded and inserted in a pre-tag
        /// with CSS class 'feedback'.
        /// 
        /// If the feedback data is null then the constant
        /// string
        /// 
        ///   data is null
        /// 
        /// is inserted in a pre-tag with CSS class 'feedback'
        /// and with color styled as red.
        /// 
        /// If the feedback data is empty then the constant
        /// string
        /// 
        ///   data is empty
        /// 
        /// is inserted in a pre-tag with CSS class 'feedback'
        /// and with color styled as red.
        /// 
        /// See the const string FeedbackCSS.
        /// </summary>
        /// <param name="header">The feedback header</param>
        /// <param name="data">The feedback data</param>
        public static string FeedbackMarkup(string header, string data)
        {
            StringBuilder builder = new StringBuilder();
            FeedbackMarkup(builder, header, data);
            return builder.ToString();
        }


        /// <summary>
        /// Adds feedback header markup constructed as follows
        /// to the given StringBuilder builder.
        /// 
        /// If the feedback header is non-trivial then the
        /// header is inserted in a p-tag with CSS class
        /// 'feedback'.
        /// 
        /// Otherwise, does nothing.
        /// 
        /// See the const string FeedbackCSS.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="header">The feedback header</param>
        public static void FeedbackHeader
            (StringBuilder builder, string header)
        {
            if (StringTools.IsTrivial(header))
                return;

            builder.Append(open_p_feedback);
            builder.Append(header);
            builder.Append(shut_p);
        }


        /// <summary>
        /// Returns feedback header markup constructed
        /// as follows.
        /// 
        /// If the feedback header is non-trivial then the
        /// header is inserted in a p-tag with CSS class
        /// 'feedback'.
        /// 
        /// See the const string FeedbackCSS.
        /// </summary>
        /// <param name="header">The feedback header</param>
        /// <param name="data">The feedback data</param>
        public static string FeedbackHeader(string header)
        {
            StringBuilder builder = new StringBuilder();
            FeedbackHeader(builder, header);
            return builder.ToString();
        }


        /// <summary>
        /// Adds feedback data markup constructed as follows
        /// to the given StringBuilder builder.
        /// 
        /// If the feedback data is non-trivial then the
        /// data is HTML encoded and inserted in a pre-tag
        /// with CSS class 'feedback'.
        /// 
        /// If the feedback data is null then the constant
        /// string
        /// 
        ///   data is null
        /// 
        /// is inserted in a pre-tag with CSS class 'feedback'
        /// and with color styled as red.
        /// 
        /// If the feedback data is empty then the constant
        /// string
        /// 
        ///   data is empty
        /// 
        /// is inserted in a pre-tag with CSS class 'feedback'
        /// and with color styled as red.
        /// 
        /// See the const string FeedbackCSS.
        /// </summary>
        /// <param name="builder">The builder to collect the markup</param>
        /// <param name="data">The feedback data</param>
        public static void FeedbackData
            (StringBuilder builder, string data)
        {
            if (StringTools.IsTrivial(data))
            {
                builder.Append(open_pre_feedback_red);

                if (data == null)
                {
                    builder.Append("data is null");
                }
                else
                {
                    builder.Append("data is empty");
                }
            }
            else
            {
                builder.Append(open_pre_feedback);
                builder.Append(StringTools.HtmlEncode(data));
            }

            builder.Append(shut_pre);
        }


        /// <summary>
        /// Returns feedback data markup constructed
        /// as follows.
        /// 
        /// If the feedback data is non-trivial then the
        /// data is HTML-encoded and inserted in a pre-tag
        /// with CSS class 'feedback'.
        /// 
        /// If the feedback data is null then the constant
        /// string
        /// 
        ///   data is null
        /// 
        /// is inserted in a pre-tag with CSS class 'feedback'
        /// and with color styled as red.
        /// 
        /// If the feedback data is empty then the constant
        /// string
        /// 
        ///   data is empty
        /// 
        /// is inserted in a pre-tag with CSS class 'feedback'
        /// and with color styled as red.
        /// 
        /// See the const string FeedbackCSS.
        /// </summary>
        /// <param name="header">The feedback header</param>
        /// <param name="data">The feedback data</param>
        public static string FeedbackData(string data)
        {
            StringBuilder builder = new StringBuilder();
            FeedbackData(builder, data);
            return builder.ToString();
        }


        /// <summary>
        /// The default inline CSS for the FeedbackMarkup
        /// function.
        /// </summary>
        public const string FeedbackCSS =
open_css_inline +

@"p.feedback
{
    font-size: 120%;
    font-weight: bold;
}

pre.feedback
{
    font-family: 'Consolas', 'Lucida Console', 'Courier New', 'Andale Mono', monospace;
    color: blue;
}
"
+ shut_css_inline;

    }
}