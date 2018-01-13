using AtmImpsServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtmImpsServer
{
    class MiddleTier
    {
        Message_Fields mFields_obj = new Message_Fields();
        DBClass DBobject = new DBClass();
        private int maxTrackIdAtm = 0;
        private int maxTrackIdImps = 0;        
        public object History()
        {
            object history = null;
            history = DBobject.History();
            return history;
        }
        public Int64 Msg_Tracker(string[] request)
        {
            if (request[123] != "IMPS")
            {
                maxTrackIdAtm = DBobject.GetMaxId("atm_tran_msg_tracking", 1);
                string insert_query = "insert into atm_tran_msg_tracking(message_type,bit_map,account_no,processing_code,amount,transaction_date_time,system_trace_number,local_transaction_time,local_transaction_date,settlement_date,capture_date,fee,acquiring_institution_identification,track_2,retrival_ref_no,autherization_identification_response,response_code,card_acceptor_terminal_id,card_acceptor_terminal_name_location,additional_data_private,currency_code_transaction,currency_code_settlement,currency_code_cardholder_billing,additional_amount,reserved_private,original_data_element,replacement_amounts,account_identification,mini_statement,track_id,scroll_id,sys_date_time,network_management_info,scroll_id_fee) values ('" + request[0] + "','" + request[1] + "','" + request[2] + "','" + request[3] + "','" + request[4] + "','" + request[7] + "','" + request[11] + "','" + request[12] + "','" + request[13] + "','" + request[15] + "','" + request[17] + "','" + request[28] + "','" + request[32] + "','" + request[35] + "','" + request[37] + "','" + request[38] + "','" + request[39] + "','" + request[41] + "','" + request[43] + "','" + request[48] + "','" + request[49] + "','" + request[50] + "','" + request[51] + "','" + request[54] + "','" + request[60] + "','" + request[90] + "','" + request[95] + "','" + request[102] + "','" + request[121] + "','" + maxTrackIdAtm + "',null,(select now(*)),'" + request[70] + "',null)";
                DBobject.db_InsertUpdateDelete_Operations(insert_query);
                return maxTrackIdAtm;
            }
            else if (request[123] == "IMPS")
            {
                maxTrackIdImps = DBobject.GetMaxId("imps_trans_log", 1);
                string insert_query = "insert into imps_trans_log(track_id,message_type,bit_map,primary_account_no,processing_code,amount,transaction_date_time,system_trace_number,local_transaction_time,local_transaction_date,settlement_date,capture_date,fee,merchant_category_code,acquiring_institution_identification,retrival_ref_no,autherization_identification_response,response_code,card_acceptor_terminal_id,card_acceptor_terminal_name_location,currency_code_transaction,additional_amount,original_data_element,year,from_acno,to_acno,product_name,delivery_channel,imps_data,customer_name_mobile,utr_reference_no,mini_statement,scroll_id,sys_date_time,network_management_info,scroll_id_fee) values ('" + maxTrackIdImps + "','" + request[0] + "','" + request[1] + "','" + request[2] + "','" + request[3] + "','" + request[4] + "','" + request[7] + "','" + request[11] + "','" + request[12] + "','" + request[13] + "','" + request[15] + "','" + request[17] + "','" + request[28] + "','" + request[18] + "','" + request[32] + "','" + request[37] + "','" + request[38] + "','" + request[39] + "','" + request[41] + "','" + request[42] + "','" + request[49] + "','" + request[54] + "','" + request[56] + "','" + request[60] + "','" + request[102] + "','" + request[103] + "','" + request[123] + "','" + request[124] + "','" + request[125] + "','" + request[126] + "','" + request[127] + "','" + request[121] + "',null,(select now(*)),'" + request[70] + "',null)";
                DBobject.db_InsertUpdateDelete_Operations(insert_query);
                return maxTrackIdImps;
            }
            else
                return 0;
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
