using System.Collections.Generic;
using System.Threading.Tasks;
using PostCodes.API.PostCodeAPI.Dto;

namespace PostCodes.API.PostCodeAPI
{
    public interface IPostCodeRestClient
    {
        Task<APIPostCodeResponseDto> GetPostCodeDataAsync(string postCode);

        Task<APIPostCodeResponseDto> GetPostCodeDataBulkAsync(IList<string> postcodes);

        Task<PostCodeValidationDto> ValidatePostCodeAsync(string postCode);
    }
}
