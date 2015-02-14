using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using edu.neu.ccis.rasala;

namespace edu.neu.ccis.rasala.controls
{

    /// <summary>
    /// StyledMessage is a simple experimental control
    /// that will output a message enclosed in a p-tag
    /// and surrounded by hr-tags.
    /// 
    /// The message is supplied in the Message attribute.
    /// 
    /// This control improves on SimpleMessage by:
    /// 
    ///   Using the CssClass attribute of the base class
    ///   WebControl to set the css class on the p-tag
    ///   
    ///   Using the additional member InlineStyle to set
    ///   the style property on the p-tag.
    /// </summary>
    public class StyledMessage : WebControl
    {
        private string _message =
            "This is the StyledMessage control default message";

        public string Message
        {
            get { return _message; }
            set { if (value != null) _message = value; }
        }


        private string _inlinestyle = "";

        public string InlineStyle
        {
            get { return _inlinestyle; }
            set { if (value != null) _inlinestyle = value; }
        }


        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("\n<hr />\n");
            builder.Append("<p");

            if (!StringTools.IsTrivial(CssClass))
            {
                builder.Append(" class='");
                builder.Append(CssClass);
                builder.Append("'");
            }

            if (!StringTools.IsTrivial(InlineStyle))
            {
                builder.Append(" style='");
                builder.Append(InlineStyle);
                builder.Append("'");
            }

            builder.Append(">");
            builder.Append(Message);
            builder.Append("</p>");
            builder.Append("\n<hr />\n");

            output.Write(builder.ToString());
        }
    }
}