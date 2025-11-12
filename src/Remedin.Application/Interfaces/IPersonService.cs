using Remedin.Application.DTOs;

namespace Remedin.Application.Interfaces;

public interface IPersonService
{
    Task<BaseResponse<PersonResponseDTO>> GetOrCreateByUserAsync(RegisterPessoaDto request);
    Task<BaseResponse<PersonResponseDTO>> GetCurrentPerson();
}
