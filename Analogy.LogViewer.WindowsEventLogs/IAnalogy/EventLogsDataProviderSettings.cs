﻿using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.Interfaces;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.WindowsEventLogs.Managers;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class EventLogsDataProviderSettings : UserSettingsFactory
    {

        public override string Title { get; set; } = "Windows Event Logs settings";
        public override UserControl DataProviderSettings { get; set; } = new EventLogsSettings();

        public override Guid FactoryId { get; set; } = EventLogPrimaryFactory.id;
        public override Guid Id { get; set; } = new Guid("61774F7C-4F62-4A61-AD24-FC8263DF518A");

        public override Task SaveSettingsAsync()
        {
            UserSettingsManager.UserSettings.Save();
            return Task.CompletedTask;
        }
    }
}
