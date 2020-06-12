using Analogy.DataProviders.Extensions;
using Analogy.LogViewer.WindowsEventLogs.Properties;
using System;
using System.Drawing;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class ImagesContainer : IAnalogyComponentImages
    {
        public Image GetLargeImage(Guid analogyComponentId) => Resources.OperatingSystem_32x32;

        public Image GetSmallImage(Guid analogyComponentId) => Resources.OperatingSystem_16x16;

        public Image GetOnlineConnectedLargeImage(Guid analogyComponentId) => Resources.OperatingSystem_32x32;

        public Image GetOnlineConnectedSmallImage(Guid analogyComponentId) => Resources.OperatingSystem_16x16;
        public Image GetOnlineDisconnectedLargeImage(Guid analogyComponentId) => Resources.OperatingSystem_32x32;
        public Image GetOnlineDisconnectedSmallImage(Guid analogyComponentId) => Resources.OperatingSystem_16x16;
    }
}
