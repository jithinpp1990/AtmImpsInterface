using System.Collections.Generic;
namespace SFTP.Models
{
    public class ServiceMap
    {
        public string ServiceName { get; set; }
        public string LocalDir { get; set; }
        public string LocalArchiveDir { get; set; }
        public string RemoteDir { get; set; }
        public string RemoteArchiveDir { get; set; }
        public string LocalRemoteDir { get; set; }
        public string LocalRemoteArchiveDir { get; set; }
        public string Type { get; set; }
        public string FileNamePrefix { get; set; }
    }
}
