using Remedin.Application.DTOs.Requests.Medicine;
using Remedin.Application.DTOs.Responses;

namespace Remedin.Application.Interfaces;

public interface IMedicineService
{
    Task<MedicineDtoResponse> AddMedicineAsync(Guid personId, CreateMedicineRequest request);
    Task<List<MedicineDtoResponse>> GetAllByPersonAsync(Guid personId);
    Task<MedicineDtoResponse> UpdateMedicineAsync(Guid personId, UpdateMedicineRequest request);
    Task<MedicineDtoResponse?> GetByIdAsync(Guid personId, Guid id);
}
