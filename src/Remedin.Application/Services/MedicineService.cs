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

    public async Task<MedicineDtoResponse> AddMedicineAsync(Guid personId, CreateMedicineRequest request)
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

        return new MedicineDtoResponse(
            medicine.Id,
            medicine.Name,
            medicine.DosageValue,
            medicine.DosageUnit,
            medicine.StartDate,
            medicine.EndDate,
            medicine.Observations
        );
    }

    public async Task<List<MedicineDtoResponse>> GetAllByPersonAsync(Guid personId)
    {
        var medicines = await _medicineRepository.GetByPersonIdAsync(personId);

        return medicines.Select(m => new MedicineDtoResponse(
            m.Id,
            m.Name,
            m.DosageValue,
            m.DosageUnit,
            m.StartDate,
            m.EndDate,
            m.Observations
        )).ToList();
    }

    public async Task<MedicineDtoResponse> UpdateMedicineAsync(Guid personId, UpdateMedicineRequest request)
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

        return new MedicineDtoResponse(
            medicine.Id,
            medicine.Name,
            medicine.DosageValue,
            medicine.DosageUnit,
            medicine.StartDate,
            medicine.EndDate,
            medicine.Observations
        );
    }

    public async Task<MedicineDtoResponse?> GetByIdAsync(Guid personId, Guid id)
    {
        var medicine = await _medicineRepository.GetByIdAsync(id);
        if (medicine == null) return null;
        if (medicine.PersonId != personId) return null;

        return new MedicineDtoResponse(
            medicine.Id,
            medicine.Name,
            medicine.DosageValue,
            medicine.DosageUnit,
            medicine.StartDate,
            medicine.EndDate,
            medicine.Observations
        );
    }
}
