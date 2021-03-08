using System;
using System.Text.Json.Serialization;

namespace PostCodes.API.PostCodeAPI.Dto
{
    public class PostCodeValidationDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public bool Result { get; set; }
    }
}
