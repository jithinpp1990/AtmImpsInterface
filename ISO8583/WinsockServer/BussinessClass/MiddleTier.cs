using AtmImpsServer.Models;
using System;

namespace AtmImpsServer
{
    class MiddleTier
    {
        Message_Fields mFields_obj = new Message_Fields();
        DBClass DBobject = new DBClass();   
        public object History()
        {
            object history = null;
            history = DBobject.History();
            return history;
        }
        public Int64 Msg_Tracker(string[] request)
        {
            return DBobject.LogDataToDB(request);
        }
        public Double accountBalane(string AccountNo, string date)
        {
            double accountBalane = 0.0;
            accountBalane = (Double)DBobject.BalanceAmount(AccountNo, date);
            return accountBalane;
        }

        #region ATM Functionalities
        public ATM_IMPS_VAS_ResponseModel BalanceInquiry(ATM_IMPS_VAS_RequestModel model)
        {
            var BalanceData = DBobject.BalanceInquiry(model);
            return BalanceData;
        }
        public ATM_IMPS_VAS_ResponseModel Ministatement(ATM_IMPS_VAS_RequestModel model)
        {
            var MinitmtData = DBobject.Ministatement(model);
            return MinitmtData;
        }
        public ATM_IMPS_VAS_ResponseModel CashWithdrawal(ATM_IMPS_VAS_RequestModel model)
        {

            var WithdrawalData = DBobject.CashWithdrawal(model);
            return WithdrawalData;
        }
        public ATM_IMPS_VAS_ResponseModel CashWithdrwalReverse(ATM_IMPS_VAS_RequestModel model)
        {
            var ReversalData = DBobject.CashWithdrwalReverse(model);
            return ReversalData;

        }
        public ATM_IMPS_VAS_ResponseModel AcquirerCashWithdrawal(ATM_IMPS_VAS_RequestModel model)
        {
            var WithdrawlaData = DBobject.AcquirerCashWithdrawal(model);
            return WithdrawlaData;
        }
        public ATM_IMPS_VAS_ResponseModel AcquirerCashWithdrwalReverse(ATM_IMPS_VAS_RequestModel model)
        {
            var ReversalData = DBobject.AcquirerCashWithdrwalReverse(model);
            return ReversalData;

        }
        #endregion

        #region IMPS Functionalities
        public ATM_IMPS_VAS_ResponseModel BalanceInquiry_IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            var ResultData = DBobject.BalanceInquiry_IMPS(model);
            return ResultData;
        }
        public ATM_IMPS_VAS_ResponseModel Ministatement_IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            var MiniStmtData = DBobject.Ministatement_IMPS(model);
            return MiniStmtData;
        }
        public ATM_IMPS_VAS_ResponseModel Statement_IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            return DBobject.Statement_IMPS(model);

        }
        public ATM_IMPS_VAS_ResponseModel ChequeRequest_IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            return DBobject.ChequeRequest_IMPS(model);
        }
        public ATM_IMPS_VAS_ResponseModel IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            var ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            ////IMPS Trans(in,out and intra)
            ////Transaction_type : -IMPS
            ////ProcessingCodeType  : -intra, -outward, -inward
            if (model.TransactionType == "45" || model.TransactionType == "48" || model.TransactionType == "94")
            {
                ResponseModel = DBobject.IMPS_Trans(model);
                return ResponseModel;
            }
            ////IMPS Trans VM(in,out and intra)
            ////Transaction_type :  -VM
            ////ProcessingCodeType  : inward
            if (model.TransactionType == "32" || model.TransactionType == "34")
            {
                ResponseModel = DBobject.IMPS_Trans_VM(model);
                return ResponseModel;
            }
            //NEFT Trans(in, out and intra)
            //Transaction_type : 88
            //ProcessingCodeType  : 88
            else if (model.TransactionType == "88" && model.ProcesingCode == "88")
            {
                ResponseModel = DBobject.NEFT(model);
                return ResponseModel;
            }
            //RTGS Trans(in,out and intra)
            //Transaction_type : 89
            //ProcessingCodeType  : 89
            else if (model.TransactionType == "89" && model.ProcesingCode == "89")
            {
                ResponseModel = DBobject.RTGS(model);
                return ResponseModel;
            }
            return ResponseModel;

        }
        public ATM_IMPS_VAS_ResponseModel IMPS_Reversal(ATM_IMPS_VAS_RequestModel model)
        {
            var ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            //IMPS Trans(in,out and intra)
            //Transaction_type : IMPS,VM
            //ProcessingCodeType  : 41-intra, 42-outward, 43-inward
            if (model.TransactionType == "45" || model.TransactionType == "32" || model.TransactionType == "48" || model.TransactionType == "34")
            {
                ResponseModel = DBobject.IMPS_Reversal(model);
                return ResponseModel;
            }
            ////P2A IMPS Trans(in,out and intra)
            ////Transaction_type : 48-IMPS, 34-VM
            ////ProcessingCodeType  : 41-intra, 42-outward, 43-inward
            //else if ((model.TransactionType == "48" || model.TransactionType == "34") && (model.ProcesingCode == "41" || model.ProcesingCode == "42" || model.ProcesingCode == "43"))
            //{
            //    ResponseModel = DBobject.P2A_IMPS_Reversal(model);
            //    return ResponseModel;
            //}
            ////P2U IMPS Trans(in,out and intra)
            ////Transaction_type : -IMPS, -VM
            ////ProcessingCodeType  : -intra, -outward, -inward
            //else if (model.TransactionType == "" && model.ProcesingCode == "")
            //{
            //    ResponseModel = DBobject.P2U_IMPS_Reversal(model);
            //    return ResponseModel;
            //}
            ////VAS Trans(in,out and intra)
            ////Transaction_type : 94
            ////ProcessingCodeType  : 94
            //else if (model.TransactionType == "94" && model.ProcesingCode == "94")
            //{
            //    ResponseModel = DBobject.VAS_Reversal(model);
            //    return ResponseModel;
            //}
            //NEFT Trans(in,out and intra)
            //Transaction_type : 88
            //ProcessingCodeType  : 88
            else if (model.TransactionType == "88" && model.ProcesingCode == "88")
            {
                ResponseModel = DBobject.NEFT_Reversal(model);
                return ResponseModel;
            }
            //RTGS Trans(in,out and intra)
            //Transaction_type : 89
            //ProcessingCodeType  : 89
            else if (model.TransactionType == "89" && model.ProcesingCode == "89")
            {
                ResponseModel = DBobject.RTGS_Reversal(model);
                return ResponseModel;
            }

            return ResponseModel;
        }
        #endregion

    }
}
