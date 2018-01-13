using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmImpsServer
{
    public class Message_Fields
    {
        public string messageType(string messageType)
        {
            string R_messageType=null;
            switch (messageType)
            {
                case "0800":
                    R_messageType = "0810";                    
                    break;
                case "0220":
                    R_messageType = "0230";
                    break;
                case "0420":
                    R_messageType = "0430";
                    break;
                case "0804":
                    R_messageType = "0814";
                    break;
                case "0200":
                    R_messageType = "0210";
                    break;
                case "0810":
                    R_messageType = "0810";
                    break;
                default:
                    R_messageType = "Invalid Message Type";
                    break;

            }
            return R_messageType;
        }
        //public string B0(string primaryBitMap)
        //{
        //    string R_primaryBitmap=primaryBitMap;
        //    return R_primaryBitmap;
        //}
        //public string B1(string secondaryBitMap)
        //{
        //    string R_secondaryBitmap = secondaryBitMap;
        //    return R_secondaryBitmap;
        //}
        //public string B2(string primaryAccountNumber)
        //{
        //    string R_primaryAccountNumber = primaryAccountNumber;
        //    return R_primaryAccountNumber;
        //}
        public string B3(string processingCode)
        {
            string R_processingCode = null;
            switch (processingCode)
            {
                case "00":
                    R_processingCode = processingCode;
                    break;
                case "01":
                    R_processingCode = processingCode;
                    break;
                case "30":
                    R_processingCode = processingCode;
                    break;
                case "38":
                    R_processingCode = processingCode;
                    break;
                case "40":
                    R_processingCode = processingCode;
                    break;
                case "31":
                    R_processingCode = processingCode;
                    break;
                case "32":
                    R_processingCode = processingCode;
                    break;
                case "21":
                    R_processingCode = processingCode;
                    break;
                case "24":
                    R_processingCode = processingCode;
                    break;
                case "90":
                    R_processingCode = processingCode;
                    break;
                case "92":
                    R_processingCode = processingCode;
                    break;
                default:
                    R_processingCode = "Invalid Processing code";
                    break;
            }
            return R_processingCode;
        }
        //public string B4(string Amount)
        //{
        //    string R_Amount=null;
        //    R_Amount = Amount;
        //    return R_Amount;
        //}
        //public string B11(string systemTraceAuditNumber)
        //{
        //    string R_systemTraceAuditNumber=systemTraceAuditNumber;
        //    return R_systemTraceAuditNumber;
        //}
        //public string B12(string localTransmissionTime)
        //{
        //    string R_localTransmissionTime=localTransmissionTime;
        //    return R_localTransmissionTime;
        //}
        public string B24(string functionCode)
        {
            string R_functionCode=null;
            switch (functionCode)
            {
                case "200":
                    R_functionCode = "Original Authentication Request or Advice";
                    break;
                case "400":
                    R_functionCode = "Reversal";
                    break;
                case "801":
                    R_functionCode = "LogOn";
                    break;
                case "802":
                    R_functionCode = "logOff";
                    break;
                case "831":
                    R_functionCode = "831";
                    break;
                default:
                    R_functionCode = "Invalid Function Code";
                    break;
            }
            return R_functionCode;
        }
        public string B39(string actionCode)
        {
            string R_actionCode="800";
            return R_actionCode;
        }
        public string B59(string trnsportData)
        {
            string R_trnsportData=trnsportData;
            return R_trnsportData;
        }
        public string S123(string DeliveryChannelContrillerId)
        {
            string R_deliveryChannelControllerId = DeliveryChannelContrillerId;
            return R_deliveryChannelControllerId;
        }
        public string ResponseCode(string data)
        {
            string ResponseCode = "00";
            switch (data)
            {
                case "approved":
                    ResponseCode = "00";
                    break;
                case "refer_to_issuer":
                    ResponseCode = "01";
                    break;
                case "pick_up":
                    ResponseCode = "04";
                    break;
                case "do_not_honor":
                    ResponseCode = "05";
                    break;
                case "error_occured":
                    ResponseCode = "06";
                    break;
                case "invalid_transaction_type":
                    ResponseCode = "12";
                    break;
                case "invalid_amount_requested":
                    ResponseCode = "13";
                    break;
                case "invalid_card_holder":
                       ResponseCode = "14";
                    break; 
                case "card_does_not_exist":
                     ResponseCode = "15";
                    break;
                case "card_does_not_exist1":
                     ResponseCode = "25";
                    break;
                case "formate_error":
                     ResponseCode = "30";
                    break;
                case "invalid_bin":
                     ResponseCode = "31";
                    break;
                case "suspect_fraud":
                     ResponseCode = "34";
                    break;
                case "restricted_card1":
                     ResponseCode = "36";
                    break;
                case "restricted_transaction":
                     ResponseCode = "40";
                    break;
                case "lost_card":
                     ResponseCode = "41";
                    break;
                case "account_type_invalid":
                     ResponseCode = "42";
                    break;
                case "stolen_card":
                     ResponseCode = "43";
                    break;
                case "amount_requested_exceeds_balance":
                     ResponseCode = "51";
                    break;
                case "current_account_does_not_exist":
                     ResponseCode = "52";
                    break;
                case "savings_account_does_not_exist":
                     ResponseCode = "53";
                    break;
                case "expired_card":
                     ResponseCode = "54";
                    break;
                case "transaction_not_permitted_to_card_holder":
                     ResponseCode = "57";
                    break;
                case "exceeds withdrawal_limit":
                     ResponseCode = "61";
                    break;
                case "restricted_card":
                     ResponseCode = "62";
                    break;
                case "security_key_violation":
                     ResponseCode = "63";
                    break;
                case "freequency_limit":
                     ResponseCode = "65";
                    break;
                case "excessive_pin_tries":
                     ResponseCode = "75";
                    break;
                case "invalid_autherization_code":
                     ResponseCode = "88";
                    break;
                case "host_unavailable":
                     ResponseCode = "91";
                    break;
                case "autherizer_not_found":
                     ResponseCode = "92";
                    break;
                case "duplicate_transaction":
                     ResponseCode = "94";
                    break;
                case "account_closed_imps":
                    ResponseCode = "M5";
                    break;
                case "frozen_account_imps":
                    ResponseCode = "M3";
                    break;
                case "invalid_account_imps":
                    ResponseCode = "M1";
                    break;
                case "transaction_rejected":
                    ResponseCode = "99";
                    break;
                case "transaction_decliened_imps":
                    ResponseCode = "02";
                    break;
                default:
                     ResponseCode = "99";
                    break;
            }
            return ResponseCode;
        }

    }
}
