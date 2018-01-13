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
    class SplitterSimulator
    {
        Field_validation FieldValidator = new Field_validation();
        //Length_Validation LengthValidator = new Length_Validation();
        ASCIIEncoding ascii = new ASCIIEncoding();
        public string Request_length = null;
        public string primaryBitMap_Binary;
        public Boolean ValidationResponse_boolean;
        public string PrimaryBitMap_Binary
        {
            get { return primaryBitMap_Binary; }
            set { primaryBitMap_Binary = value; }
        }
        
        public string[] RequestSplitter(string Request)
        {
            int j = 0;
            string BitMap_string = null;
            string secondaryBitMap_Binary = null;
            int maxSplittedLength = 0;
            string[] SplittedRequest = new string[128];
            try
            {
                
                ////////////Header Message Length///////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                Request_length = Request.Substring(0, 4);

                //LengthValidators.VariableLengthValidator lengthValid_Header = new LengthValidators.VariableLengthValidator(0, Convert.ToInt32(Request_length));
                //ValidationResponse_boolean = lengthValid_Header.IsValid(Request);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Message Type//////////////////////////////////////////////////////////////////////////////////////////////////////
                SplittedRequest[0] = Request.Substring(4, 4);
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Bit Map///////////////////////////////////////////////////////////////////////////////////////////////////////////  
                ValidationResponse_boolean = true;
                BitMap_string = Request.Substring(8, 16);
                PrimaryBitMap_Binary = hex2binary(BitMap_string);
                if (PrimaryBitMap_Binary.Substring(0, 1) == "1")
                {
                    //SplittedRequest[0] = Request.Substring(4, 32);
                    BitMap_string = BitMap_string + Request.Substring(24, 16);
                    secondaryBitMap_Binary = hex2binary(Request.Substring(24, 16));
                    PrimaryBitMap_Binary = PrimaryBitMap_Binary + secondaryBitMap_Binary;
                    SplittedRequest[1] = PrimaryBitMap_Binary;
                    maxSplittedLength = 40;
                }
                else
                {
                    SplittedRequest[1] = PrimaryBitMap_Binary;
                    maxSplittedLength = 24;
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Remaining Fields

                for (int i = PrimaryBitMap_Binary.IndexOf('1'); i > -1; i = PrimaryBitMap_Binary.IndexOf('1', i + 1))
                {
                    j = i + 1;
                    switch (j)
                    {
                        case 1:
                            SplittedRequest[j] = BitMap_string;
                            break;
                        case 2://Primary Account Number
                            int length_2 = Convert.ToInt16(Request.Substring(maxSplittedLength, 2));
                            maxSplittedLength += 2;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_2);
                            maxSplittedLength += length_2;
                            break;
                        case 3://processing Code
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 6);
                            maxSplittedLength += 6;
                            break;
                        case 4://Amount
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 12);
                            maxSplittedLength += 12;
                            break;
                        case 7://Transmission Date
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 10);
                            maxSplittedLength += 10;
                            break;
                        case 11://System Trace Audit Number
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 6);
                            maxSplittedLength += 6;
                            break;
                        case 12://Local Transmission Time
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 6);
                            maxSplittedLength += 6;
                            break;
                        case 13://Local Transmission date
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 4);
                            maxSplittedLength += 4;
                            break;
                        case 15://settlement Date
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 4);
                            maxSplittedLength += 4;
                            break;
                        case 17://Capture Date
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 4);
                            maxSplittedLength += 4;
                            break;
                        case 18://NPCI added Field
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 4);
                            maxSplittedLength += 4;
                            break;
                        case 24://Function Code
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 3);
                            maxSplittedLength += 3;
                            break;
                        case 25://NPCI added Field
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 2);
                            maxSplittedLength += 2;
                            break;
                        case 28://Fee
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 9);
                            maxSplittedLength += 9;
                            break;
                        case 32://Acquiring Institution Identification Code
                            int length_32 = Convert.ToInt16(Request.Substring(maxSplittedLength, 2));
                            maxSplittedLength += 2;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_32);
                            maxSplittedLength += length_32;
                            break;
                        case 34://Card Number
                            int length_34 = Convert.ToInt16(Request.Substring(maxSplittedLength, 2));
                            maxSplittedLength += 2;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_34);
                            maxSplittedLength += length_34;
                            break;
                        case 35:// Track 2
                            int length_35 = Convert.ToInt16(Request.Substring(maxSplittedLength, 2));
                            maxSplittedLength += 2;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_35);
                            maxSplittedLength += length_35;
                            break;
                        case 37://Retrival Reference Number
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 12);
                            maxSplittedLength += 12;
                            break;
                        case 38://Autherization Identification Response
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 6);
                            maxSplittedLength += 6;
                            break;
                        case 39://Action Code
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 3);
                            maxSplittedLength += 3;
                            break;
                        case 41://Card Accepter Terminal ID (ATM #)
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 8);
                            maxSplittedLength += 8;
                            break;
                        case 42://Card Accepter Terminal ID (IMPS)
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 15);
                            maxSplittedLength += 15;
                            break;
                        case 43://Card Acceptor Name And Location                        
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 40);
                            maxSplittedLength += 40;
                            break;
                        case 46:
                            int length_46 = Convert.ToInt16(Request.Substring(maxSplittedLength, 3));
                            maxSplittedLength += 3;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_46);
                            maxSplittedLength += length_46;
                            break;
                        case 48://
                            int length_48 = Convert.ToInt16(Request.Substring(maxSplittedLength, 3));
                            maxSplittedLength += 3;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_48);
                            maxSplittedLength += length_48;
                            break;
                        case 49://Currency Code
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 3);
                            maxSplittedLength += 3;
                            break;
                        case 50://Currency Code Settlement
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 3);
                            maxSplittedLength += 3;
                            break;
                        case 51://Currency Code Card Holder Billing
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 3);
                            maxSplittedLength += 3;
                            break;
                        case 54://Additional Amounts
                            int length_54 = Convert.ToInt16(Request.Substring(maxSplittedLength, 3));
                            maxSplittedLength += 3;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_54);
                            maxSplittedLength += length_54;
                            break;
                        case 56:
                            int length_56 = Convert.ToInt16(Request.Substring(maxSplittedLength, 2));
                            maxSplittedLength += 2;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_56);
                            maxSplittedLength += length_56;
                            break;
                        case 59://Tranport Data
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 10);
                            maxSplittedLength += 10;
                            break;
                        case 60://Reserved Private
                            int length_60 = Convert.ToInt16(Request.Substring(maxSplittedLength, 3));
                            maxSplittedLength += 3;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_60);
                            maxSplittedLength += length_60;
                            break;
                        case 70://sign in, sign off n/w code
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 3);
                            maxSplittedLength += 3;
                            break;
                        case 89://sign in, sign off n/w code
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 2);
                            maxSplittedLength += 2;
                            break;
                        case 90://Original Data Elements(for cash withdrawal reverse)
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 42);
                            maxSplittedLength += 42;
                            break;
                        case 95://Partial reversal actual dispensed amount
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, 42);
                            maxSplittedLength += 42;
                            break;
                        case 102://Account Number identification
                            int length_102 = Convert.ToInt16(Request.Substring(maxSplittedLength, 2));
                            maxSplittedLength += 2;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_102);
                            maxSplittedLength += length_102;
                            break;
                        case 103:
                            int length_103 = Convert.ToInt16(Request.Substring(maxSplittedLength, 2));
                            maxSplittedLength += 2;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_103);
                            maxSplittedLength += length_103;
                            break;
                        case 123://Channel (SWT)
                            int length_123 = Convert.ToInt16(Request.Substring(maxSplittedLength, 3));
                            maxSplittedLength += 3;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_123);
                            maxSplittedLength += length_123;
                            break;
                        case 124://Channel
                            int length_124 = Convert.ToInt16(Request.Substring(maxSplittedLength, 3));
                            maxSplittedLength += 3;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_124);
                            maxSplittedLength += length_124;
                            break;
                        case 125://IMPS Data
                            int length_125 = Convert.ToInt16(Request.Substring(maxSplittedLength, 3));
                            maxSplittedLength += 3;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_125);
                            maxSplittedLength += length_125;
                            break;
                        case 127://NUUP Data
                            int length_127 = Convert.ToInt16(Request.Substring(maxSplittedLength, 3));
                            maxSplittedLength += 3;
                            SplittedRequest[j] = Request.Substring(maxSplittedLength, length_127);
                            maxSplittedLength += length_127;
                            break;
                        default:
                            if (this.ValidationResponse_boolean == true)
                                this.ValidationResponse_boolean = false;
                            //responseCode = mFields.ResponseCode("suspect_fraud");
                            break;
                    }
                    if (this.ValidationResponse_boolean == true && SplittedRequest[j] != null)
                        this.ValidationResponse_boolean = true;//FieldValidator.ValidationSelector(SplittedRequest[j], j);
                }
                SplittedRequest[1] = PrimaryBitMap_Binary;
                return SplittedRequest;

            }
            catch (Exception ex)
            {
                return null;
                //Console.WriteLine(ex);
            }


        }
        public string hex2binary(string hexvalue)
        {
            return String.Join(String.Empty, hexvalue.Select(c => Convert.ToString(Convert.ToUInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }
    }
}
