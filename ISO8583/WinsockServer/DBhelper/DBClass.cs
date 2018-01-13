using AtmImpsServer.Models;
using System;
using System.Data.Odbc;
using System.Data;
using System.IO;

namespace AtmImpsServer
{
    class DBClass
    {
        public string ip = null;
        public string[] ports = null;
        private string _con_string = "DSN=" + ServerLoadDefaults.Dsn + ";uid=" + ServerLoadDefaults.Uid + ";pwd=" + ServerLoadDefaults.Pwd + "";
        public static OdbcConnection _conn;
        public string responseCode = null;
        public string additionalData = null;
        private string[] return_value = null;
        private string sql_string = null;
        private string additional_amount = null;
        private OdbcCommand command;
        private OdbcDataAdapter adapter;
        Message_Fields mFields_obj = new Message_Fields();

        public int createConnection(string _uname, string _pwd)
        {

            //string _con_string1 = "DSN=" + _dsn + ";uid=" + _uname + ";pwd=" + _pwd + "";
            try
            {
                _conn = new OdbcConnection(_con_string);
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
                _conn.Open();
                return 1;
            }
            catch (Exception exx)
            {
                Console.WriteLine(exx);
                return 0;
            }
        }
        public void db_Connection_open()
        {
            try
            {
                _conn = new OdbcConnection(_con_string);
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
                _conn.Open();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }

        }
        private void db_Connection_close()
        {
            _conn.Close();
        }
        /////////////////////////////////////////////////////
        #region Common db functionalities
        public void db_InsertUpdateDelete_Operations(string _sql_string)
        {
            try
            {
                db_Connection_open();
                OdbcCommand _command_obj = new OdbcCommand(_sql_string, _conn);
                _command_obj.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            db_Connection_close();
        }
        public object db_select_operation(string _sql_string, string return_param)
        {

            db_Connection_open();
            string ret_val = null;
            DataTable _DT_obj = new DataTable();
            OdbcCommand _command_obj = new OdbcCommand(_sql_string, _conn);
            //OdbcDataAdapter _DA_obj = new OdbcDataAdapter(_command_obj);
            using (OdbcDataReader reader = _command_obj.ExecuteReader())
            {
                if (reader.Read())
                {
                    //Console.WriteLine(String.Format("{0}", reader[return_param]));
                    ret_val = String.Format("{0}", reader[return_param]);
                }
            }
            //_DA_obj.Fill(_DT_obj);
            //return _DT_obj;
            db_Connection_close();
            return ret_val;



        }
        public Double BalanceAmount(string as_account_no, string date)
        {

            string balance_amount;
            Double balance_amount_f;
            string return_param = "balance";
            string sql_String = "select (sum(amount_in)-sum(amount_out)) balance from cc.deposit_inout where deposit_id=(select deposit_id from deposits where deposit_no='" + as_account_no + "') and scroll_date<='" + date + "'";
            balance_amount = (string)db_select_operation(sql_String, return_param);
            balance_amount_f = Convert.ToDouble(balance_amount);

            return balance_amount_f;
        }
        public void msg_tracker(string[] msg)
        {
            db_Connection_open();
            OdbcCommand _cmd_obj = new OdbcCommand("{call cc.sf_test_tracker(?)}", _conn);
            _cmd_obj.CommandType = CommandType.StoredProcedure;
            _cmd_obj.Parameters.AddWithValue("@msg", msg);
            _cmd_obj.ExecuteNonQuery();
        }
        public void IPnPorts()
        {
            ports = new string[3];
            try
            {
                db_Connection_open();
                OdbcCommand _cmd_obj = new OdbcCommand("{call cc.sp_atm_ip_ports()}", _conn);
                _cmd_obj.CommandType = CommandType.StoredProcedure;
                DataTable _DT_obj = new DataTable();
                using (OdbcDataReader reader = _cmd_obj.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ip = String.Format("{0}", reader["ip"]);
                        ports[0] = String.Format("{0}", reader["port1"]);
                        ports[1] = String.Format("{0}", reader["port2"]);
                        ports[2] = String.Format("{0}", reader["port3"]);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public DataTable History()
        {
            DataTable DT_Obj = new DataTable();
            try
            {
                db_Connection_open();
                OdbcCommand _cmd_obj = new OdbcCommand("{call cc.sp_atm_tran_history()}", _conn);
                _cmd_obj.CommandType = CommandType.StoredProcedure;
                DataTable _DT_obj = new DataTable();
                using (OdbcDataAdapter DA = new OdbcDataAdapter(_cmd_obj))
                {
                    DA.Fill(DT_Obj);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DT_Obj;
        }
        public Int64 GetMaxId(string as_tablename, int ai_ids_reqd)
        {
            Int64 max_id = 0;
            try
            {
                string ss = "select sf_get_max_id('" + as_tablename + "','" + ai_ids_reqd + "') max ";
                max_id = Convert.ToInt64(db_select_operation(ss, "max"));

            }
            catch (Exception exx)
            {
                using (StreamWriter sw = File.AppendText("C:/atm_ex.txt"))
                {
                    sw.WriteLine(Convert.ToString(System.DateTime.Now) + "-------" + exx.ToString());
                }
            }
            return max_id;
        }
        public object db_select_operation_scalar(string _sql_string)
        {
            db_Connection_open();

            return 0;
        }

        public Int64 LogDataToDB(string[] request)
        {
            string serviceType = null;
            if (request[123] != "IMPS")
                serviceType = "ATM";
            else
                serviceType = "IMPS";
            sql_string = "select sf_insert_atm_imps_data('" + serviceType + "','" + request[0] + "','" + request[1] + "','" + request[2] + "','" + request[3] + "','" + request[4] + "','" + request[7] + "','" + request[11] + "','" + request[12] + "','" + request[13] + "','" + request[15] + "','" + request[17] + "','" + request[18] + "','" + request[28] + "','" + request[32] + "','" + request[35] + "','" + request[37] + "','" + request[38] + "','" + request[39] + "','" + request[41] + "','" + request[42] + "','" + request[43] + "','" + request[48] + "','" + request[49] + "','" + request[50] + "','" + request[51] + "','" + request[54] + "','" + request[56] + "','" + request[60] + "','" + request[70] + "','" + request[90] + "','" + request[95] + "','" + request[102] + "','" + request[103] + "','" + request[121] + "','" + request[123] + "','" + request[124] + "','" + request[125] + "','" + request[126] + "','" + request[127] + "') max_id";
            var returnValue = Convert.ToInt64(db_select_operation(sql_string, "max_id"));
            return returnValue;
        }

        #endregion
        #region ATM DB functionalities
        public ATM_IMPS_VAS_ResponseModel BalanceInquiry(ATM_IMPS_VAS_RequestModel model)
        {
            var BalanceData = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                sql_string = "select sf_get_atm_balance_enquiry('" + model.AtmCardNumber + "','" + model.PAN + "','" + model.Date + "','" + model.AccountType + "','" + model.TrackerID + "','" + model.Fee + "','" + model.ProcesingCode + "') additional_amounts";
                return_value = Convert.ToString(db_select_operation(sql_string, "additional_amounts")).Split('*');
                BalanceData.AdditionalAmount = return_value[0];
                BalanceData.ResponseCode = mFields_obj.ResponseCode(return_value[1]);

            }
            catch (Exception)
            {
                BalanceData.ResponseCode = "error_occured";

            }
            if (BalanceData.AdditionalAmount == "" || BalanceData.AdditionalAmount == "0")
                BalanceData.AdditionalAmount = null;
            return BalanceData;
        }
        public ATM_IMPS_VAS_ResponseModel Ministatement(ATM_IMPS_VAS_RequestModel model)
        {
            var MinistatementData = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                sql_string = "select sf_get_atm_mini_statement('" + model.AtmCardNumber + "','" + model.PAN + "','" + model.Date + "','" + model.AccountType + "','" + model.TrackerID + "') return_value";
                return_value = Convert.ToString(db_select_operation(sql_string, "return_value")).Split('*');
                MinistatementData.MimiStatement = return_value[0];
                MinistatementData.ResponseCode = mFields_obj.ResponseCode(return_value[1]);


            }
            catch (Exception)
            {
                MinistatementData.MimiStatement = null;
                return MinistatementData;

            }
            if (MinistatementData.MimiStatement == "" || MinistatementData.MimiStatement == "0")
                MinistatementData.MimiStatement = null;
            return MinistatementData;
        }
        public ATM_IMPS_VAS_ResponseModel CashWithdrawal(ATM_IMPS_VAS_RequestModel model)
        {
            var WithdrawalResponse = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                sql_string = "select sf_process_ATMs_PoS_ecommerce_withdrawals('" + model.ProcesingCode + "','" + model.AtmCardNumber + "','" + model.PAN + "','" + model.Date + "','" + model.Amount + "','" + model.AccountType + "','" + model.TrackerID + "','" + model.AtmDetails + "') response_code";
                WithdrawalResponse.ResponseCode = mFields_obj.ResponseCode(Convert.ToString(db_select_operation(sql_string, "response_code")));
            }
            catch (Exception)
            {
                WithdrawalResponse.ResponseCode = mFields_obj.ResponseCode("transaction rejected");
            }
            return WithdrawalResponse;
        }
        public ATM_IMPS_VAS_ResponseModel CashWithdrwalReverse(ATM_IMPS_VAS_RequestModel model)
        {
            var ReversalResponse = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                var originalMti = model.OriginalData.Substring(0, 4);
                var originalStan = model.OriginalData.Substring(4, 6);
                var originalDate = model.OriginalData.Substring(10, 10);
                var originalAiic = model.OriginalData.Substring(20, 6);
                sql_string = "select sf_process_ATMs_PoS_ecommerce_reversal('" + model.ProcesingCode + "','" + model.AtmCardNumber + "','" + model.PAN + "','" + model.Date + "','" + model.STAN + "','" + model.Amount + "' ,'" + originalMti + "','" + originalStan + "','" + originalAiic + "','" + originalDate + "','" + model.TrackerID + "','" + model.AtmDetails + "') response_code";
                ReversalResponse.ResponseCode = mFields_obj.ResponseCode(Convert.ToString(db_select_operation(sql_string, "response_code")));
            }
            catch (Exception ex)
            {
                ReversalResponse.ResponseCode = mFields_obj.ResponseCode("transaction rejected");
            }
            return ReversalResponse;
        }
        public ATM_IMPS_VAS_ResponseModel AcquirerCashWithdrawal(ATM_IMPS_VAS_RequestModel model)
        {
            var WithdrawalResponse = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                var sql_string = "select sf_process_atm_acquirer_withdrawals('" + model.ProcesingCode + "','" + model.AtmCardNumber + "','" + model.PAN + "','" + model.Date + "','" + model.Amount + "','" + model.AccountType + "','" + model.TrackerID + "','" + model.AtmDetails + "') response_code";
                WithdrawalResponse.ResponseCode = mFields_obj.ResponseCode(Convert.ToString(db_select_operation(sql_string, "response_code")));
            }
            catch (Exception ex)
            {
                WithdrawalResponse.ResponseCode = "transaction rejected";
            }
            return WithdrawalResponse;
        }
        public ATM_IMPS_VAS_ResponseModel AcquirerCashWithdrwalReverse(ATM_IMPS_VAS_RequestModel model)
        {
            var ReversalResponse = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                var originalMti = model.OriginalData.Substring(0, 4);
                var originalStan = model.OriginalData.Substring(4, 6);
                var originalDate = model.OriginalData.Substring(10, 10);
                var originalAiic = model.OriginalData.Substring(20, 6);
                sql_string = "select sf_process_atm_acquirer_withdrawals_reversal('" + model.ProcesingCode + "','" + model.AtmCardNumber + "','" + model.PAN + "','" + model.Date + "','" + model.STAN + "','" + model.Amount + "' ,'" + originalMti + "','" + originalStan + "','" + originalAiic + "','" + originalDate + "','" + model.TrackerID + "','" + model.AtmDetails + "') response_code";
                ReversalResponse.ResponseCode = mFields_obj.ResponseCode(Convert.ToString(db_select_operation(sql_string, "response_code")));
            }
            catch (Exception)
            {
                ReversalResponse.ResponseCode = "transaction rejected";
            }
            return ReversalResponse;
        }
        #endregion
        #region IMPS DB Functionalities
        public ATM_IMPS_VAS_ResponseModel Ministatement_IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                DataTable dt = new DataTable();
                string sql_string = "select sf_get_IMPS_mini_statement('" + model.PAN + "','" + model.Date + "','" + model.FromAcNo + "','" + model.TrackerID + "') statement_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "statement_data")).Split('*');
                ResponseModel.MimiStatement = return_value[0];
                ResponseModel.AdditionalAmount = return_value[1];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[2]);

            }
            catch (Exception ex)
            {
                return null;
            }
            if (ResponseModel.MimiStatement == "" || ResponseModel.MimiStatement == "0")
                ResponseModel.MimiStatement = null;
            return ResponseModel;
        }
        public ATM_IMPS_VAS_ResponseModel BalanceInquiry_IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                string sql_string = "select sf_get_IMPS_balance_enquiry('" + model.PAN + "','" + model.Date + "','" + model.FromAcNo + "','" + model.TrackerID + "','" + model.ProcesingCode + "','" + model.TransactionType + "') balance_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "balance_data")).Split('*');
                ResponseModel.AdditionalAmount = return_value[0];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);

            }
            catch (Exception ex)
            {
                return null;
            }
            if (ResponseModel.AdditionalAmount == "" || ResponseModel.AdditionalAmount == "0")
                ResponseModel.AdditionalAmount = null;
            return ResponseModel;
        }
        public ATM_IMPS_VAS_ResponseModel Statement_IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                DataTable dt = new DataTable();
                string sql_string = "select sf_get_IMPS_statement('" + model.PAN + "','" + model.Date + "','" + model.FromDateStmt + "','" + model.ToDateStmt + "','" + model.FromAcNo + "','" + model.TrackerID + "') stmt_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "stmt_data")).Split('*');
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[0]);

            }
            catch (Exception ex)
            {
                return null;
            }
            return ResponseModel;
        }
        public ATM_IMPS_VAS_ResponseModel ChequeRequest_IMPS(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {

                string sql_string = "select sf_get_IMPS_cheque_request('" + model.PAN + "','" + model.Date + "','" + model.FromAcNo + "','" + model.TrackerID + "') stmt_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "stmt_data")).Split('*');
                ResponseModel.MimiStatement = mFields_obj.ResponseCode(return_value[0]);
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);


            }
            catch (Exception ex)
            {
                return null;
            }
            if (ResponseModel.MimiStatement == "" || ResponseModel.MimiStatement == "0")
                ResponseModel.MimiStatement = null;
            return ResponseModel;
        }
        public ATM_IMPS_VAS_ResponseModel IMPS_Trans(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                string sql_string = "select sf_process_imps_transfer('" + model.TrackerID + "','" + model.PAN + "','" + model.ProcesingCode + "','" + model.TransactionType + "','" + model.Amount + "','" + model.Date + "','" + model.STAN + "','" + model.AIIC + "','" + model.RRN + "','" + model.TerminalID + "*" + model.TerminalNameLocation + "','" + model.FromAcNo + "','" + model.ToAcNo + "','" + model.IMPSdata + "') tran_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "tran_data")).Split('*');
                ResponseModel.AdditionalAmount = return_value[0];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);
                ResponseModel.CustomerNameMobile = return_value[2];
            }
            catch (Exception ex)
            {
                return null;
            }
            return ResponseModel;
        }
        public ATM_IMPS_VAS_ResponseModel IMPS_Trans_VM(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                string sql_string = "select sf_process_imps_transfer_vm('" + model.TrackerID + "','" + model.PAN + "','" + model.ProcesingCode + "','" + model.TransactionType + "','" + model.Amount + "','" + model.Date + "','" + model.STAN + "','" + model.AIIC + "','" + model.RRN + "','" + model.TerminalID + "*" + model.TerminalNameLocation + "','" + model.FromAcNo + "','" + model.ToAcNo + "','" + model.IMPSdata + "') tran_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "tran_data")).Split('*');
                ResponseModel.AdditionalAmount = return_value[0];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);
                ResponseModel.CustomerNameMobile = return_value[2];
            }
            catch (Exception ex)
            {
                return null;
            }
            return ResponseModel;
        }
        //public IMPS_VAS_ResponseModel P2P_IMPS(IMPS_VAS_RequestModel model)
        //{
        //    IMPS_VAS_ResponseModel ResponseModel = new IMPS_VAS_ResponseModel();
        //    try
        //    {
        //        string sql_string = "select sf_process_imps_p2p_transfer('"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"')";
        //        command = new OdbcCommand(sql_string, _conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new OdbcParameter("ai_track_id", model.TrackerID));
        //        command.Parameters.Add(new OdbcParameter("as_pan", model.PAN));
        //        command.Parameters.Add(new OdbcParameter("processing_code", model.ProcesingCode));
        //        command.Parameters.Add(new OdbcParameter("ad_amount", model.Amount));
        //        command.Parameters.Add(new OdbcParameter("ad_date", model.Date));
        //        command.Parameters.Add(new OdbcParameter("as_stan", model.STAN));
        //        command.Parameters.Add(new OdbcParameter("as_aiic", model.AIIC));
        //        command.Parameters.Add(new OdbcParameter("as_rrn", model.RRN));
        //        command.Parameters.Add(new OdbcParameter("as_terminal_id_location", model.TerminalID + "*" + model.TerminalNameLocation));
        //        command.Parameters.Add(new OdbcParameter("as_from_ac_no", model.FromAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_to_ac_no", model.ToAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_imps_data", model.IMPSdata));
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ResponseModel.AdditionalAmount = reader["additional_amount"].ToString();
        //            ResponseModel.ResponseCode = reader["response_code"].ToString();
        //            ResponseModel.CustomerNameMobile = reader["customer_name_mobile"].ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return ResponseModel;
        //}
        //public IMPS_VAS_ResponseModel P2A_IMPS(IMPS_VAS_RequestModel model)
        //{

        //    IMPS_VAS_ResponseModel ResponseModel = new IMPS_VAS_ResponseModel();
        //    try
        //    {
        //        string sql_string = "select sf_process_imps_p2a_transfer('"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"')";
        //        command = new OdbcCommand(sql_string, _conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new OdbcParameter("ai_track_id", model.TrackerID));
        //        command.Parameters.Add(new OdbcParameter("as_pan", model.PAN));
        //        command.Parameters.Add(new OdbcParameter("processing_code", model.ProcesingCode));
        //        command.Parameters.Add(new OdbcParameter("ad_amount", model.Amount));
        //        command.Parameters.Add(new OdbcParameter("ad_date", model.Date));
        //        command.Parameters.Add(new OdbcParameter("as_stan", model.STAN));
        //        command.Parameters.Add(new OdbcParameter("as_aiic", model.AIIC));
        //        command.Parameters.Add(new OdbcParameter("as_rrn", model.RRN));
        //        command.Parameters.Add(new OdbcParameter("as_terminal_id_location", model.TerminalID + "*" + model.TerminalNameLocation));
        //        command.Parameters.Add(new OdbcParameter("as_from_ac_no", model.FromAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_to_ac_no", model.ToAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_imps_data", model.IMPSdata));
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ResponseModel.AdditionalAmount = reader["additional_amount"].ToString();
        //            ResponseModel.ResponseCode = reader["response_code"].ToString();
        //            ResponseModel.CustomerNameMobile = reader["customer_name_mobile"].ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return ResponseModel;
        //}
        //public IMPS_VAS_ResponseModel P2U_IMPS(IMPS_VAS_RequestModel model)
        //{

        //    IMPS_VAS_ResponseModel ResponseModel = new IMPS_VAS_ResponseModel();
        //    try
        //    {
        //        string sql_string = "select sf_process_imps_p2u_transfer('"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"')";
        //        command = new OdbcCommand(sql_string, _conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new OdbcParameter("ai_track_id", model.TrackerID));
        //        command.Parameters.Add(new OdbcParameter("as_pan", model.PAN));
        //        command.Parameters.Add(new OdbcParameter("processing_code", model.ProcesingCode));
        //        command.Parameters.Add(new OdbcParameter("ad_amount", model.Amount));
        //        command.Parameters.Add(new OdbcParameter("ad_date", model.Date));
        //        command.Parameters.Add(new OdbcParameter("as_stan", model.STAN));
        //        command.Parameters.Add(new OdbcParameter("as_aiic", model.AIIC));
        //        command.Parameters.Add(new OdbcParameter("as_rrn", model.RRN));
        //        command.Parameters.Add(new OdbcParameter("as_terminal_id_location", model.TerminalID + "*" + model.TerminalNameLocation));
        //        command.Parameters.Add(new OdbcParameter("as_from_ac_no", model.FromAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_to_ac_no", model.ToAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_imps_data", model.IMPSdata));
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ResponseModel.AdditionalAmount = reader["additional_amount"].ToString();
        //            ResponseModel.ResponseCode = reader["response_code"].ToString();
        //            ResponseModel.CustomerNameMobile = reader["customer_name_mobile"].ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return ResponseModel;

        //}
        //public IMPS_VAS_ResponseModel VAS(IMPS_VAS_RequestModel model)
        //{
        //    IMPS_VAS_ResponseModel ResponseModel = new IMPS_VAS_ResponseModel();
        //    try
        //    {
        //        string sql_string = "select sf_process_imps_vas_transfer('"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"')";
        //        command = new OdbcCommand(sql_string, _conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new OdbcParameter("ai_track_id", model.TrackerID));
        //        command.Parameters.Add(new OdbcParameter("as_pan", model.PAN));
        //        command.Parameters.Add(new OdbcParameter("processing_code", model.ProcesingCode));
        //        command.Parameters.Add(new OdbcParameter("ad_amount", model.Amount));
        //        command.Parameters.Add(new OdbcParameter("ad_date", model.Date));
        //        command.Parameters.Add(new OdbcParameter("as_stan", model.STAN));
        //        command.Parameters.Add(new OdbcParameter("as_aiic", model.AIIC));
        //        command.Parameters.Add(new OdbcParameter("as_rrn", model.RRN));
        //        command.Parameters.Add(new OdbcParameter("as_terminal_id_location", model.TerminalID + "*" + model.TerminalNameLocation));
        //        command.Parameters.Add(new OdbcParameter("as_from_ac_no", model.FromAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_to_ac_no", model.ToAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_imps_data", model.IMPSdata));
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ResponseModel.AdditionalAmount = reader["additional_amount"].ToString();
        //            ResponseModel.ResponseCode = reader["response_code"].ToString();
        //            ResponseModel.CustomerNameMobile = reader["customer_name_mobile"].ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return ResponseModel;
        //}
        public ATM_IMPS_VAS_ResponseModel NEFT(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                string sql_string = "select sf_process_imps_neft_transfer('" + model.TrackerID + "','" + model.PAN + "','" + model.ProcesingCode + "','" + model.TransactionType + "','" + model.Amount + "','" + model.Date + "','" + model.STAN + "','" + model.AIIC + "','" + model.RRN + "','" + model.TerminalID + "*" + model.TerminalNameLocation + "','" + model.FromAcNo + "','" + model.ToAcNo + "','" + model.IMPSdata + "') tran_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "tran_data")).Split('*');
                ResponseModel.AdditionalAmount = return_value[0];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);
                ResponseModel.CustomerNameMobile = return_value[2];
                ResponseModel.UTRReferenceNumber = return_value[3];
            }
            catch (Exception ex)
            {
                return null;
            }
            return ResponseModel;
        }
        public ATM_IMPS_VAS_ResponseModel RTGS(ATM_IMPS_VAS_RequestModel model)
        {

            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                string sql_string = "select sf_process_imps_rtgs_transfer('" + model.TrackerID + "','" + model.PAN + "','" + model.ProcesingCode + "','" + model.TransactionType + "','" + model.Amount + "','" + model.Date + "','" + model.STAN + "','" + model.AIIC + "','" + model.RRN + "','" + model.TerminalID + "*" + model.TerminalNameLocation + "','" + model.FromAcNo + "','" + model.ToAcNo + "','" + model.IMPSdata + "') tran_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "tran_data")).Split('*');
                ResponseModel.AdditionalAmount = return_value[0];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);
                ResponseModel.CustomerNameMobile = return_value[2];
                ResponseModel.UTRReferenceNumber = return_value[3];
            }
            catch (Exception ex)
            {
                return null;
            }
            return ResponseModel;
        }
        public ATM_IMPS_VAS_ResponseModel IMPS_Reversal(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                string sql_string = "select sf_process_imps_transfer_reversal('" + model.TrackerID + "','" + model.PAN + "','" + model.ProcesingCode + "','" + model.TransactionType + "','" + model.Amount + "','" + model.Date + "','" + model.STAN + "','" + model.AIIC + "','" + model.RRN + "','" + model.TerminalID + "*" + model.TerminalNameLocation + "','" + model.FromAcNo + "','" + model.ToAcNo + "','" + model.IMPSdata + "','" + model.OriginalData + "') tran_rev_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "tran_rev_data")).Split('*');
                ResponseModel.AdditionalAmount = return_value[0];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);
                ResponseModel.CustomerNameMobile = return_value[2];

            }
            catch (Exception ex)
            {
                return null;
            }
            return ResponseModel;
        }
        //public IMPS_VAS_ResponseModel P2P_IMPS_Reversal(IMPS_VAS_RequestModel model)
        //{
        //    IMPS_VAS_ResponseModel ResponseModel = new IMPS_VAS_ResponseModel();
        //    try
        //    {
        //        string sql_string = "select sf_process_imps_p2p_transfer_reversa('"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"')";
        //        command = new OdbcCommand(sql_string, _conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new OdbcParameter("ai_track_id", model.TrackerID));
        //        command.Parameters.Add(new OdbcParameter("as_pan", model.PAN));
        //        command.Parameters.Add(new OdbcParameter("processing_code", model.ProcesingCode));
        //        command.Parameters.Add(new OdbcParameter("ad_amount", model.Amount));
        //        command.Parameters.Add(new OdbcParameter("ad_date", model.Date));
        //        command.Parameters.Add(new OdbcParameter("as_stan", model.STAN));
        //        command.Parameters.Add(new OdbcParameter("as_aiic", model.AIIC));
        //        command.Parameters.Add(new OdbcParameter("as_rrn", model.RRN));
        //        command.Parameters.Add(new OdbcParameter("as_terminal_id_location", model.TerminalID + "*" + model.TerminalNameLocation));
        //        command.Parameters.Add(new OdbcParameter("as_from_ac_no", model.FromAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_to_ac_no", model.ToAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_imps_data", model.IMPSdata));
        //        command.Parameters.Add(new OdbcParameter("as_original_data", model.OriginalData));
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ResponseModel.AdditionalAmount = reader["additional_amount"].ToString();
        //            ResponseModel.ResponseCode = reader["response_code"].ToString();
        //            ResponseModel.CustomerNameMobile = reader["customer_name_mobile"].ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return ResponseModel;
        //}
        //public IMPS_VAS_ResponseModel P2A_IMPS_Reversal(IMPS_VAS_RequestModel model)
        //{

        //    IMPS_VAS_ResponseModel ResponseModel = new IMPS_VAS_ResponseModel();
        //    try
        //    {
        //        string sql_string = "select sf_process_imps_p2a_transfer('"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"')";
        //        command = new OdbcCommand(sql_string, _conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new OdbcParameter("ai_track_id", model.TrackerID));
        //        command.Parameters.Add(new OdbcParameter("as_pan", model.PAN));
        //        command.Parameters.Add(new OdbcParameter("processing_code", model.ProcesingCode));
        //        command.Parameters.Add(new OdbcParameter("ad_amount", model.Amount));
        //        command.Parameters.Add(new OdbcParameter("ad_date", model.Date));
        //        command.Parameters.Add(new OdbcParameter("as_stan", model.STAN));
        //        command.Parameters.Add(new OdbcParameter("as_aiic", model.AIIC));
        //        command.Parameters.Add(new OdbcParameter("as_rrn", model.RRN));
        //        command.Parameters.Add(new OdbcParameter("as_terminal_id_location", model.TerminalID + "*" + model.TerminalNameLocation));
        //        command.Parameters.Add(new OdbcParameter("as_from_ac_no", model.FromAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_to_ac_no", model.ToAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_imps_data", model.IMPSdata));
        //        command.Parameters.Add(new OdbcParameter("as_original_data", model.OriginalData));

        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ResponseModel.AdditionalAmount = reader["additional_amount"].ToString();
        //            ResponseModel.ResponseCode = reader["response_code"].ToString();
        //            ResponseModel.CustomerNameMobile = reader["customer_name_mobile"].ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return ResponseModel;
        //}
        //public IMPS_VAS_ResponseModel P2U_IMPS_Reversal(IMPS_VAS_RequestModel model)
        //{

        //    IMPS_VAS_ResponseModel ResponseModel = new IMPS_VAS_ResponseModel();
        //    try
        //    {
        //        string sql_string = "select sf_process_imps_p2u_transfer('"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"')";
        //        command = new OdbcCommand(sql_string, _conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new OdbcParameter("ai_track_id", model.TrackerID));
        //        command.Parameters.Add(new OdbcParameter("as_pan", model.PAN));
        //        command.Parameters.Add(new OdbcParameter("processing_code", model.ProcesingCode));
        //        command.Parameters.Add(new OdbcParameter("ad_amount", model.Amount));
        //        command.Parameters.Add(new OdbcParameter("ad_date", model.Date));
        //        command.Parameters.Add(new OdbcParameter("as_stan", model.STAN));
        //        command.Parameters.Add(new OdbcParameter("as_aiic", model.AIIC));
        //        command.Parameters.Add(new OdbcParameter("as_rrn", model.RRN));
        //        command.Parameters.Add(new OdbcParameter("as_terminal_id_location", model.TerminalID + "*" + model.TerminalNameLocation));
        //        command.Parameters.Add(new OdbcParameter("as_from_ac_no", model.FromAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_to_ac_no", model.ToAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_imps_data", model.IMPSdata));
        //        command.Parameters.Add(new OdbcParameter("as_original_data", model.OriginalData));
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ResponseModel.AdditionalAmount = reader["additional_amount"].ToString();
        //            ResponseModel.ResponseCode = reader["response_code"].ToString();
        //            ResponseModel.CustomerNameMobile = reader["customer_name_mobile"].ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return ResponseModel;

        //}
        //public IMPS_VAS_ResponseModel VAS_Reversal(IMPS_VAS_RequestModel model)
        //{
        //    IMPS_VAS_ResponseModel ResponseModel = new IMPS_VAS_ResponseModel();
        //    try
        //    {
        //        string sql_string = "select sf_process_imps_vas_transfer('"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"','"++"')";
        //        command = new OdbcCommand(sql_string, _conn);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.Add(new OdbcParameter("ai_track_id", model.TrackerID));
        //        command.Parameters.Add(new OdbcParameter("as_pan", model.PAN));
        //        command.Parameters.Add(new OdbcParameter("processing_code", model.ProcesingCode));
        //        command.Parameters.Add(new OdbcParameter("ad_amount", model.Amount));
        //        command.Parameters.Add(new OdbcParameter("ad_date", model.Date));
        //        command.Parameters.Add(new OdbcParameter("as_stan", model.STAN));
        //        command.Parameters.Add(new OdbcParameter("as_aiic", model.AIIC));
        //        command.Parameters.Add(new OdbcParameter("as_rrn", model.RRN));
        //        command.Parameters.Add(new OdbcParameter("as_terminal_id_location", model.TerminalID + "*" + model.TerminalNameLocation));
        //        command.Parameters.Add(new OdbcParameter("as_from_ac_no", model.FromAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_to_ac_no", model.ToAcNo));
        //        command.Parameters.Add(new OdbcParameter("as_imps_data", model.IMPSdata));
        //        command.Parameters.Add(new OdbcParameter("as_original_data", model.OriginalData));
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ResponseModel.AdditionalAmount = reader["additional_amount"].ToString();
        //            ResponseModel.ResponseCode = reader["response_code"].ToString();
        //            ResponseModel.CustomerNameMobile = reader["customer_name_mobile"].ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //    return ResponseModel;
        //}
        public ATM_IMPS_VAS_ResponseModel NEFT_Reversal(ATM_IMPS_VAS_RequestModel model)
        {
            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                string sql_string = "select sf_process_imps_neft_transfer('" + model.TrackerID + "','" + model.PAN + "','" + model.ProcesingCode + "','" + model.TransactionType + "','" + model.Amount + "','" + model.Date + "','" + model.STAN + "','" + model.AIIC + "','" + model.RRN + "','" + model.TerminalID + "*" + model.TerminalNameLocation + "','" + model.FromAcNo + "','" + model.ToAcNo + "','" + model.IMPSdata + "','" + model.OriginalData + "') neft_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "neft_data")).Split('*');
                ResponseModel.AdditionalAmount = return_value[0];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);
                ResponseModel.CustomerNameMobile = return_value[2];

            }
            catch (Exception ex)
            {
                return null;
            }
            return ResponseModel;
        }
        public ATM_IMPS_VAS_ResponseModel RTGS_Reversal(ATM_IMPS_VAS_RequestModel model)
        {

            ATM_IMPS_VAS_ResponseModel ResponseModel = new ATM_IMPS_VAS_ResponseModel();
            try
            {
                string sql_string = "select sf_process_imps_rtgs_transfer('" + model.TrackerID + "','" + model.PAN + "','" + model.ProcesingCode + "','" + model.TransactionType + "','" + model.Amount + "','" + model.Date + "','" + model.STAN + "','" + model.AIIC + "','" + model.RRN + "','" + model.TerminalID + "*" + model.TerminalNameLocation + "','" + model.FromAcNo + "','" + model.ToAcNo + "','" + model.IMPSdata + "','" + model.OriginalData + "') rtgs_data";
                return_value = Convert.ToString(db_select_operation(sql_string, "rtgs_data")).Split('*');
                ResponseModel.AdditionalAmount = return_value[0];
                ResponseModel.ResponseCode = mFields_obj.ResponseCode(return_value[1]);
                ResponseModel.CustomerNameMobile = return_value[2];
            }
            catch (Exception ex)
            {
                return null;
            }
            return ResponseModel;
        }
        #endregion

    }
}
