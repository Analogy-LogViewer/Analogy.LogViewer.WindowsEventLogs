using System;
using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class EventLogDataFactory : IAnalogyFactory
    {
        public static Guid id = new Guid("3999DB4C-0E22-4795-92C1-61B05EDB3F6C");
        public Guid FactoryId { get; } = id;
        public string Title { get; } = "Windows Event logs";
        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = new List<IAnalogyChangeLog>();
        public IEnumerable<string> Contributors { get; } = new List<string>() { "Lior Banai" };
        public string About { get; } = "Analogy Built-In Windows Event Log Data Provider";

    }
}
