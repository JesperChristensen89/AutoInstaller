using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoInstaller
{
    public partial class ChooseSoftwareForm : Form
    {
        // Class variables
        List<ApplicationFactory> software;

        // Events
        public event EventHandler<List<ApplicationFactory>> SoftwareListGenerated;
        
        public ChooseSoftwareForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Inits GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseSoftwareForm_Load(object sender, EventArgs e)
        {
            // Gets list
            software = SoftwareList.CreateSoftwareList(software);

            // GUI stuff
            foreach (var application in software)
            {
                checkedListBoxSoftware.Items.Add(application.Name, true);
            }
        }

        /// <summary>
        /// Clears all items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClear_Click(object sender, EventArgs e)
        {
            checkedListBoxSoftware.Items.Clear();
            foreach (var application in software)
            {
                checkedListBoxSoftware.Items.Add(application.Name, false);
            }
        }

        /// <summary>
        /// Generates software list for program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Checks which apps are checked
            int index = 0;
            foreach (var application in software)
            {
                application.IsChoosen = checkedListBoxSoftware.GetItemChecked(index);
                index++;
            }

            // Close form
            Close();

            // Raising event to pass software list
            SoftwareListGeneratedEvent(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the SoftwareListGenerated event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void SoftwareListGeneratedEvent(EventArgs e)
        {
            if (SoftwareListGenerated != null)
            {
                SoftwareListGenerated(this, software);
            }
        }
    }
}
