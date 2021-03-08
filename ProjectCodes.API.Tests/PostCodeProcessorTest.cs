using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using PostCodes.API.Cache;
using PostCodes.API.PostCodeAPI;
using PostCodes.API.PostCodeAPI.Dto;
using PostCodes.API.Processor;

namespace ProjectCodes.API.Tests
{
    public class PostCodeProcessorTest
    {
        private Mock<IPostCodeRestClient> _restClientMock;
        private Mock<ICacheStorage> _cacheStorageMock;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            var mocRestClient = new Mock<IPostCodeRestClient>();
            var apiResult = new PostCodeDto { PostCode = "se38hn", Latitude = 1, Longitude = 1 };
            var cordinates = new List<PostCodeResponseResultDto> { new PostCodeResponseResultDto { QueryPostCode = "se38hn", Result = apiResult } };

            var postCodeResponse = new APIPostCodeResponseDto
            {
                Status = "200",
                ResponseResult = cordinates
            };

            mocRestClient.Setup(c => c.GetPostCodeDataAsync("se38hn"))
                               .ReturnsAsync(postCodeResponse);
            _restClientMock = mocRestClient;

            var myConfiguration = new Dictionary<string, string>
            {
                {"IsCacheActivated", "true"},                
            };

            _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

            var mocCache = new Mock<ICacheStorage>();


        }
        public async Task TestGetPostCodeDetailAsync()
        {
            var mocRestClient = new Mock<IPostCodeRestClient>();
            var apiResult = new PostCodeDto { PostCode = "se38hn", Latitude = 1, Longitude = 1 };
            var cordinates = new List<PostCodeResponseResultDto> { new PostCodeResponseResultDto { QueryPostCode = "se38hn", Result = apiResult } };

            var postCodeResponse = new APIPostCodeResponseDto
            {
                Status = "200",
                ResponseResult = cordinates
            };

            mocRestClient.Setup(c => c.GetPostCodeDataAsync("se38hn"))
                               .ReturnsAsync(postCodeResponse);
            var postCodeProcessor = new PostCodeProcessor(_restClientMock.Object, _cacheStorageMock.Object, _configuration);
            var result = await postCodeProcessor.GetPostCodeDetailAsync("se38hn");
            Assert.IsNotNull(result);
        }
    }
}