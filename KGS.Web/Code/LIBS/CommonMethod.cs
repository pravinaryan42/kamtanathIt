using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace KGS.Web.Code.LIBS
{
    public static class CommonMethod
    {
        #region "WriteLog"

        /// <summary>
        /// Method to write log file
        /// </summary>
        /// <param name="Description"></param>
        public static void WriteLogFile(string Description)
        {
            string path =HostingEnvironment.MapPath(string.Concat(ConfigurationManager.AppSettings["LogFile"]));

           
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);

            // Create a writer and open the file:
            string filename = Path.Combine(path, "LogFile_"+DateTime.Now.ToString("yyyyMMdd")+".txt");
            StreamWriter log;
            if (!File.Exists(filename))
            {
                log = new StreamWriter(filename);
            }
            else
            {
                log = File.AppendText(filename);
            }

            // Write to the file:
            if (Description == "n")
            {
                log.WriteLine("-----------------------------------------------------------------------------------------------");
            }
            else
            {
                log.WriteLine(DateTime.Now.ToString() + "\t:\t" + Description);
            }

            log.WriteLine();

            // Close the stream:
            log.Close();
        }
        #endregion

        #region Validation
        /// <summary>
        /// Validate Email
        /// </summary>
        /// <param name="stremail"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string stremail)
        {
            // Return true if stremail is in valid single or multiple e-mail format.
            return Regex.IsMatch(stremail, @"^(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*(;|,)\s*|\s*$))*$");
        }
        #endregion
    
    }
}
