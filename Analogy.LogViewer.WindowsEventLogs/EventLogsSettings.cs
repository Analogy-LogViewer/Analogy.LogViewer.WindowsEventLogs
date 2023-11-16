using Analogy.LogViewer.WindowsEventLogs.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Analogy.LogViewer.WindowsEventLogs
{
    public partial class EventLogsSettings : UserControl
    {
        public EventLogsSettings()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<string> selected = lstAvailable.SelectedItems.Cast<string>().ToList();
            lstSelected.Items.AddRange(selected.ToArray());
            foreach (var log in selected)
            {
                lstAvailable.Items.Remove(log);
            }
            UpdateUserSettingList();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            List<string> selected = lstSelected.SelectedItems.Cast<string>().ToList();
            lstAvailable.Items.AddRange(selected.ToArray());
            foreach (var log in selected)
            {
                lstSelected.Items.Remove(log);
            }
            UpdateUserSettingList();
        }
        private void UpdateUserSettingList()
        {
            UserSettingsManager.UserSettings.Logs = lstSelected.Items.OfType<string>().ToList();
        }

        private void EventLogsSettings_Load(object sender, EventArgs e)
        {
            lstSelected.Items.AddRange(UserSettingsManager.UserSettings.Logs.ToArray());
            try
            {
                var all = System.Diagnostics.Eventing.Reader.EventLogSession.GlobalSession.GetLogNames().Where(EventLog.Exists).ToList().Except(UserSettingsManager.UserSettings.Logs).ToArray();
                lstAvailable.Items.AddRange(all);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error loading all logs. Make sure you are running as administrator. Error:" + exception.Message, "Error",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}