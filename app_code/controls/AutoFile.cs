/*
    File: ~/app_code/controls/AutoFile.cs
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.Web;
using System.Web.UI.HtmlControls;

using edu.neu.ccis.rasala;

namespace edu.neu.ccis.rasala.controls
{

    /// <summary>
    /// A control that extends HtmlAnchor
    /// to provide a link to the handler
    /// autofile.aspx at the directory of
    /// the page that contains the control.
    /// 
    /// Anchor uses target="_blank".
    /// </summary>
    public class AutoFile : HtmlAnchor
    {
        public AutoFile()
        {
            HttpContext context = HttpContext.Current;

            string name = AutoFileTools.autoFileName;

            string title = AutoFileTools.autoFileTitle;

            InnerText = title;

            HRef = HttpContextTools.GetWebDirectoryPath(context) + name;

            Target = "_blank";

            Title = "Link to the " + title + " utility: " + name;
        }
    }
}