using AtmImpsServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////Version : ISO 8583 : 1987/////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AtmImpsServer.Response
{
    class ResponseBuilder
    {
        MiddleTier MTObject = new MiddleTier();
        Message_Fields mFields = new Message_Fields();
        private Boolean ValidationResponse = true;

        public string[] Response(string[] request)
        {
            string MessageType = null;
            string requestBitmap = null;
            string[] responseMessage = new string[128];
            string[] responseBitmapBinary = new string[128] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
            requestBitmap = request[1];
            ATM_IMPS_VAS_RequestModel requestModel = new ATM_IMPS_VAS_RequestModel();
            try
            {
                if (request[0] != "0800" && request[70] != "301")
                {
                    requestModel.TrackerID = MTObject.Msg_Tracker(request);
                }
                MessageType = request[0];
                var echoData = EchoBuilder(request, responseBitmapBinary);
                responseMessage = echoData[0];
                responseBitmapBinary = echoData[1];
                responseMessage[0] = mFields.messageType(MessageType);
                responseBitmapBinary[0] = "1";               
                if (request[0] != "0800")
                {
                    requestModel= SetRequestModel(request);
                    var responseData= NonEchoResponse(responseMessage,responseBitmapBinary, requestModel);
                    responseMessage = responseData.ResponseArray;
                    responseBitmapBinary = responseData.BitmapBinary;
                }
                responseMessage[1] = BitMapBuilder(responseBitmapBinary);
                return responseMessage;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ATM_IMPS_VAS_RequestModel SetRequestModel(string[] request)
        {
            ATM_IMPS_VAS_RequestModel reqObject = new ATM_IMPS_VAS_RequestModel();
            reqObject.MessageType = request[0];
            reqObject.PAN = request[2];
            reqObject.STAN = request[11];
            reqObject.AIIC = request[32];
            reqObject.RRN = request[37];
            reqObject.TerminalID = request[41];
            reqObject.TerminalNameLocation = request[42];
            reqObject.AtmCardNumber = request[48];
            reqObject.OriginalData = request[56];
            reqObject.FromAcNo = request[102];
            reqObject.ToAcNo = request[103];
            reqObject.ProductName = request[123];
            reqObject.IMPSdata = request[125];
            reqObject.AtmDetails = request[41] + "*" + request[43];
            if (request[28] != null && request[28] != "")
                reqObject.Fee = Convert.ToDouble(request[28].Substring(1, 6) + "." + (request[28].Substring(7, 2)));
            if (request[3] != null && request[3] != "")
            {
                reqObject.ProcesingCode = request[3].Substring(0, 2);
                reqObject.AccountType = request[3].Substring(2, 2);
            }
            if (request[4] != null && request[4] != "")
                reqObject.Amount = Convert.ToDouble(request[4].Substring(0, 10) + "." + request[4].Substring(10, 2));
            if(request[95]!=null && request[95]!="")
                reqObject.ActualAmount= Convert.ToDouble(request[95].Substring(0, 10) + "." + request[95].Substring(10, 2));
            if (request[7] != null && request[7] != "")
                reqObject.Date = request[60].Substring(0, 4) + "-" + request[7].Substring(0, 2) + "-" + request[7].Substring(2, 2);
            if (request[125] != null && request[125] != "")
                reqObject.TransactionType = request[125].Substring((request[125].IndexOf('|', request[125].IndexOf('|') + 1)) + 1, 2);
            return reqObject;
        }
        public List<string[]> EchoBuilder(string[] requesArray,string[] bitmapBinary)
        {
            //string[] bitmapBinary = new string[128] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
            List<string[]> responseList = new List<string[]>();           
            string bitMap = requesArray[1];
            string[] responseArray = new string[128];
            int j = 0;
            for (int i = bitMap.IndexOf('1'); i > -1; i = bitMap.IndexOf('1', i + 1))
            {
                j = i + 1;
                bitmapBinary[i] = "1";
                responseArray[j] = requesArray[j];
                if (j == 2 || j == 32 || j == 34 || j == 35 || j == 102 || j == 103)
                {
                    responseArray[j] = Convert.ToString(responseArray[j].Length).PadLeft(2, '0') + responseArray[j];
                }
                if (j == 46 || j == 48 || j == 60 || j == 123 || j == 124 || j == 125 || j == 126)//|| j == 43

                {
                    responseArray[j] = Convert.ToString(responseArray[j].Length).PadLeft(3, '0') + responseArray[j];
                }
            }
            responseList.Add(responseArray);
            responseList.Add(bitmapBinary);
            return responseList;
        }
        public NonEchoResponseModel NonEchoResponse(string[] responseArray,string[] bitMapArray, ATM_IMPS_VAS_RequestModel requestModel )
        {
            string additionalAmounts = null;
            string accountIdentification = null;
            string miniStatement = null;
            string responseCode = "00";
            string nuupData = null;
            string utrRefNo = null;
            string customerNameMobile = null;
            NonEchoResponseModel responseData = new NonEchoResponseModel();
            if (requestModel.MessageType != "0800" && requestModel.ProductName != "IMPS")
            {
                switch (requestModel.MessageType)
                {
                    //Finantial TransactonMessage
                    case "0200":
                        //Balance Inquiry 
                        if (requestModel.ProcesingCode == "30" || requestModel.ProcesingCode == "31" || requestModel.ProcesingCode == "32")
                        {
                            var BalanceData = MTObject.BalanceInquiry(requestModel);
                            additionalAmounts = BalanceData.AdditionalAmount;
                            responseCode = BalanceData.ResponseCode;
                            accountIdentification = requestModel.PAN;
                        }
                        //Cash Withdrawal-00,10 POS Purchase-45,46,44,49 (no Reversal)
                        else if (requestModel.ProcesingCode == "00" || requestModel.ProcesingCode == "01" || requestModel.ProcesingCode == "45" || requestModel.ProcesingCode == "46" || requestModel.ProcesingCode == "44" || requestModel.ProcesingCode == "49")// 
                        {
                            if (requestModel.Amount <= 0)
                            {
                                responseCode = mFields.ResponseCode("invalid_amount_requested");
                            }
                            else
                            {
                                var WithdrawalData = MTObject.CashWithdrawal(requestModel);
                                responseCode = WithdrawalData.ResponseCode;
                                if (responseCode == "00")
                                {
                                    var BalanceData = MTObject.BalanceInquiry(requestModel);
                                    additionalAmounts = BalanceData.AdditionalAmount;
                                }
                                accountIdentification = requestModel.PAN;
                            }
                        }
                        //MiniStatement
                        else if (requestModel.ProcesingCode == "35" || requestModel.ProcesingCode == "36" || requestModel.ProcesingCode == "37")
                        {
                            var MinistmtData = MTObject.Ministatement(requestModel);
                            miniStatement = MinistmtData.MimiStatement;
                            responseCode = MinistmtData.ResponseCode;
                            if (responseCode == "00")
                            {
                                var BalanceData = MTObject.BalanceInquiry(requestModel);
                                additionalAmounts = BalanceData.AdditionalAmount;
                            }
                            accountIdentification = requestModel.PAN;
                        }
                        //PIN Change Notificatuon
                        else if (requestModel.ProcesingCode == "90" || requestModel.ProcesingCode == "92")
                        {
                            if (requestModel.Fee > 0)
                            {
                                var BalanceData = MTObject.BalanceInquiry(requestModel);
                                additionalAmounts = BalanceData.AdditionalAmount;
                                responseCode = BalanceData.ResponseCode;
                                accountIdentification = requestModel.PAN;
                            }
                        }
                        else
                        {
                            responseCode = "99";
                        }
                        break;
                    //Cash Withdrawal Reversal and POS Reversal Message
                    case "0420":
                        if (requestModel.ProcesingCode == "00" || requestModel.ProcesingCode == "01" || requestModel.ProcesingCode == "45" || requestModel.ProcesingCode == "46" || requestModel.ProcesingCode == "44" || requestModel.ProcesingCode == "49")
                        {
                            double Actual_amount_reversal = 0;
                            if (requestModel.ActualAmount >0)
                                  Actual_amount_reversal = requestModel.Amount - requestModel.ActualAmount;
                            else
                                Actual_amount_reversal = requestModel.Amount;

                            var ReversalData1 = MTObject.CashWithdrwalReverse(requestModel);
                            if (responseCode == "00")
                            {
                                var BalanceData = MTObject.BalanceInquiry(requestModel);
                                additionalAmounts = BalanceData.AdditionalAmount;
                            }
                            accountIdentification = requestModel.PAN;

                            if (responseCode != "00" && (requestModel.ProcesingCode == "45" || requestModel.ProcesingCode == "46" || requestModel.ProcesingCode == "44" || requestModel.ProcesingCode == "49"))
                            {
                                responseArray[4] = "000000000000";
                            }

                        }
                        else if (requestModel.ProcesingCode == "19")
                        {
                            double Actual_amount_reversal = 0;
                            if (requestModel.ActualAmount > 0)
                                Actual_amount_reversal = requestModel.Amount - requestModel.ActualAmount;
                            else
                                Actual_amount_reversal = requestModel.Amount;
                            var ReversalData = MTObject.AcquirerCashWithdrwalReverse(requestModel);
                           accountIdentification = requestModel.PAN;
                        }
                        else
                        {
                            responseCode = "99";
                        }
                        break;
                    //NFS Acquirer Withdrwal Message
                    case "0220":
                        if (requestModel.ProcesingCode == "19")
                        {
                            responseArray[28] = "C00000000";
                            if (requestModel.Amount <= 0)
                            {
                                responseCode = mFields.ResponseCode("invalid_amount_requested");
                            }
                            else
                            {
                                var WithdrawalData = MTObject.AcquirerCashWithdrawal(requestModel);
                                accountIdentification = requestModel.PAN;
                            }
                        }
                        else
                        {
                            responseCode = "99";
                        }
                        break;

                    case "800":
                        break;
                    default:
                        responseCode = "97";
                        break;
                }
            }
            //FOR IMPS
            else if (requestModel.ProductName == "IMPS")
            {
                switch (requestModel.MessageType)
                {
                    case "0200":
                        //Balance Inquiry 
                        if (requestModel.ProcesingCode == "31")// 
                        {
                            var BalanceData = MTObject.BalanceInquiry_IMPS(requestModel);
                            additionalAmounts = BalanceData.AdditionalAmount;
                            nuupData = BalanceData.NUUPdata;
                            responseCode = BalanceData.ResponseCode;
                        }
                        //MiniStatement
                        else if (requestModel.ProcesingCode == "36")
                        {
                            var StatementData = MTObject.Ministatement_IMPS(requestModel);
                            miniStatement = StatementData.MimiStatement;
                            responseCode = StatementData.ResponseCode;
                            if (responseCode == "00")
                                additionalAmounts = StatementData.AdditionalAmount;
                        }
                        //Cheque Book Request
                        else if (requestModel.ProcesingCode == "95")
                        {
                            var ChequeResponse = MTObject.ChequeRequest_IMPS(requestModel);
                            responseCode = ChequeResponse.ResponseCode;

                        }
                        //Statement request
                        else if (requestModel.ProcesingCode == "97")
                        {
                            var StatementData = MTObject.Statement_IMPS(requestModel);
                            responseCode = StatementData.ResponseCode;
                        }
                        //P2P,P2A,P2U IMPS Trans(in,out and intra)
                        //Transaction_type : 45-IMPS, 43-VAS
                        //ProcessingCodeType  : 41-intra, 42-inward, 43-outward
                        else if (requestModel.ProcesingCode == "41" || requestModel.ProcesingCode == "42" || requestModel.ProcesingCode == "43" || requestModel.ProcesingCode == "88" || requestModel.ProcesingCode == "89" || requestModel.ProcesingCode == "94")
                        {
                            var Response = MTObject.IMPS(requestModel);
                            responseCode = Response.ResponseCode;
                            if (responseCode == "00")
                            {
                                additionalAmounts = Response.AdditionalAmount;
                                customerNameMobile = Response.CustomerNameMobile;
                                utrRefNo = Response.UTRReferenceNumber;
                            }
                        }

                        else
                        {
                            responseCode = "99";
                        }
                        break;
                    //Reversal Message
                    case "0420":
                        if (requestModel.ProcesingCode == "41" || requestModel.ProcesingCode == "42" || requestModel.ProcesingCode == "43" || requestModel.ProcesingCode == "88" || requestModel.ProcesingCode == "89" || requestModel.ProcesingCode == "94")
                        {
                            var Response = MTObject.IMPS_Reversal(requestModel);
                            responseCode = Response.ResponseCode;
                            if (responseCode == "00")
                            {
                                additionalAmounts = Response.AdditionalAmount;
                                customerNameMobile = Response.CustomerNameMobile;
                            }
                        }
                        break;
                    case "800":
                        break;
                    default:
                        responseCode = "97";
                        break;
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /////////////////////////// APPENDING PROCESSED RESPONSE TO RESPONSE MESSAGE//////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (responseArray[1].Substring(69, 1) == "1")
                responseArray[1] = responseArray[1].Substring(0, 69) + "0" + responseArray[1].Substring(70, 58);
            if (responseCode != null)
            {
                responseArray[39] = responseCode;
                bitMapArray[39 - 1] = "1";

            }
            if (additionalAmounts != null)
            {
                responseArray[54] = Convert.ToString(additionalAmounts.Length).PadLeft(3, '0') + additionalAmounts;
                bitMapArray[54 - 1] = "1";
            }
            if (accountIdentification != null)
            {
                responseArray[102] = Convert.ToString(accountIdentification.Length).PadLeft(2, '0') + accountIdentification;
                bitMapArray[102 - 1] = "1";
            }
            if (miniStatement != null)
            {
                responseArray[121] = Convert.ToString(miniStatement.Length).PadLeft(3, '0') + miniStatement;
                bitMapArray[121 - 1] = "1";
            }           
            if (customerNameMobile != null && customerNameMobile != "")
            {
                responseArray[126] = Convert.ToString(customerNameMobile.Length).PadLeft(3, '0') + customerNameMobile;
                bitMapArray[126 - 1] = "1";
            }
            if (utrRefNo != null && utrRefNo != "")
            {
                responseArray[127] = Convert.ToString(utrRefNo.Length).PadLeft(3, '0') + utrRefNo;
                bitMapArray[127 - 1] = "1";
            }
            responseData.BitmapBinary = bitMapArray;
            responseData.ResponseArray = responseArray;
            return responseData;
        }
        public string BitMapBuilder(string[] binaryArray)
        {
            string bitmapString = string.Join("", binaryArray);
            if (binaryArray[0] == "0")
                bitmapString = bitmapString.Substring(0, 64);
            bitmapString = BinaryToHex(bitmapString);
            return bitmapString;
        }
        public string BinaryToHex(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            // TODO: check all 1's or 0's... Will throw otherwise

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

    }

}
