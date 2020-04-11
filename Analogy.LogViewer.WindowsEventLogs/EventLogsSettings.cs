using System;
using System.IO;
using System.Windows.Forms;
using Analogy.LogViewer.WindowsEventLogs.Managers;

namespace Analogy.LogViewer.WindowsEventLogs
{
    public partial class EventLogsSettings : UserControl
    {
        public EventLogsSettings()
        {
            InitializeComponent();
        }

        private void GitRepositoriesSettings_Load(object sender, EventArgs e)
        {
          
        }

      
        private void btnAdd_Click(object sender, EventArgs e)
        {
        
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            })
            {
                // Show the FolderBrowserDialog.  
                DialogResult result = folderDlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtRepository.Text = folderDlg.SelectedPath;
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
           
        }
    }
}
