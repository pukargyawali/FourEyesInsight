using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostCodes.API.PostCodeAPI.Dto;
using PostCodes.API.Response;

namespace PostCodes.API.Processor
{
    public interface IPostCodeProcessor
    {
        Task<PostCodeResponse> GetPostCodeDetailAsync(string postCode);

        Task<IList<PostCodeResponse>> GetPostCodeDetailCollectionAsync(IList<string> postCodes);
    }
}
