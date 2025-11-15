using Microsoft.AspNetCore.Mvc;
using Remedin.Application.DTOs.Requests.Medicine;
using Remedin.Application.DTOs.Responses;
using Remedin.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class MedicineController : ControllerBase
{
    private readonly IMedicineService _medicineService;
    private readonly IPersonService _personService;

    public MedicineController(
        IMedicineService medicineService,
        IPersonService personService
        )
    {
        _medicineService = medicineService;
        _personService = personService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MedicineDtoResponse>>> GetAll()
    {
        var person = await _personService.GetCurrentPerson();
        if (person == null || person.Data == null)
            return Unauthorized("User is not authenticated.");

        var medicines = await _medicineService.GetAllByPersonAsync(person.Data.Id);
        return Ok(medicines);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MedicineDtoResponse>> GetById(Guid id)
    {
        var person = await _personService.GetCurrentPerson();
        if (person == null || person.Data == null)
            return Unauthorized("User is not authenticated.");

        var medicine = await _medicineService.GetByIdAsync(person.Data.Id, id);
        if (medicine == null)
            return NotFound();

        return Ok(medicine);
    }

    [HttpPost]
    public async Task<ActionResult<MedicineDtoResponse>> Create([FromBody] CreateMedicineRequest request)
    {
        var person = await _personService.GetCurrentPerson();
        if (person == null || person.Data == null)
            return Unauthorized("User is not authenticated.");

        var created = await _medicineService.AddMedicineAsync(person.Data.Id, request); 
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MedicineDtoResponse>> Update(Guid id, [FromBody] UpdateMedicineRequest request)
    {
        if (id != request.Id)
            return BadRequest("The ID in the URL does not match the ID in the request body.");

        var person = await _personService.GetCurrentPerson();
        if (person == null || person.Data == null)
            return Unauthorized("User is not authenticated.");

        var updated = await _medicineService.UpdateMedicineAsync(person.Data.Id, request);
        return Ok(updated);
    }
}
