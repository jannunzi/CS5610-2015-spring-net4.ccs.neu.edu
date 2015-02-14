using System;
using System.Text;
using System.IO;

namespace edu.neu.ccis.rasala
{
    /** Adapted from blog code by Peter Douglass. */

    /// <summary>
    /// BlogItem constructs one blog item created from the html
    /// data in the file used in its constructor.
    /// 
    /// The html in the file should consist only of body content.
    /// There should no doctype, html, head, or body tags.
    /// 
    /// The file extension of the file should be .htm.
    /// 
    /// To facilitate styling by CSS, the blog item will be made
    /// with 2 div tags with classes name item_head and item_body.
    /// 
    /// The item_head div will enclose a span named item_head_span.
    /// This allows the separation of styles that apply to a block
    /// element from those that apply to an inline element.
    /// 
    /// The content of the item_head_span span tag is the earliest
    /// date and time of the file.  To permit separate styling if
    /// desired, these entities are nested in spans named
    /// item_date_span and item_time_span.
    /// 
    /// The item_body div contains the content of the file verbatim.
    /// 
    /// Since the content of the file should be html, it defines its
    /// own structure and styles.  These may be influenced by styles
    /// associated with item_body and the enclosing page if desired.
    /// 
    /// On 1/21/09, added namespace and refactored to add the method
    /// AppendTo so that a StringBuilder may be passed to a BlogItem
    /// as a blog is created.
    /// 
    /// On 4/29/09, replaced creation date with earliest date.
    /// This is because of the following phenomenon.  If a
    /// file is copied to a new machine, it may carry with it
    /// its modification date but have a creation date on the
    /// new system that reflects the moment of copying.
    /// Whether or not this phenomenon happens depends on the
    /// software used to do the copying and may not be easily
    /// predictable.
    /// </summary>
    public class BlogItem
    {
        /// <summary>
        /// The html file with the blog item content.
        /// </summary>
        FileInfo _file;

        /// <summary>
        /// The earliest date-time of
        /// the html file with the blog item content.
        /// </summary>
        DateTime _datetime;

        /// <summary>
        /// The constructed HTML for the blog item.
        /// </summary>
        string _BlogItemHTML;


        /// <summary>
        /// The BlogItem constructor that accepts
        /// the HTML file with the blog item content.
        /// </summary>
        /// <param name="file">The file with the blog item HTML content.</param>
        public BlogItem(FileInfo file)
        {
            _file = file;

            if (file != null)
                _datetime = file.EarliestTime();
        }


        /// <summary>
        /// Appends the html for the blog item content that
        /// is constructed using the earliest date and time
        /// of the internal file together with its content
        /// to the given StringBuilder object.
        /// </summary>
        /// <param name="builder">The builder on which to
        /// append the blog item content.</param>
        public void AppendTo(StringBuilder builder)
        {
            if (builder == null)
                return;

            builder.Append("<div class='item_head'>\n");
            builder.Append("<span class='item_head_span'>\n");
            builder.Append("<span class='item_date_span'>");
            builder.Append(_datetime.ToLongDateString());
            builder.Append("</span>\n");
            builder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;\n");
            builder.Append("<span class='item_time_span'>");
            builder.Append(_datetime.ToLongTimeString());
            builder.Append("</span>\n");
            builder.Append("</span>\n");
            builder.Append("</div>\n");

            builder.Append("<div class='item_body'>\n");

            try
            {
                using (var reader = new StreamReader(_file.FullName))
                {
                    builder.Append(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                builder.Append("<p>Error in reading blog file: ");
                builder.Append(_file.Name);
                builder.Append("</p>\n");
                builder.Append("<p>Error message:</p>\n<p>");
                builder.Append(ex.Message);
                builder.Append("</p>\n");
            }

            builder.Append("</div>\n");
        }


        /// <summary>
        /// Returns the html for the blog item content that
        /// is constructed using the earliest date and time
        /// of the internal file together with its content.
        /// 
        /// Returns the same string as BlogItemHTML.
        /// </summary>
        public override string ToString()
        {
            if (_file == null)
                return "";

            if (_BlogItemHTML != null)
                return _BlogItemHTML;

            StringBuilder builder = new StringBuilder();

            AppendTo(builder);

            _BlogItemHTML = builder.ToString();

            return _BlogItemHTML;
        }


        /// <summary>
        /// Returns the html for the blog item content that
        /// is constructed using the earliest date and time
        /// of the internal file together with its content.
        /// 
        /// Returns the same string as ToString().
        /// </summary>
        public string BlogItemHTML
        {
            get
            {
                return ToString();
            }
        }


        /// <summary>
        /// The html file with the blog item content.
        /// </summary>
        public FileInfo BlogItemFile
        {
            get
            {
                return _file;
            }
        }


        /// <summary>
        /// The earliest date-time of
        /// the html file with the blog item content.
        /// </summary>
        public DateTime BlogItemEarliestTime
        {
            get
            {
                return _datetime;
            }
        }
    }
}
