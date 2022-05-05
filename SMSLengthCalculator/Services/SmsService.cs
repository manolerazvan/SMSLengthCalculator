using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSLengthCalculator
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;
      
        // ESC for the extended GSM chars
        public const int ESC_INT = 27;
        // Basic set + " "
        public const string BASIC_SET = "@£$¥èéùìòÇØøÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ!\"#¤%&'()*+,-./0123456789:;<=> ?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà\r\n";
        //Extension set
        public const string EXTENSION_SET = "€[]|^\\~{}";
       
        public SmsService(ILogger<SmsService> logger)
        {
            _logger = logger;
        }

        public async Task<SmsResponse> GetSMSPartsAsync(string inputString)
        {
            try
            {
                var ret = new SmsResponse();

                var inputSmsCharList = new List<char>();
                var isGMS7Only = true;
                foreach (var c in inputString)
                {
                    if (BASIC_SET.Any(e => e == c))
                    {
                        inputSmsCharList.Add(c);
                        continue;
                    }

                    if (EXTENSION_SET.Any(e => e == c))
                    {
                        inputSmsCharList.Add((char)ESC_INT);
                        inputSmsCharList.Add(c);
                        continue;
                    }

                    inputSmsCharList.Add(c);
                    isGMS7Only = false;
                }

                var partSize = 160;
                if (isGMS7Only)
                {
                    if (inputSmsCharList.Count <= 160)
                    {
                        ret.NoOfParts = 1;
                        partSize = 160;
                    } 
                    else
                    {
                        ret.NoOfParts = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(inputSmsCharList.Count) / 153));
                        partSize = 153;
                    }
                }
                else
                {
                    inputSmsCharList.RemoveAll(e => e == (char)ESC_INT);
                    if (inputSmsCharList.Count <= 70)
                    {
                        ret.NoOfParts = 1;
                        partSize = 70;
                    }
                    else
                    {
                        ret.NoOfParts = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(inputSmsCharList.Count) / 67));
                        partSize = 67;
                    }
                }
                
                if (ret.NoOfParts > 255)
                {
                    ret.SetError("Max no of SMS parts exceeded!");
                } 
                else
                {
                    ret.SmsParts = FormSmsParts(inputSmsCharList, partSize);
                }

                return await Task.FromResult(ret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        private List<string> FormSmsParts(List<char> source, int size)
        {
            try
            {
                var ret = new List<string>();
                var skip = 0;
                while (skip < source.Count)
                {
                    ret.Add(String.Join("", source.Skip(skip).Take(size)));
                    skip += size;
                }
                return ret;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
