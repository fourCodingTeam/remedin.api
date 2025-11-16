using Remedin.Application.DTOs;
using Remedin.Application.DTOs.Requests.Schedule;
using Remedin.Application.DTOs.Responses;

namespace Remedin.Application.Interfaces;

public interface IScheduleService
{
    Task<BaseResponse<ScheduleDtoResponse>> AddScheduleAsync(Guid personId, CreateScheduleRequest request);
    Task<BaseResponse<PagedResult<ScheduleDtoResponse>>> GetAllByPersonAsync(Guid personId, int page = 1, int pageSize = 10);
    Task<BaseResponse<ScheduleDtoResponse?>> GetByIdAsync(Guid personId, Guid id);
    Task<BaseResponse<ScheduleDtoResponse>> UpdateScheduleAsync(Guid personId, UpdateScheduleRequest request);
}