using System;
using Newtonsoft.Json;

namespace PostCodes.API.PostCodeAPI.Dto
{
    public class PostCodeCoordinates
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longtitude")]
        public double Longitude { get; set; }
    }
}
