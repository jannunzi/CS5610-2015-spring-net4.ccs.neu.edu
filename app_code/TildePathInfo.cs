/*
    File: ~/app_code/TildePathInfo.cs
    
    Copyright 2013,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;

namespace edu.neu.ccis.rasala
{
    public class TildePathInfo
    {
        // tilde file path
        private string _path = "";

        // file size
        private long _size = 0;

        // most recent date
        private DateTime _date = new DateTime(0);

        // image width if applicable
        private int _width = 0;

        // image height if applicable
        private int _height = 0;

        // is this data valid?
        private bool _valid = false;


        public string TildeFilePath
        {
            get { return _path; }
        }

        public long FileSize
        {
            get { return _size; }
        }

        public DateTime FileDate
        {
            get { return _date; }
        }

        // Note: Returns 0 if file is not an image
        public int ImageWidth
        {
            get { return _width; }
        }

        // Note: Returns 0 if file is not an image
        public int ImageHeight
        {
            get { return _height; }
        }

        public bool Valid
        {
            get { return _valid; }
        }


        public TildePathInfo(HttpServerUtility server, string tildeFilePath)
        {
            if (server == null)
                return;

            if (StringTools.IsTrivial(tildeFilePath))
                return;

            CommonCode(server, tildeFilePath);
        }

        public TildePathInfo(Page page, string tildeFilePath)
        {
            if (page == null)
                return;

            if (StringTools.IsTrivial(tildeFilePath))
                return;

            CommonCode(page.Server, tildeFilePath);
        }

        public TildePathInfo(HttpContext context, string tildeFilePath)
        {
            if (context == null)
                return;

            if (StringTools.IsTrivial(tildeFilePath))
                return;

            CommonCode(context.Server, tildeFilePath);
        }


        private void CommonCode(HttpServerUtility server, string tildeFilePath)
        {
            _path = tildeFilePath;

            if (_path[0] != '~')
                return;

            if (!SourceTools.OKtoServe(tildeFilePath, true))
                return;

            try
            {
                string filePath = server.MapPath(tildeFilePath);
                FileInfo info = new FileInfo(filePath);
                _size = info.Length;
                _date = info.MostRecentTime();
                _valid = true;

                int category = FileTools.GetFileCategory(_path);
                bool image = category == FileTools.IMAGE;

                if (image)
                {
                    using (FileStream stream =
                        new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        try
                        {
                            Bitmap bitmap = new Bitmap(stream);
                            _width = bitmap.Width;
                            _height = bitmap.Height;
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }


        private string errormessage = " The file path is not a valid tilde file path";


        public override string ToString()
        {
            if (_valid)
                return _path;
            else
                return _path + errormessage;
        }


        // ToHtml requires a knowledge of the page on which
        // the html will be installed in order to properly
        // compute relative links.
        public string ToHtml(Page page)
        {
            if (!_valid)
                return "<p><code>" + _path + "</code>" + errormessage + "</p>\n";

            StringBuilder builder = new StringBuilder();

            // Show tilde file path
            // Link to the file view utility if viewable

            int category = FileTools.GetFileCategory(_path);
            bool viewable = category != FileTools.OTHER;

            builder.Append("<p>");

            if (viewable)
            {
                string FileViewTildePath = SourceTools.FileViewTildePath;
                string fileViewURL = FileTools.GetRelativePath(page, FileViewTildePath);

                builder.Append("<a href ='");
                builder.Append(fileViewURL);
                builder.Append("?");
                builder.Append(_path);
                builder.Append("' target='_blank'>");
            }

            builder.Append("<code>");
            builder.Append(_path);
            builder.Append("</code>");

            if (viewable)
            {
                builder.Append("</a>");
            }

            builder.Append("</p>\n");

            // Add launch link if servable

            bool servable = HttpContextTools.IsServable(_path);

            if (servable)
            {
                string launchURL = FileTools.GetRelativePath(page, _path);

                builder.Append("<p>Launch ");
                builder.Append("<a href ='");
                builder.Append(launchURL);
                builder.Append("' target='_blank'>");
                builder.Append("<code>");
                builder.Append(_path);
                builder.Append("</code>");
                builder.Append("</a>");
                builder.Append("</p>\n");
            }

            builder.Append("<table class='filedata'>\n");

            // File size

            builder.Append("<tr>\n");

            builder.Append("<td>");
            builder.Append("Size");
            builder.Append("</td>\n");

            builder.Append("<td>");
            builder.Append(_size.ToString());
            builder.Append("</td>\n");

            builder.Append("</tr>\n");

            // File date

            builder.Append("<tr>\n");

            builder.Append("<td>");
            builder.Append("Date");
            builder.Append("</td>\n");

            builder.Append("<td>");
            builder.Append(_date.ToYMD());
            builder.Append("</td>\n");

            builder.Append("<td>");
            builder.Append(_date.ToHMS());
            builder.Append("</td>\n");

            builder.Append("</tr>\n");

            // Add image dimensions if applicable

            bool image = (_width > 0) && (_height > 0);

            if (image)
            {
                builder.Append("<tr>\n");

                builder.Append("<td>");
                builder.Append("Dims");
                builder.Append("</td>\n");

                builder.Append("<td>");
                builder.Append(_width.ToString());
                builder.Append(" ");
                builder.Append("&times;");
                builder.Append(" ");
                builder.Append(_height.ToString());
                builder.Append("</td>\n");

                builder.Append("</tr>\n");
            }

            builder.Append("</table>\n");

            return builder.ToString();
        }
    }


    /// <summary>
    /// Compare modes are "alpha", "size", "date".
    /// 
    /// The default is "alpha" if anything else is passed to
    /// the constructor.
    /// </summary>
    public class TildePathInfoComparer : IComparer<TildePathInfo>
    {
        private string _compare_mode = "alpha";


        public TildePathInfoComparer() { }


        public TildePathInfoComparer(string compare_mode)
        {
            if (StringTools.IsTrivial(compare_mode))
                return;

            compare_mode = compare_mode.ToLower();

            if (compare_mode == "size")
                _compare_mode = "size";

            if (compare_mode == "date")
                _compare_mode = "date";
        }


        // Implementation of IComparer<TildePathInfo>
        public Int32 Compare(TildePathInfo p1, TildePathInfo p2)
        {
            if (_compare_mode == "size")
            {
                return p1.FileSize.CompareTo(p2.FileSize);
            }

            if (_compare_mode == "date")
            {
                return p1.FileDate.CompareTo(p2.FileDate);
            }

            return p1.TildeFilePath.CompareTo(p2.TildeFilePath);
        }
    }
}