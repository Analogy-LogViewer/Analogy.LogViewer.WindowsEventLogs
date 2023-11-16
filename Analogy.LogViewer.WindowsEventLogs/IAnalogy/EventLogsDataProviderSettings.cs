using Analogy.Interfaces;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.WindowsEventLogs.Managers;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class EventLogsDataProviderSettings : TemplateUserSettingsFactory
    {
        public override string Title { get; set; } = "Windows Event Logs settings";
        public override UserControl DataProviderSettings { get; set; } 

        public override Guid FactoryId { get; set; } = EventLogPrimaryFactory.id;
        public override Guid Id { get; set; } = new Guid("61774F7C-4F62-4A61-AD24-FC8263DF518A");

        public override void CreateUserControl(ILogger logger)
        {
            DataProviderSettings = new EventLogsSettings();
        }

        public override Task SaveSettingsAsync()
        {
            UserSettingsManager.UserSettings.Save();
            return Task.CompletedTask;
        }
    }
}