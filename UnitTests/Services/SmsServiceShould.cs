using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SMSLengthCalculator;
using Xunit;

namespace UnitTests
{
    public class SmsServiceShould
    {
        private readonly ILogger<SmsService> _logger = Substitute.For<ILogger<SmsService>>();
        private readonly ISmsService _smsService;

        public const string BASIC_SET = "@£$¥èéùìòÇØøÅåΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ!\"#¤%&'()*+,-./0123456789:;<=> ?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà\r\n";
        public const string EXTENSION_SET = "€[]|^\\~{}";
        
        private static Random random = new Random();

        public SmsServiceShould()
        {
            _smsService = new SmsService(_logger);
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async Task ReturnDesiredResultOnGetSMSPartNo(int expected, string inputString)
        {
            var result = await _smsService.GetSMSPartsAsync(inputString);
            Assert.Equal(expected, result.NoOfParts);
        }

        [Fact]
        public async Task ReturnErrorResultOnGetSMSPartNoWithMoreThan255Parts()
        {
            var inputString = RandomString(39016, BASIC_SET);
            var result = await _smsService.GetSMSPartsAsync(inputString);
            Assert.True(result.HasError);
        }

        [Fact]
        public async Task ReturnOkResultOnGetSMSPartNoWithLessThan255Parts()
        {
            var inputString = RandomString(39015, BASIC_SET);
            var result = await _smsService.GetSMSPartsAsync(inputString);
            Assert.False(result.HasError);
        }

        [Fact]
        public async Task ReturnExceptionOnGetSMSPartNoWithInputNull()
        {
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _smsService.GetSMSPartsAsync(null));
        }
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { 1, RandomString(10, BASIC_SET) };
            yield return new object[] { 1, RandomString(160, BASIC_SET) };
            yield return new object[] { 2, RandomString(161, BASIC_SET) };

            yield return new object[] { 1, RandomString(10, EXTENSION_SET) };
            yield return new object[] { 1, RandomString(80, EXTENSION_SET) };
            yield return new object[] { 2, RandomString(81, EXTENSION_SET) };

            yield return new object[] { 1, RandomEmoji(10) };
            yield return new object[] { 1, RandomEmoji(35) };
            yield return new object[] { 2, RandomEmoji(36) };

            yield return new object[] { 1, $"{RandomString(10, BASIC_SET)}{RandomString(1, EXTENSION_SET)}" };
            yield return new object[] { 1, $"{RandomString(158, BASIC_SET)}{RandomString(1, EXTENSION_SET)}" };
            yield return new object[] { 2, $"{RandomString(159, BASIC_SET)}{RandomString(1, EXTENSION_SET)}" };

            yield return new object[] { 1, $"{RandomString(10, BASIC_SET)}{RandomEmoji(1)}" };
            yield return new object[] { 1, $"{RandomString(68, BASIC_SET)}{RandomEmoji(1)}" };
            yield return new object[] { 2, $"{RandomString(69, BASIC_SET)}{RandomEmoji(1)}" };
            
            yield return new object[] { 1, $"{RandomString(10, EXTENSION_SET)}{RandomEmoji(1)}" };
            yield return new object[] { 1, $"{RandomString(68, EXTENSION_SET)}{RandomEmoji(1)}" };
            yield return new object[] { 2, $"{RandomString(69, EXTENSION_SET)}{RandomEmoji(1)}" };

            yield return new object[] { 1, $"{RandomString(10, BASIC_SET)}{RandomString(10, EXTENSION_SET)}{RandomEmoji(1)}" };
            yield return new object[] { 1, $"{RandomString(67, BASIC_SET)}{RandomString(1, EXTENSION_SET)}{RandomEmoji(1)}" };
            yield return new object[] { 2, $"{RandomString(68, BASIC_SET)}{RandomString(1, EXTENSION_SET)}{RandomEmoji(1)}" };
        }

        public static string RandomString(int length,string fromString)
        {
            return new string(Enumerable.Repeat(fromString, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomEmoji(int length)
        {
            var unicodeBuilder = new StringBuilder();
            var unicodeParts = new string[] { "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😎" };
            for (var i = 0; i < length; i++)
            {
                var r = random.Next(unicodeParts.Length);
                unicodeBuilder.Append(unicodeParts[r]);
            }

            return unicodeBuilder.ToString();
        }
    }
}
