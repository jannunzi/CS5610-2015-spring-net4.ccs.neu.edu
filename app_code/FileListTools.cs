/*
    File: ~/app_code/FileListTools.cs
    
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
using System.Xml;

namespace edu.neu.ccis.rasala
{
    // Some tools are based on tools in FileTools and SourceTools
    // Some tools are new
    public class FileListTools
    {
        /// <summary>
        /// First converts tildeDirectoryPath to lowercase.
        /// 
        /// Then uses the server to convert tildeDirectoryPath
        /// to an absolute base directory path.
        /// 
        /// Then produces the list of absolute directory paths
        /// whose tree roots itself at this base path.
        /// 
        /// If there is an error then may return an empty list.
        /// </summary>
        public static List<string> AbsoluteDirectoryPathList
            (HttpServerUtility server, string tildeDirectoryPath)
        {
            List<string> list = new List<string>();

            string path = tildeDirectoryPath.ToLower();

            if (StringTools.IsTrivial(path))
                goto returnstatement;

            if (path[0] != '~')
                goto returnstatement;

            int n = path.Length;

            if (path[n - 1] != '/')
                path = path + '/';

            if (!SourceTools.OKtoServe(path, true))
                goto returnstatement;

            try
            {
                string directoryPath = server.MapPath(path);
                AbsoluteDirectoryPathListHelper(list, directoryPath);
            }
            catch { }


            returnstatement:

            return list;
        }


        /// <summary>
        /// Helper method to perform recursion to populate list.
        /// 
        /// The directory path is an absolute path.
        /// </summary>
        private static void AbsoluteDirectoryPathListHelper
            (List<string> list, string directoryPath)
        {
            try
            {
                // If the next 2 steps do not throw an exception
                // then we are fine to proceed
                DirectoryInfo info = new DirectoryInfo(directoryPath);
                DirectoryInfo[] dirs = info.GetDirectories();

                // Put directoryPath into the list
                list.Add(directoryPath);

                // Do recursion on the subdirectories
                foreach (DirectoryInfo subdir in dirs)
                {
                    string name = subdir.Name;

                    if (!SourceTools.IsPublic(name))
                        continue;

                    string path = subdir.FullName;

                    int n = path.Length;

                    if (path[n - 1] != '\\')
                        path += '\\';

                    AbsoluteDirectoryPathListHelper(list, path);
                }
            }
            catch { }
        }


        /// <summary>
        /// First converts tildeDirectoryPath to lowercase.
        /// 
        /// Then produces the list of tilde directory paths
        /// whose tree roots itself at this base path.
        /// 
        /// If there is an error then may return an empty list.
        /// </summary>
        public static List<string> TildeDirectoryPathList
            (HttpServerUtility server, string tildeDirectoryPath)
        {
            List<string> list = AbsoluteDirectoryPathList(server, tildeDirectoryPath);

            if (list.Count == 0)
                return list;

            string root = server.MapPath("~/");

            return FileTools.GetTildePaths(root, list);
        }


        /// <summary>
        /// Uses TildeDirectoryPathList to obtain the list of
        /// tilde directory paths rooted at tildeDirectoryPath.
        /// 
        /// Traverses the files in each of these directories.
        /// 
        /// Uses DateTimeExtensions.FromYMDHMS to convert both
        /// startdate and enddate to DateTime objects that will
        /// be used to filter the above files to those within
        /// the give time frame.
        /// 
        /// Then produces a list of TildePathInfo objects after
        /// the above filtering.
        /// 
        /// If there is an error then may return an empty list.
        /// </summary>
        public static List<TildePathInfo> TildeFilePathList
            (HttpServerUtility server,
            string tildeDirectoryPath,
            string startdate,
            string enddate)
        {
            List<TildePathInfo> filepathlist = new List<TildePathInfo>();

            List<string> pathlist =
                TildeDirectoryPathList(server, tildeDirectoryPath);

            DateTime start = DateTimeExtensions.FromYMDHMS(startdate);
            DateTime end = DateTimeExtensions.FromYMDHMS(enddate);

            foreach (string path in pathlist)
            {
                TildeFilePathListHelper(server, path, filepathlist, start, end);
            }

            return filepathlist;
        }


        private static void TildeFilePathListHelper
            (HttpServerUtility server,
            string tildeDirectoryPath,
            List<TildePathInfo> filepathlist,
            DateTime start,
            DateTime end)
        {
            string directoryPath = server.MapPath(tildeDirectoryPath);

            DirectoryInfo info = new DirectoryInfo(directoryPath);
            FileInfo[] files = info.GetFiles();

            foreach (FileInfo f in files)
            {
                string tildeFilePath = tildeDirectoryPath + f.Name;
                TildePathInfo pathinfo = new TildePathInfo(server, tildeFilePath);
                DateTime date = pathinfo.FileDate;

                bool ok1 = start.CompareTo(date) <= 0;
                bool ok2 = end.CompareTo(date) >= 0;

                if (ok1 && ok2)
                    filepathlist.Add(pathinfo);
            }
        }


        /// <summary>
        /// Uses TildeDirectoryPathList to obtain the list of
        /// tilde directory paths rooted at tildeDirectoryPath.
        /// 
        /// Traverses the files in each of these directories.
        /// 
        /// Uses DateTimeExtensions.FromYMDHMS to convert both
        /// startdate and enddate to DateTime objects that will
        /// be used to filter the above files to those within
        /// the give time frame.
        /// 
        /// Then produces a list of TildePathInfo objects after
        /// the above filtering.
        /// 
        /// Finally sorts the TildePathInfo objects using the
        /// compare_mode which should be "alpha", "size", or
        /// "date".
        /// 
        /// If there is an error then may return an empty list.
        /// </summary>
        public static List<TildePathInfo> TildeFilePathList
            (HttpServerUtility server,
            string tildeDirectoryPath,
            string startdate,
            string enddate,
            string compare_mode)
        {
            List<TildePathInfo> tildefilepathlist =
                TildeFilePathList(server, tildeDirectoryPath, startdate, enddate);

            compare_mode = compare_mode.ToLower();

            if (compare_mode != "alpha")
                tildefilepathlist.Sort(new TildePathInfoComparer(compare_mode));
            
            return tildefilepathlist;
        }
    }
}