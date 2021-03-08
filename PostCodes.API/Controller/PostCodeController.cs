using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostCodes.API.PostCodeAPI.Dto;
using PostCodes.API.Processor;
using PostCodes.API.Response;

namespace PostCodes.API.Controller
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostCodeController : ControllerBase
    {
        private readonly IPostCodeProcessor _postCodeProcessor;

        public PostCodeController(IPostCodeProcessor postCodeProcessor) => (_postCodeProcessor) = (postCodeProcessor);

        /// <summary>
        /// This Enpoint returns Post Code detail for one postcode
        /// passed into it
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        [Route("GetPostCode")]
        [HttpGet]
        [ProducesResponseType(typeof(PostCodeResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<PostCodeDto>> GetPostCode(string postCode)
        {
            
            if (string.IsNullOrEmpty(postCode))            
                return UnprocessableEntity("The postcode is mandatory");         

            var response = await _postCodeProcessor.GetPostCodeDetailAsync(postCode);

            if (response == null)
                return NotFound("You've entered an invalid postcode");            

            return Ok(response);
        }


        /// <summary>
        /// This Enpoints returns Post Code detail for a list of postcode
        /// provided to it
        /// </summary>
        /// <param name="postCodes"></param>
        /// <returns></returns>
        [Route("PostCodes")]
        [HttpPost]
        [ProducesResponseType(typeof(IList<PostCodeResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<IList<PostCodeDto>>> GetPostCodeBulk([FromBody] IList<string> postCodes)
        {

            if (postCodes == null)
                return UnprocessableEntity("The postcode is mandatory");

            var response = await _postCodeProcessor.GetPostCodeDetailCollectionAsync(postCodes);

            if (response == null)
                return NotFound("You've entered an invalid postcode");

            return Ok(response);
        }
    }
}
