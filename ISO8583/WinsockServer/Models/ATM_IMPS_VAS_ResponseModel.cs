using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtmImpsServer.Models
{
    class ATM_IMPS_VAS_ResponseModel
    {
        public string ResponseCode { get; set; }
        public string AdditionalAmount { get; set; }
        public string MimiStatement { get; set; }
        public string NUUPdata { get; set; }
        public string CustomerNameMobile { get; set; }
        public string UTRReferenceNumber { get; set; }

    }
}
