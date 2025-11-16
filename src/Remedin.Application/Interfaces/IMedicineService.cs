using Remedin.Application.DTOs;
using Remedin.Application.DTOs.Requests.Medicine;
using Remedin.Application.DTOs.Responses;

namespace Remedin.Application.Interfaces;

public interface IMedicineService
{
    Task<BaseResponse<MedicineDtoResponse>> AddMedicineAsync(Guid personId, CreateMedicineRequest request);
    Task<BaseResponse<PagedResult<MedicineDtoResponse>>> GetAllByPersonAsync(Guid personId, int page = 1, int pageSize = 10);
    Task<BaseResponse<MedicineDtoResponse>> UpdateMedicineAsync(Guid personId, UpdateMedicineRequest request);
    Task<BaseResponse<MedicineDtoResponse?>> GetByIdAsync(Guid personId, Guid id);
}
