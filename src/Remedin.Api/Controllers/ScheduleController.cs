using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remedin.Application.DTOs;
using Remedin.Application.DTOs.Requests.Schedule;
using Remedin.Application.DTOs.Responses;
using Remedin.Application.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;
    private readonly IPersonService _personService;

    public ScheduleController(IScheduleService scheduleService, IPersonService personService)
    {
        _scheduleService = scheduleService;
        _personService = personService;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<PagedResult<ScheduleDtoResponse>>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var person = await _personService.GetCurrentPerson();
        if (person == null || person.Data == null)
            return Unauthorized("User is not authenticated.");

        var response = await _scheduleService.GetAllByPersonAsync(person.Data.Id, page, pageSize);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ScheduleDtoResponse?>>> GetById(Guid id)
    {
        var person = await _personService.GetCurrentPerson();
        if (person == null || person.Data == null)
            return Unauthorized("User is not authenticated.");

        var schedule = await _scheduleService.GetByIdAsync(person.Data.Id, id);
        if (schedule == null || schedule.Data == null)
            return NotFound();

        return Ok(schedule);
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse<ScheduleDtoResponse>>> Create([FromBody] CreateScheduleRequest request)
    {
        var person = await _personService.GetCurrentPerson();
        if (person == null || person.Data == null)
            return Unauthorized("User is not authenticated.");

        var created = await _scheduleService.AddScheduleAsync(person.Data.Id, request);
        return CreatedAtAction(nameof(GetById), new { id = created.Data?.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BaseResponse<ScheduleDtoResponse>>> Update(Guid id, [FromBody] UpdateScheduleRequest request)
    {
        if (id != request.Id)
            return BadRequest("The ID in the URL does not match the ID in the request body.");

        var person = await _personService.GetCurrentPerson();
        if (person == null || person.Data == null)
            return Unauthorized("User is not authenticated.");

        var updated = await _scheduleService.UpdateScheduleAsync(person.Data.Id, request);
        return Ok(updated);
    }
}