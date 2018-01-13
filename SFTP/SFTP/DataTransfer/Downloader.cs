using System.Collections.Generic;
using System.Linq;
using System.IO;
using SFTP.Models;
using SFTP.SftpConnector;
using SFTP.DbOperations;
using System;
namespace SFTP.DataTransfer
{
    public class Downloader : ConnectToServer
    {
        private DBExecutions _dbObj = new DBExecutions();
        public void DownLoadAndProcess(List<ServiceMap> serviceList)
        {

            List<DataModel> response = new List<DataModel>();
            List<string> er = new List<string>();
            var list = serviceList.Where(x => x.Type == "D").ToList();
            try
            {
                foreach (var service in list)
                {
                    var redData = ReadData(service);
                    if (redData.Count > 0)
                        response = _dbObj.ProcessDownloadedData(service, redData);
                    if (response.Count > 0 && service.ServiceName == "TRANSACTION")
                    {
                        foreach (var resFile in response)
                        {
                            FileWriter.WriteFile(serviceList.Where(x => x.ServiceName == "TRANACTIONACK").First().LocalDir, resFile.FileName, resFile.DataList);
                         
                        }
                      
                    }
                }
            }
            catch (Exception ex)
            {
                er.Add(DateTime.Now.ToString() + "---Downloader Exception---" + ex.ToString());
                FileWriter.WriteFile(System.Configuration.ConfigurationManager.AppSettings["errorlogDir"], System.Configuration.ConfigurationManager.AppSettings["errorlogFile"], er);

            }
        }


        public List<DataModel> ReadData(ServiceMap service)
        {
            List<DataModel> dataCollection = new List<DataModel>();
            DataModel data = new DataModel();
            using (var clientConnection = ClientCon)
            {

                var files = clientConnection.ListDirectory(service.RemoteDir);
                foreach (var file in (files.Where(x => !x.Name.StartsWith(".")).ToList()))
                {
                    if (file.Name.StartsWith(service.FileNamePrefix))
                    {
                        string remoteFileName = file.Name;
                        FileWriter.WriteFile(service.LocalDir, remoteFileName);
                        File.OpenWrite(remoteFileName);
                        Stream dataStream = File.OpenWrite(service.LocalDir + remoteFileName);
                        clientConnection.DownloadFile(service.RemoteDir + remoteFileName, dataStream);
                        var remoteData = clientConnection.ReadAllLines(service.RemoteDir + remoteFileName);
                        FileWriter.WriteFile(service.LocalRemoteDir, remoteFileName, remoteData.ToList());
                        data.DataList = remoteData.ToList();
                        data.FileName = remoteFileName;
                        dataCollection.Add(data);
                        clientConnection.RenameFile(service.RemoteDir + remoteFileName, service.RemoteArchiveDir + remoteFileName);
                        clientConnection.RenameFile(service.RemoteArchiveDir + remoteFileName, service.RemoteArchiveDir + remoteFileName.Replace("txt","done"));
                        Console.WriteLine("Downloaded "+ service.ServiceName + " File " + remoteFileName + " on " + Convert.ToString(DateTime.Now));
                    }
                }
                return dataCollection;
            }

        }
    }
}
