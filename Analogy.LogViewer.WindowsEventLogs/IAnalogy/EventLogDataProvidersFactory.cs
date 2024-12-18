using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.Template.Managers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class EventLogDataProvidersFactory : DataProvidersFactory
    {
        public override Guid FactoryId { get; set; } = EventLogPrimaryFactory.id;
        public override string Title { get; set; } = "Windows Event Log Data Provider";

        public override IEnumerable<IAnalogyDataProvider> DataProviders { get; set; }
        public EventLogDataProvidersFactory()
        {
            DataProviders = new List<IAnalogyDataProvider>
            {
                new RealTimeEventLogs(),
                new OfflineEventLogDataProvider(),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "System.evtx"), new Guid("17C93A69-2D7D-4620-9289-6889DE4EFC79")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Application.evtx"), new Guid("27C93A69-2D7D-4620-9289-6889DE4EFC80")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Security.evtx"), new Guid("37C93A69-2D7D-4620-9289-6889DE4EFC80")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Setup.evtx"), new Guid("47C93A69-2D7D-4620-9289-6889DE4EFC80")),
            };
        }
    }

    public class WindowsEventLogFile : IAnalogySingleFileDataProvider
    {
        public bool DisableFilePoolingOption { get; } = true;
        public string FileNamePath { get; set; }
        public event EventHandler<AnalogyStartedProcessingArgs>? ProcessingStarted;
        public event EventHandler<AnalogyEndProcessingArgs>? ProcessingFinished;

        public virtual IEnumerable<AnalogyLogMessagePropertyName> HideExistingColumns() => Enumerable.Empty<AnalogyLogMessagePropertyName>();

        public virtual IEnumerable<string> HideAdditionalColumns() => Enumerable.Empty<string>();

        public Guid Id { get; set; }
        public Image? LargeImage { get; set; }
        public Image? SmallImage { get; set; }
        public string OptionalTitle { get; set; }
        public bool UseCustomColors { get; set; }
        public AnalogyToolTip? ToolTip { get; set; }

        public IEnumerable<(string OriginalHeader, string ReplacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color BackgroundColor, Color ForegroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public WindowsEventLogFile(string fileNamePath, Guid id)
        {
            FileNamePath = fileNamePath;
            Id = id;
            OptionalTitle = Path.GetFileName(FileNamePath);
        }
        public Task InitializeDataProvider(ILogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            return Task.CompletedTask;
        }

        public void MessageOpened(IAnalogyLogMessage message)
        {
            //nop
        }
        public void MessageSelected(IAnalogyLogMessage message)
        {
            //nop
        }

        public async Task<IEnumerable<IAnalogyLogMessage>> Process(CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            DateTime now = DateTime.Now;
            ProcessingStarted?.Invoke(this, new AnalogyStartedProcessingArgs());
            EventViewerLogLoader logLoader = new EventViewerLogLoader(token);
            var messages = await logLoader.ReadFromFile(FileNamePath, messagesHandler).ConfigureAwait(false);
            ProcessingFinished?.Invoke(this, new AnalogyEndProcessingArgs(now, DateTime.Now, "", messages.Count()));
            return messages;
        }
    }
}