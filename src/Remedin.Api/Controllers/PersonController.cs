using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Remedin.Application.DTOs;
using Remedin.Application.Interfaces;
using Supabase.Functions.Responses;

namespace Remedin.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponse>> Register([FromBody] RegisterPessoaDto request)
    {
        var result = await _personService.GetOrCreateByUserAsync(request);
        return Ok(result);
    }

    [HttpGet("Me")]
    public async Task<ActionResult<BaseResponse<PersonResponseDTO>>> GetCurrentPerson()
    {
        var response = await _personService.GetCurrentPerson();
        return Ok(response);
    }
}
