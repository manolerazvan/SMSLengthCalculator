using NSubstitute;
using SMSLengthCalculator;
using SMSLengthCalculator.Controllers;
using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;

namespace UnitTests
{
    public class SMSControllerShould
    {

        private readonly ISmsService _smsService;
        private readonly SMSController _smsController;

        public SMSControllerShould()
        {
            _smsService = Substitute.For<ISmsService>();
            _smsController = new SMSController(_smsService);
        }

        [Fact]
        public async Task ReturnOkOnGetSMSPartNoIfSmsServiceSuccessful()
        {
            var smsServiceResponse = new SmsResponse();
            _smsService.GetSMSPartsAsync(string.Empty).ReturnsForAnyArgs(smsServiceResponse);
            var response = await _smsController.GetSMSPartsAsync(string.Empty);
            response.AssertIsOk();
        }

        [Fact]
        public async Task ReturnInternalServerErrorOnGetSMSPartNoIfSmsServiceThrowsException()
        {
            _smsService.WhenForAnyArgs(p => p.GetSMSPartsAsync(string.Empty)).Do(r => throw new Exception());
            var response = await _smsController.GetSMSPartsAsync(string.Empty);
            response.AssertIsInternalServerError();
        }

        [Fact]
        public async Task Return406OnGetSMSPartNoIfSmsServiceSuccessfulButExceedsLimit()
        {
            var smsServiceResponse = new SmsResponse();
            smsServiceResponse.SetError(string.Empty);
            _smsService.GetSMSPartsAsync(string.Empty).ReturnsForAnyArgs(smsServiceResponse);
            var response = await _smsController.GetSMSPartsAsync(string.Empty);
            response.AssertIsNotAcceptableError();
        }
    }
}
