using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class EventLogDataProvidersFactory : IAnalogyDataProvidersFactory
    {
        public Guid FactoryId { get; } = EventLogDataFactory.id;
        public string Title { get; } = "Analogy Built-In Windows Event Log Data Provider";

        public IEnumerable<IAnalogyDataProvider> DataProviders { get; }
        public EventLogDataProvidersFactory()
        {
            DataProviders = new List<IAnalogyDataProvider>
            {
                new RealTimeEventLogs(),
                new OfflineEventLogDataProvider(),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "System.evtx"),new Guid("17C93A69-2D7D-4620-9289-6889DE4EFC79")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Application.evtx"),new Guid("27C93A69-2D7D-4620-9289-6889DE4EFC80")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Security.evtx"),new Guid("37C93A69-2D7D-4620-9289-6889DE4EFC80")),
                new WindowsEventLogFile(Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs", "Security.evtx"),new Guid("47C93A69-2D7D-4620-9289-6889DE4EFC80")),

            };
        }
    }

    public class WindowsEventLogFile : IAnalogySingleFileDataProvider
    {
        public bool DisableFilePoolingOption { get; } = true;
        public string FileNamePath { get; }

        public Guid ID { get; }
        public string OptionalTitle { get; }
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public WindowsEventLogFile(string fileNamePath, Guid id)
        {

            FileNamePath = fileNamePath;
            ID = id;
            OptionalTitle = Path.GetFileName(FileNamePath);
        }
        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
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
