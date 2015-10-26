using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AutoInstaller
{
    public partial class ApplicationForm : Form
    {
        // Class variables
        List<ApplicationFactory> m_software;
        Download m_download;

        public ApplicationForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initiates form
        /// </summary>
        private void ApplicationForm_Load(object sender, EventArgs e)
        {
            //GUI stuff
            buttonBegin.Enabled = false;

            // set FormClosing event
            FormClosing += new FormClosingEventHandler(ApplicationFormClosing);
        }

        /// <summary>
        /// Closing routine that deletes downloaded installation files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationFormClosing(object sender, FormClosingEventArgs e)
        {
            // Do only if softwarelist has been generated
            if (m_software!=null)
            {

                // Ask user if the wants to delete install files
                DialogResult deleteFiles = MessageBox.Show("Do you want to delete install files?", "Auto Installer",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (deleteFiles == DialogResult.Yes)
                {
                    // delete install file
                    foreach (var application in m_software)
                    {
                        var fileName = application.Name + ".exe";

                        try
                        {
                            File.Delete(fileName);
                        }
                        // if currently downloading - stop download and delete
                        catch
                        {
                            m_download.CancelDownload();

                            // wait cancel to complete
                            Thread.Sleep(50);
                            File.Delete(fileName);
                        }
                    }
                }
            }
            // exit
        }

        /// <summary>
        /// Starts download and installation queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBegin_Click(object sender, EventArgs e)
        {
            // GUI stuff
            buttonBegin.Enabled = false;

            // Start download queue
            m_download = new Download(m_software, listViewStatus);
            m_download.StartDownloadQueue();
        }

        /// <summary>
        /// Opens up choose software window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChooseSoftware_Click(object sender, EventArgs e)
        {
            // Clear list in case it has been generated before
            listViewStatus.Items.Clear();

            // Generate instance of SoftwareForm
            var chooseSoftwareForm = new ChooseSoftwareForm();

            // Set events
            chooseSoftwareForm.SoftwareListGenerated += new EventHandler<List<ApplicationFactory>>(SoftwareListUpdated);

            // Open form
            chooseSoftwareForm.Show();
        }

        private void MyEventHandler(object sender, List<ApplicationFactory> e)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// Eventhandler when OK is pressed in ChooseSoftwareForm that updates software to install
        /// </summary>
        /// <param name="software"></param>
        private void SoftwareListUpdated(object sender, List<ApplicationFactory> software)
        {
            // set class member variable
            m_software = software;

            // Update GUI
            buttonBegin.Enabled = true;

            int applicationIndex = 0;
            foreach (var application in m_software)
            {
                if (application.IsChoosen)
                {
                    var listViewItem = new ListViewItem(new string[] { application.Name, "Waiting", "" });
                    listViewStatus.Items.Add(listViewItem);
                    applicationIndex++;
                }
            }
        }
    }
}
