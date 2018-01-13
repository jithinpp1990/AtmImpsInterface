using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmImpsServer.FieldValidators
{
    interface IFieldValidator
    {
        /// <summary>
        ///   Description of the validator
        /// </summary>
        string Description { get; }

        /// <summary>
        ///   Validates the format of the given string value
        /// </summary>
        /// <param name = "value">Value to validate</param>
        /// <returns>true if valid, false otherwise</returns>
        bool IsValid(byte[] value);
    }
}
