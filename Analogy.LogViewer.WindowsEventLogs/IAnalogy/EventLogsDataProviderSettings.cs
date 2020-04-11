using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.DataProviders.Extensions;
using Analogy.LogViewer.WindowsEventLogs.Managers;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
   public class EventLogsDataProviderSettings :IAnalogyDataProviderSettings
   {

       public string Title { get; } = "Windows Event Logs settings";
        public UserControl DataProviderSettings { get; }=new EventLogsSettings();
        public Image SmallImage { get; }
        public Image LargeImage { get; }
        public Guid FactoryId { get; set; } = EventLogDataFactory.id;
        public Guid ID { get; set; } = new Guid("61774F7C-4F62-4A61-AD24-FC8263DF518A");

        public Task SaveSettingsAsync()
        {
            UserSettingsManager.UserSettings.Save();
            return Task.CompletedTask;
        }
    }
}
