using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analogy.DataProviders.Extensions;
using Analogy.LogViewer.WindowsEventLogs.Properties;

namespace Analogy.LogViewer.WindowsEventLogs.IAnalogy
{
    public class  ImagesContainer : IAnalogyComponentImages
    {
        public IEnumerable<DataProviderImages> GetDataProviderImages()
        {
            yield return new DataProviderImages(EventLogDataFactory.id, Resources.OperatingSystem_16x16, Resources.OperatingSystem_32x32);
            yield return new DataProviderImages(new Guid("17C93A69-2D7D-4620-9289-6889DE4EFC79"), Resources.OperatingSystem_16x16, Resources.OperatingSystem_32x32);
            yield return new DataProviderImages(new Guid("27C93A69-2D7D-4620-9289-6889DE4EFC80"), Resources.OperatingSystem_16x16, Resources.OperatingSystem_32x32);
            yield return new DataProviderImages(new Guid("37C93A69-2D7D-4620-9289-6889DE4EFC80"), Resources.OperatingSystem_16x16, Resources.OperatingSystem_32x32);
            yield return new DataProviderImages(new Guid("47C93A69-2D7D-4620-9289-6889DE4EFC80"), Resources.OperatingSystem_16x16, Resources.OperatingSystem_32x32);
            yield return new DataProviderImages(new Guid("465F4963-71F3-4E50-8253-FA286BF5692B"), Resources.OperatingSystem_16x16, Resources.OperatingSystem_32x32);
            yield return new DataProviderImages(new Guid("407C8AD7-E7A3-4B36-9221-BB5D48E78766"), Resources.OperatingSystem_16x16, Resources.OperatingSystem_32x32);
        }

        public Image GetLargeImage(Guid analogyComponentId)
        {
            throw new NotImplementedException();
        }

        public Image GetSmallImage(Guid analogyComponentId)
        {
            throw new NotImplementedException();
        }

        public Image GetOnlineConnectedLargeImage(Guid analogyComponentId)
        {
            throw new NotImplementedException();
        }

        public Image GetOnlineConnectedSmallImage(Guid analogyComponentId)
        {
            throw new NotImplementedException();
        }

        public Image GetOnlineDisconnectedLargeImage(Guid analogyComponentId)
        {
            throw new NotImplementedException();
        }

        public Image GetOnlineDisconnectedSmallImage(Guid analogyComponentId)
        {
            throw new NotImplementedException();
        }
    }
}
