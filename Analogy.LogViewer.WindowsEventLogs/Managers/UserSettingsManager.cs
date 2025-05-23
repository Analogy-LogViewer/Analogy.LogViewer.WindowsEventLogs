﻿using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

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
                    Logs = System.Text.Json.JsonSerializer.Deserialize<List<string>>(data);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.LogCritical(ex, $"Unable to read file {EventLogSettingFile}: {ex}");
                }
            }
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(EventLogSettingFile, System.Text.Json.JsonSerializer.Serialize(Logs));
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogCritical("", $"Unable to save file {EventLogSettingFile}: {ex}");
            }
        }
    }
}