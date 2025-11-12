using Remedin.Application.DTOs.Requests;
using Remedin.Application.DTOs.Responses;

namespace Remedin.Application.Interfaces;

public interface IPersonService
{
    Task<PersonDtoResponse> GetOrCreateByUserAsync(RegisterPessoaDto request);
}
