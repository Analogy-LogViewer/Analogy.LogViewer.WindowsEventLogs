using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.WindowsEventLogs.Managers;

namespace Analogy.LogViewer.WindowsEventLogs
{
    public class OfflineEventLogDataProvider : OfflineDataProvider
    {
        public override string OptionalTitle { get; set; } = "Windows Event Log Data Provider";
        public override Guid Id { get; set; } = new Guid("465F4963-71F3-4E50-8253-FA286BF5692B");
        public override Image LargeImage { get; set; } = null;
        public override Image SmallImage { get; set; } = null;
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

        public override bool CanSaveToLogFile { get; set; } = false;
        public override string FileOpenDialogFilters { get; set; } = "Windows Event log files (*.evtx)|*.evtx";
        public override string FileSaveDialogFilters { get; set; } = "";
        public override IEnumerable<string> SupportFormats { get; set; } = new[] { "*.evtx" };

        public override string InitialFolderFullPath { get; set; } =
            Path.Combine(Environment.ExpandEnvironmentVariables("%SystemRoot%"), "System32", "Winevt", "Logs");
        public override bool DisableFilePoolingOption { get; set; } = false;

        public override async Task<IEnumerable<AnalogyLogMessage>> Process(string fileName, CancellationToken token, ILogMessageCreatedHandler messagesHandler)
        {
            if (CanOpenFile(fileName))
            {
                EventViewerLogLoader logLoader = new EventViewerLogLoader(token);
                var messages = await logLoader.ReadFromFile(fileName, messagesHandler).ConfigureAwait(false);
                return messages;
            }

            AnalogyLogMessage m = new AnalogyLogMessage
            {
                Text = $"Unsupported file: {fileName}. Skipping file",
                Level = AnalogyLogLevel.Critical,
                Source = "Analogy",
                Module = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id,
                Class = AnalogyLogClass.General,
                User = Environment.UserName,
                Date = DateTime.Now
            };
            messagesHandler.AppendMessage(m, Environment.MachineName);
            return new List<AnalogyLogMessage>() { m };
        }

        public override bool CanOpenFile(string fileName) => fileName.EndsWith(".evtx", StringComparison.InvariantCultureIgnoreCase);
        public override bool CanOpenAllFiles(IEnumerable<string> fileNames) => fileNames.All(CanOpenFile);

        protected override List<FileInfo> GetSupportedFilesInternal(DirectoryInfo dirInfo, bool recursive)
        {
            List<FileInfo> files = dirInfo.GetFiles("*.evtx").ToList();
            if (!recursive)
            {
                return files;
            }

            try
            {
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    files.AddRange(GetSupportedFilesInternal(dir, true));
                }
            }
            catch (Exception)
            {
                return files;
            }

            return files;
        }

    }
}
