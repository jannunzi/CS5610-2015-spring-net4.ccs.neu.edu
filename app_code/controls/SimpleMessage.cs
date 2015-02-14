using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using edu.neu.ccis.rasala;

namespace edu.neu.ccis.rasala.controls
{

    /// <summary>
    /// SimpleMessage is a simple experimental control
    /// that will output a message enclosed in a p-tag
    /// and surrounded by hr-tags.
    /// 
    /// The message is supplied in the Message attribute.
    /// </summary>
    public class SimpleMessage : WebControl
    {
        private string _message =
            "This is the SimpleMessage control default message";

        public string Message
        {
            get { return _message; }
            set { if (value != null) _message = value; }
        }

        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("\n<hr />\n<p>");
            builder.Append(Message);
            builder.Append("</p>\n<hr />\n");

            output.Write(builder.ToString());
        }
    }
}