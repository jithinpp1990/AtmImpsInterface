using System.Configuration;
using System.Collections.Generic;
using iAnywhere.Data.SQLAnywhere;
using SFTP.Models;
using Dapper;
using System.Data;
using System.Linq;

namespace SFTP.DbOperations
{
    public class DBExecutions
    {
        public DBExecutions()
        {
            DbConnection.OpenConnection();
        }
        public SftpHostModel GetPrimaryData()
        {
            SftpHostModel host = new SftpHostModel();
            host.Host = ConfigurationManager.AppSettings["host"];
            host.Username = ConfigurationManager.AppSettings["userName"];
            host.Password = ConfigurationManager.AppSettings["password"];
            return host;
        }        

        public List<ServiceMap> GetServiceMap()
        {
            using (var connection = DbConnection._dbConnection)
            {
                var serviceDet = connection.Query<ServiceMap>("sp_atm_sftp_service_list", commandType: System.Data.CommandType.StoredProcedure).AsList();
                return serviceDet;
            }

        }

        public DataModel GenerateUploadData(ServiceMap service)
        {
            DataModel response = new DataModel();
            using (var connection = DbConnection._dbConnection)
            {
                var dataList = connection.Query<string>("sp_outward_data_cedge", new { as_service_type = service.ServiceName }, commandType: System.Data.CommandType.StoredProcedure).AsList();
                if (dataList.Count() > 2)
                {
                    response.DataList = dataList;
                    response.FileName = dataList.FirstOrDefault();
                    response.DataList.RemoveAt(0);
                    if(service.ServiceName!= "TRANACTIONACK")
                        response.DataList.RemoveAt(0);
                }
                return response;
            }
        }

        public List<DataModel> ProcessDownloadedData(ServiceMap service, List<DataModel> dataCollection = null)
        {
            int count = 0;
            List<DataModel> ack = new List<DataModel>();
            DataModel res = new DataModel();
            res.DataList = new List<string>();
            DynamicParameters parameter = new DynamicParameters();

            foreach (var dataFile in dataCollection)
            {
               // dataFile.DataList.RemoveAt(0);
                foreach (var data in dataFile.DataList)
                {
                    using (var connection = DbConnection._dbConnection)
                    {
                        parameter.Add("@as_message_type", service.ServiceName);
                        parameter.Add("@as_message", data);
                        parameter.Add("@as_file_name", dataFile.FileName.Replace(".txt", ""));
                        var result = (connection.Query<AckDataDto>("sp_create_inward_data_cedge", parameter, commandType: CommandType.StoredProcedure)).First();
                        count = count + result.Count;
                    }

                }
                if (service.ServiceName == "TRANSACTION")
                {
                    res.FileName = dataFile.FileName;
                    res.DataList.Add(count.ToString());
                    ack.Add(res);
                }

            }

            return ack;
        }

    }
}
