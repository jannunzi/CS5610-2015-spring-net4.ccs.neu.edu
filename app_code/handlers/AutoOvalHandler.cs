using System;
using System.Web;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using System.IO;

namespace edu.neu.ccis.rasala
{
    /// <summary>
    /// The class is a handler for pseudo file names of the form:
    /// 
    /// AutoOval_w_h_r_g_b_a.aspx
    /// 
    /// Using the file extension .aspx is a hack.  A file with this
    /// extension will be directed to ASP.NET for processing and so
    /// can then be diverted to this handler using the httpHandlers
    /// setting in Web.config.
    /// 
    /// We were unable to get IIS to direct a pseudo file of type
    /// .jpg to ASP.NET for processing.
    /// 
    /// Note that the web directory of the request will be part of
    /// the file path.
    /// 
    /// The handler will use the following defaults if parameters in
    /// the filename are not supplied:
    /// 
    ///   w = 10
    ///   h = 10
    ///   r = 0
    ///   g = 0
    ///   b = 0
    ///   a = 255 (opaque)
    /// 
    /// The handler will paint a semi-transparent color over a white
    /// background to produce color mixing.  Experiment has shown
    /// that this is the best approximation to use when a is less
    /// than 255.
    /// 
    /// In 2008, at the suggestion of Peter Douglass, an alternative
    /// way to invoke this handler using query strings was introduced.
    /// Specifically, a path of the form:
    /// 
    /// AutoOval.aspx?w=...&h=...&r=...&g=...&b=...&a=...
    /// 
    /// will be accepted.  Here ... stands for the numeric parameters.
    /// As usual for query strings, the parameters may be given in
    /// any order.  Omitted parameters will use the defaults given
    /// above.
    /// 
    /// The original format is still supported for backwards
    /// compatibility.
    /// </summary>
    public class AutoOvalHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // File path has the form: .../AutoOval_w_h_r_g_b_a.aspx
            //
            // The web directory of the request will be part of the
            // file path
            string path = context.Request.FilePath;


            // debug code to test that handler is entered
            /*
            string rootparent = FileTools.GetRootParent(context.Server);

            string logfilename = rootparent + "logdata.txt";

            using (StreamWriter writer = new StreamWriter(logfilename, true))
            {
                writer.WriteLine(path);
            }
            */


            // eliminate the extra characters in the file path
            int spot;
            string name;

            // remove the web directory from the front of file path
            spot = path.LastIndexOf('/');
            name = (spot >= 0) ? path.Substring(spot + 1) : path;

            // remove the file extension from the back of file name
            spot = name.LastIndexOf('.');
            name = (spot >= 0) ? name.Substring(0, spot) : name;

            //split on the underscore character in name
            string[] split = name.Split('_');
            int N = split.Length;

            // default values
            // 10 by 10 box in opaque black
            int w = 10;
            int h = 10;
            int r = 0;
            int g = 0;
            int b = 0;
            int a = 255;

            // parse what parameters are available
            if (N > 1)
                w = parse(split[1], 1, Int32.MaxValue, w);

            if (N > 2)
                h = parse(split[2], 1, Int32.MaxValue, h);

            if (N > 3)
                r = parse(split[3], 0, 255, r);

            if (N > 4)
                g = parse(split[4], 0, 255, g);

            if (N > 5)
                b = parse(split[5], 0, 255, b);

            if (N > 6)
                a = parse(split[6], 0, 255, a);

            // check for information passed via a query string
            var querydata = context.Request.QueryString;

            w = parse(querydata.Get("w"), 1, Int32.MaxValue, w);
            h = parse(querydata.Get("h"), 1, Int32.MaxValue, h);
            r = parse(querydata.Get("r"), 0, 255, r);
            g = parse(querydata.Get("g"), 0, 255, g);
            b = parse(querydata.Get("b"), 0, 255, b);
            a = parse(querydata.Get("a"), 0, 255, a);

            // set the response type to image/jpeg
            context.Response.ContentType = "image/jpeg";

            // create bitmap, fill with color, and save to response stream
            Color color = Color.FromArgb(a, r, g, b);

            Bitmap bitmap = new Bitmap(w, h);

            Graphics graphics = Graphics.FromImage(bitmap);

            // first fill with white then overfill with color
            graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, w, h);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            graphics.FillEllipse(new SolidBrush(color), 0, 0, w, h);

            bitmap.Save(context.Response.OutputStream, ImageFormat.Jpeg);


            // debug code to test that handler reaches this spot
            /*
            using (StreamWriter writer = new StreamWriter(logfilename, true))
            {
                writer.WriteLine("At end");
            }
            */


            // Here is the documentation for context.Response.End()
            //
            // Sends all currently buffered output to the client,
            // stops execution of the page,
            // and raises the EndRequest event.
            //
            // Hence, if used, this must be the last line in the
            // method since no further lines of code will be
            // executed.
            //
            // It turns out that this call is not needed so it is
            // commented out.
            //
            // context.Response.End();
        }


        private int parse(string data, int min, int max, int def)
        {
            int val = def;

            try
            {
                if (data != null)
                    val = Convert.ToInt32(data);
            }
            catch (Exception) { }

            val = (val < min) ? min : ((val > max) ? max : val);

            return val;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}