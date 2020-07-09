using Analogy.DataProviders.Extensions;
using Analogy.LogViewer.WindowsEventLogs.Properties;
using System;
using System.Drawing;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class ImagesContainer : IAnalogyComponentImages
    {
        public Image GetLargeImage(Guid analogyComponentId)
        {
            if (analogyComponentId == EventLogDataFactory.id)
               return Resources.OperatingSystem_32x32;
            return null;
        }

        public Image GetSmallImage(Guid analogyComponentId)
        {
            if (analogyComponentId == EventLogDataFactory.id)
                return Resources.OperatingSystem_16x16;
            return null;
        }

            public Image GetOnlineConnectedLargeImage(Guid analogyComponentId) => Resources.OperatingSystem_32x32;

        public Image GetOnlineConnectedSmallImage(Guid analogyComponentId) => Resources.OperatingSystem_16x16;
        public Image GetOnlineDisconnectedLargeImage(Guid analogyComponentId) => Resources.OperatingSystem_32x32;
        public Image GetOnlineDisconnectedSmallImage(Guid analogyComponentId) => Resources.OperatingSystem_16x16;
    }
}
