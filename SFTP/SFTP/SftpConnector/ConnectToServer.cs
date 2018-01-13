using Renci.SshNet;
using System.Configuration;
namespace SFTP.SftpConnector
{
    public class ConnectToServer
    {
        protected SftpClient ClientCon
        {
            get
            {
                SftpClient client=new SftpClient(ConfigurationManager.AppSettings["host"],int.Parse(ConfigurationManager.AppSettings["port"]),ConfigurationManager.AppSettings["userName"], ConfigurationManager.AppSettings["password"]);
                client.Connect();
                return client;
            }
        }
    }
}
