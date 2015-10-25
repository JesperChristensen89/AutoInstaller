using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoInstaller
{
    public class Install
    {
        // class variables
        List<ApplicationFactory> m_software;
        ListView m_listViewStatus;
        int applicationIndex = 0;

        /// <summary>
        /// Install class constructor
        /// </summary>
        /// <param name="software"></param>
        /// <param name="listViewStatus"></param>
        public Install(List<ApplicationFactory> software, ListView listViewStatus)
        {
            m_software = software;
            m_listViewStatus = listViewStatus;
        }

        /// <summary>
        /// Starting install process queue 
        /// </summary>
        /// <returns></returns>
        public Task StartInstallQueue()
        {
            // Creating process to run in background
            return Task.Run(() =>
            {
                using (var process = new Process())
                {
                    foreach (var application in m_software)
                    {
                        // Only install choosen items
                        if (application.IsChoosen)
                        {
                            // wait untill application is downloaded
                            while (!application.IsDownloaded) ;

                            // Update GUI
                            m_listViewStatus.Invoke((MethodInvoker)(() =>
                                m_listViewStatus.Items[applicationIndex].SubItems[1].Text = "Installing"));

                            m_listViewStatus.Invoke((MethodInvoker)(() =>
                                m_listViewStatus.Items[applicationIndex].SubItems[2].Text = ""));

                            // run process and wait for it to close
                            var fileName = application.Name + ".exe";

                            process.StartInfo.FileName = fileName;
                            process.Start();
                            process.WaitForExit();

                            // Update GUI
                            m_listViewStatus.Invoke((MethodInvoker)(() =>
                                m_listViewStatus.Items[applicationIndex].SubItems[1].Text = "Installed"));

                            // Update application index as known to GUI
                            applicationIndex++;
                        }
                    }
                }
            });
        }

    }
}
