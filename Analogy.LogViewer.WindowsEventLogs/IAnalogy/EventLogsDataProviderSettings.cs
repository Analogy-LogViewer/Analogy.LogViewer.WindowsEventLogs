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
        public Guid FactoryId { get; } = EventLogDataFactory.id;

        public Task SaveSettingsAsync()
        {
            UserSettingsManager.UserSettings.Save();
            return Task.CompletedTask;
        }
    }
}
