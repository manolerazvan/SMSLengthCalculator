using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    internal static class AssertHelper
    {
        //public static void AssertIsOk(this IActionResult result)
        //{
        //    var responseObject = Assert.IsType<ObjectResult>(result);
        //    Assert.Equal(200, responseObject.StatusCode);
        //}

        public static void AssertIsOk(this IActionResult result)
        {
            Assert.IsType<OkObjectResult>(result);
        }

        public static void AssertIsInternalServerError(this IActionResult result)
        {
            var responseObject = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, responseObject.StatusCode);
        }

        public static BadRequestObjectResult AssertIsBadRequest(this IActionResult result)
        {
            return Assert.IsType<BadRequestObjectResult>(result);
        }

        public static void AssertIsNotAcceptableError(this IActionResult result)
        {
            var responseObject = Assert.IsType<ObjectResult>(result);
            Assert.Equal(406, responseObject.StatusCode);
        }
    }
}
