using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PostCodes.API.Cache;
using PostCodes.API.PostCodeAPI;
using PostCodes.API.PostCodeAPI.Dto;
using PostCodes.API.Response;

namespace PostCodes.API.Processor
{
    public class PostCodeProcessor : IPostCodeProcessor
    {
        private readonly IPostCodeRestClient _postCodeApiClient;
        private readonly ICacheStorage _cacheStorage;
        private readonly IConfiguration _configuration;       

        private bool _isCacheActivated => _configuration.GetValue<bool>("IsCacheActivated");

        public PostCodeProcessor(IPostCodeRestClient postCodeRestClient, ICacheStorage cacheStorage, IConfiguration configuration) =>
                                (_postCodeApiClient, _cacheStorage, _configuration) = (postCodeRestClient, cacheStorage, configuration);
        
        /// <summary>
        /// Core Method of the API
        /// This method implements Cache-Aside Design pattern
        /// to retrieve data from redis cache.
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public async Task<PostCodeResponse> GetPostCodeDetailAsync(string postCode)
        {
            //pass postcode through some validator
            try
            {
                var validate = await ValidatePostCodeAsync(postCode);
                if (validate.Result)
                {
                    //check if caching is turned on
                    if (_isCacheActivated)
                    {
                        var result = await _cacheStorage.GetValueAsync<PostCodeDto>(postCode);
                        if (result == null)
                        {
                            //get data from api
                            var response = (await _postCodeApiClient.GetPostCodeDataAsync(postCode))
                                                                    .ResponseResult
                                                                    .FirstOrDefault();
                            // add data to cache
                            await _cacheStorage.AddValueAsync(postCode, response.Result);
                            return MapPostCode(response.Result);
                        }
                        else
                        {
                            return MapPostCode(result);
                        }
                    }
                    else
                    {
                        //get data from api
                        var response = (await _postCodeApiClient.GetPostCodeDataAsync(postCode))
                                                                .ResponseResult
                                                                .FirstOrDefault();
                        return MapPostCode(response.Result);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                //Use logger to log the exception                
                throw new Exception("Error occured while processing your request (" + ex.Message + ")", ex);
            }          
            
           
        }

        /// <summary>
        /// Method to bulk post request to the external API
        /// </summary>
        /// <param name="postCodes"></param>
        /// <returns></returns>
        public async Task<IList<PostCodeResponse>> GetPostCodeDetailCollectionAsync(IList<string> postCodes)
        {
            var result = new List<PostCodeResponse>(); 
            
            foreach (var postCode in postCodes)
            {
                var postCodeDetail = await GetPostCodeDetailAsync(postCode);
                if (postCodeDetail == null)
                {
                    result.Add(new PostCodeResponse
                    {
                        PostCode = $"{postCode} invalid",
                        Coordinates = null
                    });
                }
                else
                {
                    result.Add(await GetPostCodeDetailAsync(postCode));
                }                
            }
            return result;
        }

        /// <summary>
        /// External API is used to validated the postcode provided
        /// It provides more reliable validaition in comparison to regex validations
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        private async Task<PostCodeValidationDto> ValidatePostCodeAsync(string postCode)
        {           
            return await _postCodeApiClient.ValidatePostCodeAsync(postCode);
        }

        /// <summary>
        /// Mapper method
        /// </summary>
        /// <param name="postCodeResult"></param>
        /// <returns></returns>
        private PostCodeResponse MapPostCode(PostCodeDto postCodeResult)
        {
            return new PostCodeResponse
            {
                PostCode = postCodeResult.PostCode,
                Coordinates = new PostCodeCoordinates
                {
                    Latitude = postCodeResult.Latitude,
                    Longitude = postCodeResult.Longitude
                }

            };
        }
    }
}
