
using System.Collections.Generic;

namespace SFTP.Models
{
    public static class ServicesDto
    {
        public static List<string> Services  = new List<string>(new string[] { "Topup","TopupAck", "StopAc","StopAcAck" ,"Transaction","TranactionAck", "TransactionReport" });
        public static List<string> DownloadServices=new List<string>(new string[] { "TopupAck","StopAcAck", "Transaction"});
        public static List<string> UploadServices = new List<string>(new string[] { "Topup", "StopAc", "TranactionAck", "TransactionReport" });
    }
}
