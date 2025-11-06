using Remedin.Application.DTOs;
using Remedin.Application.Interfaces;
using Remedin.Domain.Entities;
using Remedin.Domain.Interfaces;
using System;

namespace Remedin.Application.Services;

public class PersonService : IPersonService
{
    private readonly IAuthService _authService;
    private readonly IPersonRepository _personRepository;

    public PersonService(IAuthService authService, IPersonRepository personRepository)
    {
        _authService = authService;
        _personRepository = personRepository;
    }

    public async Task<PersonDto> GetOrCreateByUserAsync(string token)
    {
        var user = await _authService.GetCurrentUserAsync(token)
            ?? throw new UnauthorizedAccessException("Invalid or expired token.");

        var person = await _personRepository.GetBySupabaseUserIdAsync(user.Id.ToString());

        if (person == null)
        {
            person = new Person(Guid.NewGuid(), user.Email, user.Email, user.Id.ToString());
            await _personRepository.AddAsync(person);
        }

        return new PersonDto(person.Id, person.Name, person.Email);
    }
}
