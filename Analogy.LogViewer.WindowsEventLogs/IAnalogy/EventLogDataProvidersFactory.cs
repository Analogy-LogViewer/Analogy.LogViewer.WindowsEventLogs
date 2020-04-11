using System;
using System.Collections.Generic;
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
            DataProviders = new List<IAnalogyDataProvider> { new RealTimeEventLogs() };
        }
    }
}
