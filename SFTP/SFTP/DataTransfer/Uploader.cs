using System.IO;
using SFTP.SftpConnector;
using SFTP.Models;
using SFTP.DbOperations;
using System.Collections.Generic;
using System.Linq;
using System;
namespace SFTP.DataTransfer
{
    public class Uploader: ConnectToServer
    {
     
        public void GenerateUploadData(List<ServiceMap> serviceList)
        {
            DBExecutions dbObj = new DBExecutions();
            List<string> er = new List<string>();
            var list = serviceList.Where(x => x.Type == "U").ToList();
            try
            {
                foreach (var service   in list)
                {                    
                    var response = dbObj.GenerateUploadData(service);
                    if (response.DataList!=null && response.FileName!=null)
                        FileWriter.WriteFile(service.LocalDir, response.FileName, response.DataList);
                    UploadData(service);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                er.Add(DateTime.Now.ToString()+"---Uploader Exception---"+ex.ToString());
                FileWriter.WriteFile(System.Configuration.ConfigurationManager.AppSettings["errorlogDir"], System.Configuration.ConfigurationManager.AppSettings["errorlogFile"], er);
            }
        }
        public void UploadData(ServiceMap service)
        {
            DirectoryInfo directoryLocal = new DirectoryInfo(service.LocalDir);
            using (var clientConnection =ClientCon)
            {
                foreach (var file in directoryLocal.GetFiles("*.txt"))
                {
                    clientConnection.ChangeDirectory(service.RemoteDir);
                    using (FileStream fs = new FileStream(file.FullName, FileMode.Open))
                    {
                        clientConnection.BufferSize = 4 * 1024;
                        clientConnection.UploadFile(fs,file.Name);
                        Console.WriteLine("Uploaded "+ service.ServiceName + " File "+ file.Name+ " on " + Convert.ToString(DateTime.Now));
                    }
                    file.MoveTo(service.LocalArchiveDir+file.Name);
                    file.MoveTo(service.LocalArchiveDir + file.Name.Replace("txt","done"));
                }
            }
        }
    }
}
