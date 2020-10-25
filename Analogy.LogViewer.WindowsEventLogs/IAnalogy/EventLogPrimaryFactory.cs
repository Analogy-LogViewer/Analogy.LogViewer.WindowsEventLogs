using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.Template;
using Analogy.LogViewer.WindowsEventLogs.Properties;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class EventLogPrimaryFactory : PrimaryFactory
    {
        internal static Guid id = new Guid("3999DB4C-0E22-4795-92C1-61B05EDB3F6C");
    
        public override Guid FactoryId { get; set; } = id;
        public override string Title { get; set; } = "Windows Event logs";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = new List<IAnalogyChangeLog>();
        public override IEnumerable<string> Contributors { get; set; } = new List<string>() { "Lior Banai" };
        public override string About { get; set; } = "Windows Event Log Data Provider";
        public override Image LargeImage { get; set; } = Resources.OperatingSystem_32x32;
        public override Image SmallImage { get; set; } = Resources.OperatingSystem_16x16;

    }
}
