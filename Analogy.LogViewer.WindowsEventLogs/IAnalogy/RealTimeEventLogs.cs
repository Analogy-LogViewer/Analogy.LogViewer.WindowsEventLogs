using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Analogy.Interfaces;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.WindowsEventLogs.Managers;


namespace Analogy.LogViewer.WindowsEventLogs
{
    public class RealTimeEventLogs : OnlineDataProvider
    {
        private List<EventLog> Logs = new List<EventLog>();
        public override Guid Id { get; set; } = new Guid("407C8AD7-E7A3-4B36-9221-BB5D48E78766");
        public override  Image ConnectedLargeImage { get; set; } = null;
        public override  Image ConnectedSmallImage { get; set; } = null;
        public override  Image DisconnectedLargeImage { get; set; } = null;
        public override Image DisconnectedSmallImage { get; set; } = null;
        public override string OptionalTitle { get; set; } = "Real Time Windows Event logs";
        public override Task<bool> CanStartReceiving() => Task.FromResult(true);
        public override IAnalogyOfflineDataProvider FileOperationsHandler { get; set; } = null;
        public override bool UseCustomColors { get; set; } = false;
        public override IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public override (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public override Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            return base.InitializeDataProviderAsync(logger);
        }


        public override Task StartReceiving()
        {
            try
            {
                SetupLogs();
            }
            catch (Exception e)
            {
                LogManager.Instance.LogError(nameof(StartReceiving), $@"Error reading: {e}");
            }
            return Task.CompletedTask;
        }


        public override Task StopReceiving() => Task.CompletedTask;



        private void SetupLogs()
        {
            foreach (EventLog eventLog in Logs)
            {
                eventLog.EnableRaisingEvents = false;
                eventLog.Dispose();
            }
            Logs.Clear();
            Counter all = new Counter("All");
            foreach (string logName in UserSettingsManager.UserSettings.Logs)
            {
                try
                {
                    if (EventLog.Exists(logName))
                    {
                        var eventLog = new EventLog(logName);
                        Logs.Add(eventLog);
                        Counter c = new Counter(logName);
                        // set event handler
                        eventLog.EntryWritten += (apps, arg) =>
                        {
                            all.Increment();
                            c.Increment();

                            AnalogyLogMessage m = CreateMessageFromEvent(arg.Entry);
                            m.Module = logName;
                            MessageReady(this,new AnalogyLogMessageArgs(m, Environment.MachineName, "Windows Event Logs", Id));
                        };

                        eventLog.EnableRaisingEvents = true;
                    }
                }
                catch (Exception e)
                {
                    string m = "Error Opening log. Please make sure you are running as Administrator." + Environment.NewLine + "Error:" + e.Message;
                    MessageBox.Show(m, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AnalogyLogMessage l = new AnalogyLogMessage(m, AnalogyLogLevel.Error, AnalogyLogClass.General, "Analogy", "None");
                    MessageReady(this, new AnalogyLogMessageArgs(l, Environment.MachineName, "Windows Event Logs", Id));
                    LogManager.Instance.LogException($"Error reading event log: {e.Message}",e, "Windows Event Logs");
                }
            }
        }

        private class Counter
        {
            public string Name { get; }
            private int Count { get; set; }

            public Counter(string name)
            {
                Name = name;
                Count = 0;
            }

            public void Increment() => Count++;
            public override string ToString()
            {
                return $"Log: {Name}. Messages: {Count}";
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AnalogyLogMessage CreateMessageFromEvent(EventLogEntry eEntry)
        {
            AnalogyLogMessage m = new AnalogyLogMessage();
            switch (eEntry.EntryType)
            {
                case EventLogEntryType.Error:
                    m.Level = AnalogyLogLevel.Error;
                    break;
                case EventLogEntryType.Warning:
                    m.Level = AnalogyLogLevel.Warning;
                    break;
                case EventLogEntryType.Information:
                    m.Level = AnalogyLogLevel.Information;
                    break;
                case EventLogEntryType.SuccessAudit:
                    m.Level = AnalogyLogLevel.Information;
                    break;
                case EventLogEntryType.FailureAudit:
                    m.Level = AnalogyLogLevel.Error;
                    break;
                default:
                    m.Level = AnalogyLogLevel.Information;
                    break;
            }

            m.Category = eEntry.Category;
            m.Date = eEntry.TimeGenerated;
            m.Id = Guid.NewGuid();
            m.Source = eEntry.Source;
            m.Text = eEntry.Message;
            m.User = eEntry.UserName;
            m.Module = eEntry.Source;
            return m;
        }
    }

}
