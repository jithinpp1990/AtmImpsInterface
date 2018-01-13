using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmImpsServer.Models
{
    class ATM_IMPS_VAS_RequestModel
    {
        public string MessageType { get; set; }
        public Int64 TrackerID { get; set; }
        public string PAN { get; set; } 
        public string ProcesingCode{ get; set; }
        public double Amount{ get; set; }
        public double ActualAmount { get; set; }
        public string Date{ get; set; }
        public string FromAcNo{ get; set; }
        public string ToAcNo{ get; set; }
        public string IMPSdata{ get; set; }
        public string STAN { get; set; }
        public string AIIC{ get; set; }
        public string RRN{ get; set; }
        public string OriginalData { get; set; }
        public string TerminalID { get; set; }
        public string TerminalNameLocation { get; set; }
        public string ProductName { set; get; }
        public string DeliveryChanel { get; set; }
        public string TransactionType { get; set; }
        public string FromDateStmt { get; set; }
        public string ToDateStmt { get; set; }

        public string AtmCardNumber { get; set; }
        public string AtmDetails { get; set; }
        public string AccountType { get; set; }
        public double Fee { get; set; }

    }
}
