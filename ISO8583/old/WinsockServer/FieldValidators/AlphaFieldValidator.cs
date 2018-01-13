﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmImpsServer.FieldValidators
{
    class AlphaFieldValidator : IFieldValidator
    {
        #region IFieldValidator Members

        /// <summary>
        ///   Description of the validator
        /// </summary>
        public string Description
        {
            get { return "a"; }
        }

        /// <summary>
        ///   Validates the format of the given string value
        /// </summary>
        /// <param name = "value">Value to validate</param>
        /// <returns>true if valid, false otherwise</returns>
        public bool IsValid(byte[] value)
        {
            foreach (int b in value)
            {
                if (b < 65 || b > 90 && b < 97 || b > 122)
                    return false;
            }
            return true;
        }

        #endregion
    }
}