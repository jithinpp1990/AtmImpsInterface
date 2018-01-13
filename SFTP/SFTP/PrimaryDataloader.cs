using SFTP.Models;
using SFTP.DbOperations;
using System;
using SFTP.SftpConnector;
using System.Configuration;
using System.Collections.Generic;
namespace SFTP
{
    public class PrimaryDataloader : ConnectToServer
    {
        private static DBExecutions _DbObject = new DBExecutions();
        public static SftpHostModel GetPrimaryData()
        {

            var primaryData = _DbObject.GetPrimaryData();

            return primaryData;
        }

        public static List<ServiceMap> GetServiceMap()
        {
            var serviceDet = _DbObject.GetServiceMap();
            if (serviceDet.Count > 0)
                Console.WriteLine("Connected To DB Server");
            else
                Console.WriteLine("DB Server Connection Failed");
            return serviceDet;
        }

        public  void CheckRemoteDir()
        {
            try
            {
                using (var clientConnection = ClientCon)
                {
                    var files = clientConnection.ListDirectory(ConfigurationManager.AppSettings["sftpDir"]);
                    foreach(var file in files)
                    {
                        Console.WriteLine(file.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}
