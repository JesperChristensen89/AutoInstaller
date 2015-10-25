using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInstaller
{
    static class SoftwareList
    {

        /// <summary>
        /// Creates softwarelist from root textfile "software.txt"
        /// </summary>
        /// <param name="software"></param>
        /// <returns></returns>
        public static List<ApplicationFactory> CreateSoftwareList(List<ApplicationFactory> software)
        {
            software = new List<ApplicationFactory>();

            using (StreamReader sr = new StreamReader("software.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] application = line.Split(';');

                    software.Add(new ApplicationFactory(application[0], application[1], false, false));
                }
            }

            return software;
        }
    }
}
