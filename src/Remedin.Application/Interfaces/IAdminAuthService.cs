using Remedin.Application.DTOs.Responses;

namespace Remedin.Application.Interfaces;

public interface IAdminAuthService
{
    Task<BaseResponse<bool>> DeleteUserAsync(string userId);
}
