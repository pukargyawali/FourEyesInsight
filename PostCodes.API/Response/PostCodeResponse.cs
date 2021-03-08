using System;
using Newtonsoft.Json;
using PostCodes.API.PostCodeAPI.Dto;

namespace PostCodes.API.Response
{
    public class PostCodeResponse
    {
        [JsonProperty("postcode")]
        public string PostCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]       
        public PostCodeCoordinates Coordinates { get; set; }
    }
}
