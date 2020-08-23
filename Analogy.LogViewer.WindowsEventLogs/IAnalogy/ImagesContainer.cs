using Analogy.DataProviders.Extensions;
using Analogy.LogViewer.WindowsEventLogs.Properties;
using System;
using System.Drawing;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{

    public class ImagesContainer : IAnalogyComponentImages
    {
        private Guid system = new Guid("17C93A69-2D7D-4620-9289-6889DE4EFC79");
        private Guid application = new Guid("27C93A69-2D7D-4620-9289-6889DE4EFC80");
        private Guid security = new Guid("37C93A69-2D7D-4620-9289-6889DE4EFC80");
        private Guid setup = new Guid("47C93A69-2D7D-4620-9289-6889DE4EFC80");
        public Image GetLargeImage(Guid analogyComponentId)
        {
            if (analogyComponentId == EventLogDataFactory.id ||
                analogyComponentId == system ||
                analogyComponentId == application ||
                analogyComponentId == setup ||
                analogyComponentId == security)
                return Resources.OperatingSystem_32x32;
            return null;
        }

        public Image GetSmallImage(Guid analogyComponentId)
        {
            if (analogyComponentId == EventLogDataFactory.id ||
                analogyComponentId == system ||
                analogyComponentId == application ||
                analogyComponentId == setup ||
                analogyComponentId == security)
                return Resources.OperatingSystem_16x16;
            return null;
        }

        public Image GetOnlineConnectedLargeImage(Guid analogyComponentId) => Resources.OperatingSystem_32x32;

        public Image GetOnlineConnectedSmallImage(Guid analogyComponentId) => Resources.OperatingSystem_16x16;
        public Image GetOnlineDisconnectedLargeImage(Guid analogyComponentId) => Resources.OperatingSystem_32x32;
        public Image GetOnlineDisconnectedSmallImage(Guid analogyComponentId) => Resources.OperatingSystem_16x16;
    }
}
