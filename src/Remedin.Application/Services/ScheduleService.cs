using Remedin.Application.DTOs;
using Remedin.Application.DTOs.Requests.Schedule;
using Remedin.Application.DTOs.Responses;
using Remedin.Application.Interfaces;
using Remedin.Domain.Entities;
using Remedin.Domain.Interfaces;

namespace Remedin.Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IMedicineRepository _medicineRepository;

    public ScheduleService(IScheduleRepository scheduleRepository, IMedicineRepository medicineRepository)
    {
        _scheduleRepository = scheduleRepository;
        _medicineRepository = medicineRepository;
    }

    public async Task<BaseResponse<ScheduleDtoResponse>> AddScheduleAsync(Guid personId, CreateScheduleRequest request)
    {
        var medicine = await _medicineRepository.GetByIdAsync(request.MedicineId)
            ?? throw new InvalidOperationException("Medicine not found");

        if (medicine.PersonId != personId)
            throw new UnauthorizedAccessException("User does not own the medicine");

        var schedule = new Schedule(
            Guid.NewGuid(),
            request.MedicineId,
            request.ScheduledTime,
            request.FrequencyType,
            request.PreAlarmMinutes,
            request.PosAlarmMinutes
        );

        if (request.WeekDays != null)
        {
            foreach (var day in request.WeekDays)
            {
                schedule.WeekDays.Add(new ScheduleWeekDay(schedule.Id, day));
            }
        }

        await _scheduleRepository.AddAsync(schedule);

        var dto = new ScheduleDtoResponse(
            schedule.Id,
            schedule.MedicineId,
            schedule.ScheduledTime,
            schedule.FrequencyType,
            schedule.PreAlarmMinutes,
            schedule.PosAlarmMinutes,
            schedule.WeekDays.Select(w => w.DayOfWeek).ToList()
        );

        return BaseResponse<ScheduleDtoResponse>.Ok("Schedule created successfully", dto);
    }

    public async Task<BaseResponse<PagedResult<ScheduleDtoResponse>>> GetAllByPersonAsync(Guid personId, int page = 1, int pageSize = 10)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var (schedules, total) = await _scheduleRepository.GetByPersonIdPagedAsync(personId, page, pageSize);

        var list = schedules.Select(s => new ScheduleDtoResponse(
            s.Id,
            s.MedicineId,
            s.ScheduledTime,
            s.FrequencyType,
            s.PreAlarmMinutes,
            s.PosAlarmMinutes,
            s.WeekDays.Select(w => w.DayOfWeek).ToList()
        )).ToList();

        var paged = new PagedResult<ScheduleDtoResponse>(list, total, page, pageSize);
        return BaseResponse<PagedResult<ScheduleDtoResponse>>.Ok("Schedules fetched successfully", paged);
    }

    public async Task<BaseResponse<ScheduleDtoResponse?>> GetByIdAsync(Guid personId, Guid id)
    {
        var schedule = await _scheduleRepository.GetByIdAsync(id);
        if (schedule == null) return BaseResponse<ScheduleDtoResponse?>.Fail("Schedule not found");
        if (schedule.Medicine == null || schedule.Medicine.PersonId != personId)
            return BaseResponse<ScheduleDtoResponse?>.Fail("Schedule not found or access denied");

        var dto = new ScheduleDtoResponse(
            schedule.Id,
            schedule.MedicineId,
            schedule.ScheduledTime,
            schedule.FrequencyType,
            schedule.PreAlarmMinutes,
            schedule.PosAlarmMinutes,
            schedule.WeekDays.Select(w => w.DayOfWeek).ToList()
        );

        return BaseResponse<ScheduleDtoResponse?>.Ok("Schedule fetched successfully", dto);
    }

    public async Task<BaseResponse<ScheduleDtoResponse>> UpdateScheduleAsync(Guid personId, UpdateScheduleRequest request)
    {
        var existing = await _scheduleRepository.GetByIdAsync(request.Id)
            ?? throw new InvalidOperationException("Schedule not found");

        if (existing.Medicine == null || existing.Medicine.PersonId != personId)
            throw new UnauthorizedAccessException("User does not own the schedule");

        if (request.MedicineId != existing.MedicineId)
        {
            var newMed = await _medicineRepository.GetByIdAsync(request.MedicineId)
                ?? throw new InvalidOperationException("Target medicine not found");

            if (newMed.PersonId != personId)
                throw new UnauthorizedAccessException("User does not own the target medicine");
        }

        existing.UpdateBaseData(
            request.MedicineId,
            request.ScheduledTime,
            request.FrequencyType,
            request.PreAlarmMinutes,
            request.PosAlarmMinutes
        );

        existing.WeekDays.Clear();

        if (request.WeekDays != null)
        {
            foreach (var day in request.WeekDays)
            {
                existing.WeekDays.Add(new ScheduleWeekDay(existing.Id, day));
            }
        }

        await _scheduleRepository.UpdateAsync(existing);

        var dto = new ScheduleDtoResponse(
            existing.Id,
            existing.MedicineId,
            existing.ScheduledTime,
            existing.FrequencyType,
            existing.PreAlarmMinutes,
            existing.PosAlarmMinutes,
            existing.WeekDays.Select(w => w.DayOfWeek).ToList()
        );

        return BaseResponse<ScheduleDtoResponse>.Ok("Schedule updated successfully", dto);
    }
}