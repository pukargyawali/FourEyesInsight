using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PostCodes.API.PostCodeAPI.Dto
{
    public class APIPostCodeResponseDto
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("result")]
        public IList<PostCodeResponseResultDto> ResponseResult { get; set; }

    }
}
