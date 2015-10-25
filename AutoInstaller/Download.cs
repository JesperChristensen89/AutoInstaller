using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;
using System.ComponentModel;

namespace AutoInstaller
{
    public class Download
    {
        // class variables
        List<ApplicationFactory> m_software;
        ListView m_listViewStatus;
        int prevDownloadPercentage = 0;
        int applicationIndex = 0;
        WebClient webClient;
        bool shuttingDown = false;

        /// <summary>
        /// Constructor for Download class
        /// </summary>
        /// <param name="software"></param>
        public Download(List<ApplicationFactory> software, ListView listViewStatus)
        {
            m_software = software;
            m_listViewStatus = listViewStatus;
        }

        /// <summary>
        /// Starts download queue and activates installation when first download completes
        /// </summary>
        public async void StartDownloadQueue()
        {
            foreach (var application in m_software)
            {
                if(!shuttingDown)
                {
                    // Only download if application is choosen
                    if(application.IsChoosen)
                    {
                        webClient = new WebClient();

                        // Update GUI
                        m_listViewStatus.Items[applicationIndex].SubItems[1].Text = "Downloading";

                        // Setting eventhandlers
                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
                        webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(webClient_DownloadCompleted);

                        // Starting download and wait for it to finish
                        string fileName = application.Name + ".exe";

                        webClient.Credentials = CredentialCache.DefaultNetworkCredentials;

                        try
                        {
                            await webClient.DownloadFileTaskAsync(new Uri(application.Url), fileName);

                            // Updating object
                            application.IsDownloaded = true;

                            // Disposing client
                            webClient.Dispose();

                            // Update application index as known to GUI
                            applicationIndex++;
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cancel current download and dispose client
        /// </summary>
        public virtual void CancelDownload()
        {
            webClient.CancelAsync();
            webClient.Dispose();

            // Setting closing-routine flag
            shuttingDown = true;
        }


        /// <summary>
        /// Updates GUI and starts install queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webClient_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Check if download was cancelled
            if (e.Cancelled)
            {
                return;
            }

            // Update GUI
            m_listViewStatus.Items[applicationIndex].SubItems[1].Text = "Waiting";
            m_listViewStatus.Items[applicationIndex].SubItems[2].Text = "Downloaded";

            // starting install queue once when first application is downloaded
            if (applicationIndex == 0)
            {
                var install = new Install(m_software, m_listViewStatus);
                install.StartInstallQueue();
            }

        }
        
        /// <summary>
        /// Updates GUI with download percentage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (!shuttingDown)
            { 
                if (prevDownloadPercentage != e.ProgressPercentage)
                {
                    string status = e.ProgressPercentage + "%";
                    m_listViewStatus.Items[applicationIndex].SubItems[2].Text = status;

                    prevDownloadPercentage = e.ProgressPercentage;
                }
            }
        }
    }
}
