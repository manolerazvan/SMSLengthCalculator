using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSLengthCalculator
{
    public class SmsResponse
    {
        public int NoOfParts { get; set; }
        public List<string> SmsParts { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; } = false;

        public void SetError(string errorMessage)
        {
            HasError = true;
            ErrorMessage = errorMessage;
        }
    }
}
