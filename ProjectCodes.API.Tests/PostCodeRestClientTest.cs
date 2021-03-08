using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using PostCodes.API.PostCodeAPI;

namespace ProjectCodes.API.Tests
{
    public class PostCodeRestClientTest
    {
        private string _baseUrl;

        [SetUp]
        public void Setup()
        {
            _baseUrl = "https://postcodes.io";
        }

            [Test]
        public async Task TestGetPostCodeData()
        {
          
            var restClient = new PostCodeRestClient(_baseUrl);
            var result = await restClient.GetPostCodeDataAsync("le40bl");
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task GetPostCodeDataBulkAsync()
        {
           
            var restClient = new PostCodeRestClient(_baseUrl);
            var result = await restClient.GetPostCodeDataBulkAsync(new List<string> { "le45ja"});
            Assert.IsNotNull(result);
        }
    }
}
