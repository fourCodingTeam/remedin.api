using Remedin.Application.DTOs;

namespace Remedin.Application.Interfaces;

public interface IPersonService
{
    Task<PersonDto> GetOrCreateByUserAsync(string token);
}
