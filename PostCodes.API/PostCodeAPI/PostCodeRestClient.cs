using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PostCodes.API.PostCodeAPI.Dto;
using PostCodes.API.PostCodeAPI;
using System.Collections.Generic;
using System.Text;

namespace PostCodes.API.PostCodeAPI
{
    public class PostCodeRestClient : IPostCodeRestClient
    {
        private readonly HttpClient _client;

        public PostCodeRestClient(string baseUri)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri) };
        }

        /// <summary>
        /// This method calls GetPostCodeDataBulkAsync to get result
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public async Task<APIPostCodeResponseDto> GetPostCodeDataAsync(string postCode)
        {
            try
            {
                return await GetPostCodeDataBulkAsync(new List<string> { postCode });
            }
            catch (Exception ex)
            {
                //Use logger to log the exception                
                throw new Exception("Error occured while calling external API(" + ex.Message + ")", ex);
            }

        }

        /// <summary>
        /// This method constructs client for external API and returns
        /// results from the API
        /// </summary>
        /// <param name="postcodes"></param>
        /// <returns></returns>
        public async Task<APIPostCodeResponseDto> GetPostCodeDataBulkAsync(IList<string> postcodes)
        {
            try
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(new { postcodes }), Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(
                    $"postcodes?filter=postcode,longitude,latitude", httpContent);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                var postCodeData = JsonConvert.DeserializeObject<APIPostCodeResponseDto>(
                   await response.Content.ReadAsStringAsync());

                return postCodeData;
            }
            catch (Exception ex)
            {
                //Use logger to log the exception                
                throw new Exception("Error occured while calling external API(" + ex.Message + ")", ex);
            }

        }

        public async Task<PostCodeValidationDto> ValidatePostCodeAsync(string postCode)
        {
            try
            {
                var response = await _client.GetAsync($"postcodes/{postCode}/validate");
                return JsonConvert.DeserializeObject<PostCodeValidationDto>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                //Use logger to log the exception                
                throw new Exception("Error occured while calling external validation API(" + ex.Message + ")", ex);
            }

        }
       
    }

}
