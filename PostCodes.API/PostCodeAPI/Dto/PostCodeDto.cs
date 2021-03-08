using Newtonsoft.Json;

namespace PostCodes.API.PostCodeAPI.Dto
{
    public class PostCodeResponseResultDto
    {
        [JsonProperty("query")]
        public string QueryPostCode { get; set; }

        [JsonProperty("result")]
        public PostCodeDto Result { get; set; }
    }

    public class PostCodeDto
    {
        [JsonProperty("postcode")]
        public string PostCode { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
