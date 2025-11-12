using Microsoft.AspNetCore.Mvc;
using Remedin.Application.DTOs.Responses;
using Remedin.Application.Interfaces;

namespace Remedin.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<PersonDtoResponse>> GetMe()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var person = await _personService.GetOrCreateByUserAsync(token);
        return Ok(person);
    }
}
