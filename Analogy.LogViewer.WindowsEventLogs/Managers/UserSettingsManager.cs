using System;
using System.Collections.Generic;
using System.IO;
using Analogy.LogViewer.Template.Managers;
using Newtonsoft.Json;

namespace Analogy.LogViewer.WindowsEventLogs.Managers
{
    public class UserSettingsManager
    {
        private static readonly Lazy<UserSettingsManager> _instance =
            new Lazy<UserSettingsManager>(() => new UserSettingsManager());
        public static UserSettingsManager UserSettings { get; set; } = _instance.Value;
        private string EventLogSettingFile { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Analogy.LogViewer", "AnalogyWindowsEventLogsSettings.json");
        public List<string> Logs { get; set; }

        public UserSettingsManager()
        {
            Logs=new List<string>();
            if (File.Exists(EventLogSettingFile))
            {
                try
                {
                    string data = File.ReadAllText(EventLogSettingFile);
                    Logs = JsonConvert.DeserializeObject<List<string>>(data);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogCritical("", $"Unable to read file {EventLogSettingFile}: {ex}");
                }
            }
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(EventLogSettingFile, JsonConvert.SerializeObject(Logs));
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogCritical("", $"Unable to save file {EventLogSettingFile}: {ex}");

            }
        }
    }
}
