using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.LogViewer.Template.Managers;
using Analogy.LogViewer.WindowsEventLogs.Managers;
using Microsoft.Extensions.Logging;

namespace Analogy.LogViewer.WindowsEventLogs
{
    public class EventViewerLogLoader
    {
        private readonly string[] _separators = {"Severity: ", ", Category: ", ", MessageID: ", ", Message: "};
        private CancellationToken Token { get; }

        public EventViewerLogLoader(CancellationToken token)
        {
            Token = token;
        }

        public async Task<IEnumerable<IAnalogyLogMessage>> ReadFromFile(string fileName, ILogMessageCreatedHandler messagesHandler)
        {
            if (!File.Exists(fileName))
            {
                await Task.CompletedTask;
                return new List<AnalogyLogMessage>();
            }

            List<IAnalogyLogMessage> messages = new List<IAnalogyLogMessage>();
            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var reader = new EventLogReader(fileName, PathType.FilePath))
                    {
                        EventRecord record;
                        long count = 0;
                        while ((record = reader.ReadEvent()) != null)
                        {
                            if (Token.IsCancellationRequested)
                            {
                                break;
                            }

                            using (record)
                            {
                                AnalogyLogMessage m = new AnalogyLogMessage
                                {
                                    Date = record.TimeCreated ?? DateTime.MinValue,
                                    Source = record.ProviderName,
                                    Module = record.ProviderName,
                                    Level = AnalogyLogLevel.Information,
                                    Id = record.ActivityId ?? Guid.Empty,
                                    ProcessId = record.ProcessId ?? 0,
                                    MachineName= record.MachineName,
                                    ThreadId =record.ThreadId??0,
                                    FileName = fileName,
                                    User = record.UserId?.Value
                                };
                                string properties = string.Join(Environment.NewLine,
                                    record.Properties.Select(p => p.Value));
                                try
                                {
                                    m.Text =
                                        $"{record.MachineName} :({record.LogName}) - {record.FormatDescription()}{properties}{(record.ThreadId != null ? " Thread id:" + record.ThreadId.Value : string.Empty)}";
                                    if (record.LevelDisplayName != null)
                                    {
                                        switch (record.LevelDisplayName)
                                        {
                                            case "Information":
                                                m.Level = AnalogyLogLevel.Information;
                                                break;
                                            case "Error":
                                                m.Level = AnalogyLogLevel.Error;
                                                break;
                                            case "Critical Error":
                                                m.Level = AnalogyLogLevel.Critical;
                                                break;
                                            case "Warning":
                                                m.Level = AnalogyLogLevel.Warning;
                                                break;
                                            case "Verbose":
                                                m.Level = AnalogyLogLevel.Verbose;
                                                break;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    var items = record.Properties[0].Value.ToString()
                                        .Split(_separators, StringSplitOptions.RemoveEmptyEntries);
                                    if (items.Any() && items.Length == 4)
                                    {
                                        m.Text = $"{record.MachineName} :({record.LogName}) - {items[3]} . Message ID: {items[2]}";
                                        switch (items[0])
                                        {
                                            case "Informational":
                                                m.Level = AnalogyLogLevel.Information;
                                                break;
                                            case "Information":
                                                m.Level = AnalogyLogLevel.Information;
                                                break;
                                            case "Error":
                                                m.Level = AnalogyLogLevel.Error;
                                                break;
                                            case "Critical":
                                                m.Level = AnalogyLogLevel.Critical;
                                                break;
                                            case "Critical Error":
                                                m.Level = AnalogyLogLevel.Critical;
                                                break;
                                            case "Warning":
                                                m.Level = AnalogyLogLevel.Warning;
                                                break;
                                            case "Verbose":
                                                m.Level = AnalogyLogLevel.Verbose;
                                                break;
                                            default:
                                                m.Level = AnalogyLogLevel.Information;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        m.Text =
                                            $"{record.MachineName} :({record.LogName}) - {record.FormatDescription()}{properties}{(record.ThreadId.HasValue ? " Thread id:" + record.ThreadId.Value : string.Empty)}";
                                        if (record.Level != null)
                                        {
                                            switch (record.Level.Value)
                                            {
                                                case 2:
                                                    m.Level = AnalogyLogLevel.Error;
                                                    break;
                                                case 3:
                                                    m.Level = AnalogyLogLevel.Warning;
                                                    break;
                                                case 4:
                                                    m.Level = AnalogyLogLevel.Information;
                                                    break;
                                                default:
                                                    m.Level = AnalogyLogLevel.Information;
                                                    break;
                                            }
                                        }

                                    }
                                }

                                messages.Add(m);
                                messagesHandler.AppendMessage(m, GetFileNameAsDataSource(fileName));
                                count++;
                                messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    string fail = "Failed To parse: " + fileName + " Error:" + e;
                    AnalogyLogMessage m = new AnalogyLogMessage
                    {
                        Text = fail,
                        Level = AnalogyLogLevel.Critical,
                        Class = AnalogyLogClass.General,
                        Source = "Analogy"
                    };
                    LogManager.Instance.LogError(e, $"Error reading file:{e.Message}",e, nameof(ReadFromFile));
                    messages.Add(m);
                    messagesHandler.AppendMessages(messages, GetFileNameAsDataSource(fileName));
                }

                if (!messages.Any())
                {
                    AnalogyLogMessage empty = new AnalogyLogMessage($"File {fileName} is empty or corrupted",
                        AnalogyLogLevel.Error, AnalogyLogClass.General, "Analogy", "None");
                    messages.Add(empty);
                    messagesHandler.AppendMessage(empty, GetFileNameAsDataSource(fileName));
                }

                return messages;
            }, Token);
        }

        private static string GetFileNameAsDataSource(string fileName)
        {
            string file = Path.GetFileName(fileName);
            return fileName != null && fileName.Equals(file) ? fileName : $"{file} ({fileName})";

        }
    }
}
