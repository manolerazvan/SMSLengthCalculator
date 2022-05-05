using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMSLengthCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SMSController : ControllerBase
    {
        private readonly ISmsService _smsService;
        public SMSController(ISmsService smsService)
        {
            _smsService = smsService;
        }
        [HttpPost]
        [ProducesResponseType(typeof(SmsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSMSPartsAsync([FromBody] string inputString)
        {
            try
            {
                var ret = await _smsService.GetSMSPartsAsync(inputString);
                if (ret.HasError)
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, ret.ErrorMessage);
                }
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }
    }
}
