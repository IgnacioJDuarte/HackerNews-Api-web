using HackerNews_Web.Services;
using NSubstitute;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NSubstitute.ExceptionExtensions;

namespace HackerNews_web_tests
{
    public class HackerNewsConsumerServiceTests
    {
        [Fact]
        public async Task GetStories_ShouldFail_UrlDoesntProvided()
        {
            var logger = Substitute.For<ILogger<HackerNewsConsumerService>>();
            var configuration = Substitute.For<IConfiguration>();
            configuration.GetValue<string>("HackerNewsUrl").Returns("InvaildUrl");
            var hackerNewsConsumerService = new HackerNewsConsumerService(logger, configuration);
            Assert.ThrowsAsync<ArgumentException>(hackerNewsConsumerService.GetStories);
        }
    }
}
