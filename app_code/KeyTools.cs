/*
    File: ~/app_code/KeyTools.cs
    
    Copyright 2012,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
 */

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace edu.neu.ccis.rasala
{
    public class KeyTools
    {
        /*
         * The standard directory for the key file: keys
         */
        const string directory = "keys";


        /*
         * Return directory + "\\" + keyidentifier + ".txt"
         * 
         * This is the file name for the key file relative to
         * the root parent.
         */
        public static string KeyFileName(string keyidentifier)
        {
            return directory + "\\" + keyidentifier + ".txt";
        }


        /*
         * Return the key associated with the keyidentifier.
         * 
         * This is the trimmed first line in the file with name
         * KeyFileName.
         * 
         * In particular, additional lines are ignored.
         * 
         * The Page is needed to find the file system path.
         */
        public static string GetKey(Page page, string keyidentifier)
        {
            return GetKey(page.Server, keyidentifier);
        }


        /*
         * Return the key associated with the keyidentifier.
         * 
         * This is the trimmed first line in the file with name
         * KeyFileName.
         * 
         * In particular, additional lines are ignored.
         * 
         * The HttpContext is needed to find the file system path.
         */
        public static string GetKey(HttpContext context, string keyidentifier)
        {
            return GetKey(context.Server, keyidentifier);
        }


        /*
         * Return the key associated with the keyidentifier.
         * 
         * This is the trimmed first line in the file with name
         * KeyFileName.
         * 
         * In particular, additional lines are ignored.
         * 
         * The HttpServerUtility is needed to find the file system path.
         */
        public static string GetKey(HttpServerUtility server, string keyidentifier)
        {
            string rootparent = FileTools.GetRootParent(server);

            string filename = rootparent + KeyFileName(keyidentifier);

            List<string> data = FileTools.FileDataAbsolutePath(filename);

            if (data.Count <= 0)
                return "";
            else
                return data[0];
        }

    }
}