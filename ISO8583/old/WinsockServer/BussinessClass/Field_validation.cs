using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmImpsServer
{
    class Field_validation
    {
        ASCIIEncoding ascii = new ASCIIEncoding();
        FieldValidators.NumericFieldValidator Numeric = new FieldValidators.NumericFieldValidator();
        FieldValidators.AlphaFieldValidator Alpha = new FieldValidators.AlphaFieldValidator();
        FieldValidators.AlphaNumericAndSpaceFieldValidator AlphaNumericSpace = new FieldValidators.AlphaNumericAndSpaceFieldValidator();
        FieldValidators.AlphaNumericSpecialFieldValidator AlphaNumericSpecial = new FieldValidators.AlphaNumericSpecialFieldValidator();
        FieldValidators.HexFieldValidator HexField = new FieldValidators.HexFieldValidator();
        string ValidationResponse = null;
        public string test(string field)
        {
            string responseCode = "00";
            return responseCode;
        }
        public Boolean ValidationSelector(string value, int field_no)
        {
            Byte[] encodedBytes = ascii.GetBytes(value);
            Boolean valitity = true;
            if (new int[] { 1 }.Contains(field_no))
            {
                valitity = HexField.IsValid(encodedBytes);
            }
            else if (new int[] { 2, 3, 4, 12, 17, 24, 30, 32, 39, 56 }.Contains(field_no))
            {
                valitity = true; //Numeric.IsValid(encodedBytes);
            }
            else if (new int[] { 11, 37 }.Contains(field_no))
            {
                valitity = AlphaNumericSpace.IsValid(encodedBytes);
            }
            else if (new int[] { 41, 42, 43, 46, 48, 59, 102, 103, 125, 126 }.Contains(field_no))
            {
                valitity = AlphaNumericSpecial.IsValid(encodedBytes);
            }
            return valitity;
        }

    }
}