using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSLengthCalculator
{
    public interface ISmsService
    {
        Task<SmsResponse> GetSMSPartsAsync(string inputString);
    }
}
