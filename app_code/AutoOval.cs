using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Drawing;

namespace edu.neu.ccis.rasala
{
    /// <summary>
    /// AutoOval has static methods ImgTag that generate the HTML img
    /// tags for auto generated filled oval images on a white
    /// background.
    /// 
    /// There are also static methods for the ImgURL and the AltText.
    /// 
    /// The parameters are the width w and the height h and color
    /// data given as r, g, b, a or as Color structure.
    /// 
    /// The handler will paint a semi-transparent color over a white
    /// background to produce color mixing.
    /// 
    /// The ImgURL has the form: AutoOval_w_h_r_g_b_a.aspx
    /// 
    /// Using the file extension .aspx is a hack.  A file with this
    /// extension will be directed to ASP.NET for processing and so
    /// can then be diverted to this handler using the httpHandlers
    /// setting in Web.config.
    /// 
    /// We were unable to get IIS to direct a pseudo file of type
    /// .jpg to ASP.NET for processing.
    /// 
    /// </summary>
    public class AutoOval
    {
        private AutoOval() { }

        /// <summary>
        /// Return the img tag for an autobox with the given parameters.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        /// <param name="a">alpha</param>
        public static string ImgTag(int w, int h, int r, int g, int b, int a)
        {
            w = (w < 1) ? 1 : w;
            h = (h < 1) ? 1 : h;

            r = (r < 0) ? 0 : ((r > 255) ? 255 : r);
            g = (g < 0) ? 0 : ((g > 255) ? 255 : g);
            b = (b < 0) ? 0 : ((b > 255) ? 255 : b);
            a = (a < 0) ? 0 : ((a > 255) ? 255 : a);

            string[] data =
            {
                "<img src=\"",
                ImgURL(w, h, r, g, b, a),
                "\" alt=\"",
                AltText(w, h),
                "\" />"
            };

            return StringTools.Build(data);
        }


        /// <summary>
        /// Return the img tag for an autobox with the given parameters
        /// and a=255.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        public static string ImgTag(int w, int h, int r, int g, int b)
        {
            return ImgTag(w, h, r, g, b, 255);
        }


        /// <summary>
        /// Return the img tag for an autobox with the given parameters
        /// and the color black.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        public static string ImgTag(int w, int h)
        {
            return ImgTag(w, h, 0, 0, 0, 255);
        }


        /// <summary>
        /// Return the img tag for an autobox with the given parameters.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="c">the box color</param>
        public static string ImgTag(int w, int h, Color c)
        {
            return ImgTag(w, h, c.R, c.G, c.B, c.A);
        }


        /// <summary>
        /// Return the img URL for an autobox with the given parameters.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        /// <param name="a">alpha</param>
        public static string ImgURL(int w, int h, int r, int g, int b, int a)
        {
            w = (w < 1) ? 1 : w;
            h = (h < 1) ? 1 : h;

            r = (r < 0) ? 0 : ((r > 255) ? 255 : r);
            g = (g < 0) ? 0 : ((g > 255) ? 255 : g);
            b = (b < 0) ? 0 : ((b > 255) ? 255 : b);
            a = (a < 0) ? 0 : ((a > 255) ? 255 : a);

            string[] data =
            {
                "AutoOval_",
                Convert.ToString(w), "_",
                Convert.ToString(h), "_",
                Convert.ToString(r), "_",
                Convert.ToString(g), "_",
                Convert.ToString(b), "_",
                Convert.ToString(a),
                ".aspx"
            };

            return StringTools.Build(data);
        }


        /// <summary>
        /// Return the img URL for an autobox with the given parameters
        /// and a=255.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        public static string ImgURL(int w, int h, int r, int g, int b)
        {
            return ImgURL(w, h, r, g, b, 255);
        }


        /// <summary>
        /// Return the img URL for an autobox with the given parameters
        /// and the color black.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        public static string ImgURL(int w, int h)
        {
            return ImgURL(w, h, 0, 0, 0, 255);
        }


        /// <summary>
        /// Return the img URL for an autobox with the given parameters.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        /// <param name="c">the box color</param>
        public static string ImgURL(int w, int h, Color c)
        {
            return ImgURL(w, h, c.R, c.G, c.B, c.A);
        }


        /// <summary>
        /// Return the alt text for an autobox with the given parameters.
        /// 
        /// The alt text mentions only the width and height so only these
        /// parameters are passed.
        /// </summary>
        /// <param name="w">width</param>
        /// <param name="h">height</param>
        public static string AltText(int w, int h)
        {
            w = (w < 1) ? 1 : w;
            h = (h < 1) ? 1 : h;

            string[] data =
            {
               "AutoOval Image of Size ",
                Convert.ToString(w),
                " by ",
                Convert.ToString(h)
            };

            return StringTools.Build(data);
        }
    }
}