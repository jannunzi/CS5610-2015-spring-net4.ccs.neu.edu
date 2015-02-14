using System;
using System.IO;
using System.Web;

namespace edu.neu.ccis.rasala
{
    /// Adapted from code by Peter Douglass
    ///
    /// This is the code behind the HTTP handler accessed as:
    /// 
    ///     download.aspx?tilde-file-path
    /// 
    /// Here tilde-file-path is a path beginning with ~/ and
    /// given relative to the top level of the web site.
    /// 
    /// See ~/web.config for how this handler is activated
    /// for both Visual Studio and the IIS 7 server.
    /// 
    /// 
    /// 5/5/2009.  Added tests.
    /// 
    /// 1. Is a tilde path url provided to this service?
    /// 2. Is the tilde path url OK to serve.
    /// 
    /// See SourceTools for the code used for #2.
    /// 
    /// This plugs a security hole that would permit a file
    /// to be downloaded even if that file is not allowed to
    /// be seen in sources server.
    /// 
    /// 
    /// 5/8/2009.  Fixed download filename bug.
    /// 
    /// The download process is supported by an HTTP header
    /// tag:
    /// 
    ///     "content-disposition"
    /// 
    /// with value:
    /// 
    ///     "attachment; filename=" + ...
    /// 
    /// The tag is widely supported in browsers to permit
    /// file downloads but is not a W3C standard.  Hence,
    /// implementation of the download behavior varies.
    /// 
    /// The implementation in IE and Firefox is best.  If
    /// the "content-disposition" header is present then
    /// the "content-type" header is ignored.  This makes
    /// sense since the browser is transmitting the file
    /// to the OS rather than using it.
    /// 
    /// However, Chrome, Opera, and Safari behave in a
    /// more proactive manner that causes problems.
    /// 
    /// In ASP.NET, the Response object sets a default
    /// header for
    /// 
    ///     "content-type"
    /// 
    /// with value
    /// 
    ///     "text/html"
    /// 
    /// The proactive browsers use this information to
    /// alter the filename provided by the download
    /// operation to either append ".htm" or ".html"
    /// to the filename or to replace the given file
    /// extension with one of these extensions.
    /// 
    /// This of course changes the way the file is
    /// perceived by the OS and is not a good thing.
    /// 
    /// The solution is to explicity set
    /// 
    ///     "content-type"
    /// 
    /// to
    /// 
    ///     "application/octet-stream"
    /// 
    /// This tells the browser that the file is to be
    /// treated as binary data and to leave the given
    /// filename as is.
    /// 
    /// 5/11/2009.  Amplified the above documentation.
    /// 
    /// 6/25/2011.
    /// 
    /// The query string should now consist only of the
    /// url of the file to be downloaded given as a
    /// tilde path.
    /// 
    /// Previously this url was obtained via:
    /// 
    ///   url = context.Request.QueryString.Get(0);
    ///   
    /// and the tilde path was prefixed with "url=".
    /// 
    /// Now this url is obtained using RequestTools:
    /// 
    ///   url = RequestTools.Query(context.Request);
    ///
    /// The prefix "url=" is no longer needed but if
    /// present will be deleted.
    public class Downloader : IHttpHandler
    {
        const string errorpage = "~/error/DownloadError.aspx";


        public void ProcessRequest(HttpContext context)
        {
            string url = null;

            try
            {
                url = RequestTools.Query(context.Request);

                if (url.StartsWith("url="))
                {
                    url = url.Substring(4);
                }
            }
            catch (Exception) { }

            if (StringTools.IsTrivial(url))
                context.Response.Redirect(errorpage);

            if (!SourceTools.OKtoServeFile(context.Server, url, true))
                context.Response.Redirect(errorpage + "?" + url);

            string filePath = context.Server.MapPath(url);
            var fileInfo = new FileInfo(filePath);

            context.Response.AppendHeader
                ("content-type", "application/octet-stream");
            
            context.Response.AppendHeader
                ("content-disposition", "attachment; filename=" + fileInfo.Name);
            
            context.Response.TransmitFile(filePath);
        }

        public bool IsReusable { get { return false; } }
    }
}