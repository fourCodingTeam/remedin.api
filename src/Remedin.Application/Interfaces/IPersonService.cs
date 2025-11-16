using Remedin.Application.DTOs.Requests;
using Remedin.Application.DTOs.Responses;
using Remedin.Domain.Entities;

namespace Remedin.Application.Interfaces;

public interface IPersonService
{
    Task<BaseResponse<PersonResponseDTO>> GetOrCreateByUserAsync(RegisterPessoaDto request);
    Task<BaseResponse<Person>> GetCurrentPerson();
}
