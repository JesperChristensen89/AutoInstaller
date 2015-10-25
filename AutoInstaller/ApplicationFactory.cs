using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoInstaller
{
    public class ApplicationFactory
    {
        // Class variables
        string m_name;
        string m_url;
        bool m_downloaded;
        bool m_choosen;

        /// <summary>
        /// Constructs an application object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        /// <param name="downloaded"></param>
        public ApplicationFactory(string name, string url, bool downloaded, bool choosen)
        {
            m_name = name;
            m_url = url;
            m_downloaded = downloaded;
            m_choosen = choosen;
        }

        /// <summary>
        /// Gets application name
        /// </summary>
        public string Name { get { return m_name; } }

        /// <summary>
        /// Gets application URL
        /// </summary>
        public string Url { get { return m_url; } }

        /// <summary>
        /// Gets whether or not the application is downloaded
        /// </summary>
        public bool IsDownloaded { get { return m_downloaded; } set { m_downloaded = value; } }

        /// <summary>
        /// Gets whether or not the application is choosen
        /// </summary>
        public bool IsChoosen { get { return m_choosen; } set { m_choosen = value; } }

    }
}
