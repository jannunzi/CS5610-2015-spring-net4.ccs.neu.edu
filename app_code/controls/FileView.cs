using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using edu.neu.ccis.rasala;

namespace edu.neu.ccis.rasala.controls
{
    /// <summary>
    /// The FileView control is directly inspired by the AspxSource
    /// control designed by Peter Douglass.
    /// 
    /// The purpose of this control is to link to a page with the
    /// source of the page on which the control is placed.
    /// 
    /// Because Peter's control depends on a series of definitions
    /// on his site including his Source Server, I decided to build
    /// a similar control using my own definitions.
    /// 
    /// 
    /// On February 8, 2011, this code was completely rewritten to
    /// create the CSS and the HTML markup directly rather than use
    /// code from SourceTools.
    /// 
    /// The technical problem is that SourceTools currently brings
    /// in a lot of extraneous CSS that may conflict with CSS that
    /// is already installed on the page.
    /// 
    /// In the new design, the markup is embedded in a pre-tag and
    /// that tag is given CSS class 'fileview_pretag' which is not
    /// likely to conflict with CSS installed on the page directly.
    /// The CSS styles for the a-tag within the pre-tag are defined
    /// relative to the parent CSS class 'fileview_pretag'.
    /// 
    /// 
    /// We now describe the public attributes.
    /// 
    /// 
    /// TildeFilePath
    /// 
    /// This parameter provides the tilde path of the file that will
    /// have a link to view its source.
    /// 
    /// If the tilde path is omitted then it defaults to be the tilde
    /// path of the page object that contains the control.
    /// 
    /// 
    /// MarkupText
    /// 
    /// This parameter provides the option of specifying the link to
    /// view the file with more readable text than the tilde path of
    /// the file.
    /// 
    /// If this parameter is not provided then:
    /// 
    /// * If TildeFilePath is explicitly provided then MarkupText is
    ///   set as "View " + TildeFilePath
    /// 
    /// * Otherwise, TildeFilePath is computed as the tilde path of
    ///   the Page and Markup Text is set to "View Source".
    /// 
    /// 
    /// The next attributes are true/false.
    /// 
    /// 
    /// HR_Before
    /// 
    /// If true then insert a horizontal rule before the markup.
    /// 
    /// 
    /// HR_After
    /// 
    /// If true then insert a horizontal rule after the markup.
    /// 
    /// 
    /// The following attributes are read-only and provide access to
    /// the CSS and its installation key for use by other tools in
    /// app_code in the future.
    /// 
    /// FileViewCSS
    /// 
    /// FileViewKey
    /// </summary>
    public class FileView : WebControl
    {

private static string _FileViewKey = "FileViewKey";

private static string _FileViewCSS =
@"
<style type='text/css'>
.fileview_pretag
{
    color: #00f;
    font-weight: bold;
    font-family: 'Consolas', 'Lucida Console', 'Courier New', 'Andale Mono', monospace;
}

.fileview_pretag a
{
    color: #08f;
    text-decoration: none;
}

.fileview_pretag a:visited
{
    color: #88f;
}

.fileview_pretag a:hover
{
    color: #f00;
}
</style>
";

        public static string FileViewKey { get { return _FileViewKey; } }

        public static string FileViewCSS { get { return _FileViewCSS; } }


        public string TildeFilePath { get; set; }

        public string MarkupText { get; set; }

        public bool HR_Before { get; set; }

        public bool HR_After { get; set; }


        private static string FileViewTildePath = "~/fileview/Default.aspx";


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            CSS.LoadScriptString(Page, FileViewKey, FileViewCSS, "", false);
        }


        protected override void Render(HtmlTextWriter output)
        {
            if (StringTools.IsTrivial(TildeFilePath))
            {
                TildeFilePath = FileTools.GetTildePathWithFileName(Page);

                if (StringTools.IsTrivial(MarkupText))
                {
                    MarkupText = "View Source";
                }
            }

            if (StringTools.IsTrivial(MarkupText))
            {
                MarkupText = "View " + TildeFilePath;
            }

            string FileViewURL =
                FileTools.GetRelativePath(Page, FileViewTildePath);

            output.Write("\n");

            if (HR_Before)
                output.Write("<hr />\n");

            output.Write("<pre class='fileview_pretag'>");
            output.Write("<a href='");
            output.Write(FileViewURL);
            output.Write("?");
            output.Write(TildeFilePath);
            output.Write("' target='_blank'>");
            output.Write(MarkupText);
            output.Write("</a>");
            output.Write("</pre>");

            if (HR_After)
                output.Write("\n<hr />");

            output.Write("\n");
        }
    }
}