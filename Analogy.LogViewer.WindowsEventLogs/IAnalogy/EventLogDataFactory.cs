using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.WindowsEventLogs.Properties;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class EventLogDataFactory : IAnalogyFactory
    {
        public static Guid id = new Guid("3999DB4C-0E22-4795-92C1-61B05EDB3F6C");
        public Guid FactoryId { get; set; } = id;
        public string Title { get; set; } = "Windows Event logs";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = new List<IAnalogyChangeLog>();
        public IEnumerable<string> Contributors { get; set; } = new List<string>() { "Lior Banai" };
        public string About { get; set; } = "Analogy Built-In Windows Event Log Data Provider";
        public Image LargeImage { get; set; } = Resources.OperatingSystem_32x32;
        public Image SmallImage { get; set; } = Resources.OperatingSystem_16x16;

    }
}
