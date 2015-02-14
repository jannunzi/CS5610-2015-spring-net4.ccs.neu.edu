/*
    File: ~/app_code/handlers/AutoFileHandler.cs
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.Web;

namespace edu.neu.ccis.rasala
{

    /*
     * IHttpHandler to process pseudo file: autofile.aspx
     * 
     * See ~/web.config for how this handler is activated
     * for both Visual Studio 2010 and the IIS 7 server.
     */
    public class AutoFileHandler : IHttpHandler
    {
        public const bool onlyPublic = true;


        public void ProcessRequest(HttpContext context)
        {
            string title = AutoFileTools.autoFileTitle;

            string headMarkup =
                HttpContextTools.MakeStyleSheetLinks(context)
                +
                HttpContextTools.MakeJavascriptReferences(context);

            string bodyMarkup =
                AutoFileTools.MakeAutoFileMarkup(context, onlyPublic);

            string markup =
                HTML_Tools.MakePageMarkup
                    (null, title, headMarkup, null, bodyMarkup);

            context.Response.ContentType =
                HTML_Tools.htmlContentType;

            context.Response.Write(markup);
        }


        public bool IsReusable { get { return false; } }
    }
}
