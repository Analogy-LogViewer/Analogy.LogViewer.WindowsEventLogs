using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.Template.Managers;

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
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "System.evtx"),new Guid("17C93A69-2D7D-4620-9289-6889DE4EFC79")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Application.evtx"),new Guid("27C93A69-2D7D-4620-9289-6889DE4EFC80")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Security.evtx"),new Guid("37C93A69-2D7D-4620-9289-6889DE4EFC80")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Setup.evtx"),new Guid("47C93A69-2D7D-4620-9289-6889DE4EFC80")),

            };
        }
    }

    public class WindowsEventLogFile : IAnalogySingleFileDataProvider
    {
        public bool DisableFilePoolingOption { get; } = true;
        public string FileNamePath { get; set; }

        public IEnumerable<string> HideColumns()
        {
            return new List<string>(0);
        }

        public Guid Id { get; set; }
        public Image? LargeImage { get; set; } = null;
        public Image? SmallImage { get; set; } = null;
        public string OptionalTitle { get; set; }
        public bool UseCustomColors { get; set; } = false;
        public AnalogyToolTip? ToolTip { get; set; }

        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public WindowsEventLogFile(string fileNamePath, Guid id)
        {

            FileNamePath = fileNamePath;
            Id = id;
            OptionalTitle = Path.GetFileName(FileNamePath);
        }
        public Task InitializeDataProvider(IAnalogyLogger logger)
        {
            LogManager.Instance.SetLogger(logger);
            return Task.CompletedTask;
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //nop
        }

        public async Task<IEnumerable<AnalogyLogMessage>> Process(CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            EventViewerLogLoader logLoader = new EventViewerLogLoader(token);
            var messages = await logLoader.ReadFromFile(FileNamePath, messagesHandler).ConfigureAwait(false);
            return messages;
        }
    }

}
