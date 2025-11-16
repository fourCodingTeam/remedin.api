using Remedin.Application.DTOs;
using Remedin.Application.DTOs.Requests.Medicine;
using Remedin.Application.DTOs.Responses;
using Remedin.Application.Interfaces;
using Remedin.Domain.Entities;
using Remedin.Domain.Interfaces;

namespace Remedin.Application.Services;

public class MedicineService : IMedicineService
{
    private readonly IMedicineRepository _medicineRepository;

    public MedicineService(IMedicineRepository medicineRepository)
    {
        _medicineRepository = medicineRepository;
    }

    public async Task<BaseResponse<MedicineDtoResponse>> AddMedicineAsync(Guid personId, CreateMedicineRequest request)
    {
        var medicine = new Medicine(
            Guid.NewGuid(),
            personId,
            request.Name,
            request.DosageValue,
            request.DosageUnit,
            request.StartDate,
            request.EndDate,
            request.Observations
        );

        await _medicineRepository.AddAsync(medicine);

        var dto = new MedicineDtoResponse(
            medicine.Id,
            medicine.Name,
            medicine.DosageValue,
            medicine.DosageUnit,
            medicine.StartDate,
            medicine.EndDate,
            medicine.Observations
        );

        return BaseResponse<MedicineDtoResponse>.Ok(dto, "Medicine created successfully");
    }

    public async Task<BaseResponse<PagedResult<MedicineDtoResponse>>> GetAllByPersonAsync(Guid personId, int page = 1, int pageSize = 10)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var (medicines, total) = await _medicineRepository.GetByPersonIdPagedAsync(personId, page, pageSize);

        var list = medicines.Select(m => new MedicineDtoResponse(
            m.Id,
            m.Name,
            m.DosageValue,
            m.DosageUnit,
            m.StartDate,
            m.EndDate,
            m.Observations
        )).ToList();

        var paged = new PagedResult<MedicineDtoResponse>(list, total, page, pageSize);
        return BaseResponse<PagedResult<MedicineDtoResponse>>.Ok(paged, "Medicines fetched successfully");
    }

    public async Task<BaseResponse<MedicineDtoResponse>> UpdateMedicineAsync(Guid personId, UpdateMedicineRequest request)
    {
        var medicine = new Medicine(
            request.Id,
            personId,
            request.Name,
            request.DosageValue,
            request.DosageUnit,
            request.StartDate,
            request.EndDate,
            request.Observations
        );

        await _medicineRepository.UpdateAsync(medicine);

        var dto = new MedicineDtoResponse(
            medicine.Id,
            medicine.Name,
            medicine.DosageValue,
            medicine.DosageUnit,
            medicine.StartDate,
            medicine.EndDate,
            medicine.Observations
        );

        return BaseResponse<MedicineDtoResponse>.Ok(dto, "Medicine updated successfully");
    }

    public async Task<BaseResponse<MedicineDtoResponse?>> GetByIdAsync(Guid personId, Guid id)
    {
        var medicine = await _medicineRepository.GetByIdAsync(id);
        if (medicine == null) return BaseResponse<MedicineDtoResponse?>.Fail(null, "Medicine not found");
        if (medicine.PersonId != personId) return BaseResponse<MedicineDtoResponse?>.Fail(null, "Medicine not found or access denied");

        var dto = new MedicineDtoResponse(
            medicine.Id,
            medicine.Name,
            medicine.DosageValue,
            medicine.DosageUnit,
            medicine.StartDate,
            medicine.EndDate,
            medicine.Observations
        );

        return BaseResponse<MedicineDtoResponse?>.Ok(dto, "Medicine fetched successfully");
    }
}
