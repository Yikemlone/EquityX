using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquityX.Utilities
{
    public static class PathDB
    {
        public static string GetPath(string dbName)
        {
            string path = string.Empty;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", dbName);
            }
            else
            {
                path = dbName;
            }

            return path;
        }
    }
}
