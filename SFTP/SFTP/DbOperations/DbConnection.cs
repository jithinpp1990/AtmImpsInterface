using System.Configuration;
using iAnywhere.Data.SQLAnywhere;
namespace SFTP.DbOperations
{
    public static class DbConnection
    {
        public static SAConnection _dbConnection;

        public static SAConnection OpenConnection()
        {
            _dbConnection = new SAConnection("Eng ="+ ConfigurationManager.AppSettings["dbEng"] +"; Uid = " + ConfigurationManager.AppSettings["dbUser"] + "; Pwd = sql; DBN ="+ ConfigurationManager.AppSettings["dbn"] + "; Links = tcpip(Host = "+ ConfigurationManager.AppSettings["dbIp"] + "); ");
            if (_dbConnection.State == System.Data.ConnectionState.Closed)
                _dbConnection.Open();
            return _dbConnection;

        }

    }
}
