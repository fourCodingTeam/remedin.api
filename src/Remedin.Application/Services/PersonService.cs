using Remedin.Application.DTOs.Requests;
using Remedin.Application.DTOs.Responses;
using Remedin.Application.Interfaces;
using Remedin.Domain.Entities;
using Remedin.Domain.Interfaces;
using System;

namespace Remedin.Application.Services;

public class PersonService : IPersonService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IPersonRepository _personRepository;

    public PersonService(ICurrentUserService currentUser, IPersonRepository personRepository)
    {
        _currentUser = currentUser;
        _personRepository = personRepository;
    }

    public async Task<PersonDtoResponse> GetOrCreateByUserAsync(RegisterPessoaDto request)
    {
        var supabaseUserId = _currentUser.SupabaseUserId
            ?? throw new UnauthorizedAccessException("Invalid or expired token.");

        var person = await _personRepository.GetBySupabaseUserIdAsync(supabaseUserId);

        if (person is null)
        {
            person = new Person(
                supabaseUserId,
                request.Email, 
                request.Name,
                request.BirthDate,
                request.Phone,
                request.WeightKg,
                request.HeightCm
            );
            await _personRepository.AddAsync(person);
        }

        return new PersonDtoResponse(person.Id, person.Name, person.Email);
    }

    public async Task<PersonDtoResponse> GetCurrentUser()
    {
        var supabaseUserId = _currentUser.SupabaseUserId
            ?? throw new InvalidOperationException("Usuário não autenticado.");

        var person = await _personRepository.GetBySupabaseUserIdAsync(supabaseUserId)
            ?? throw new InvalidOperationException("Pessoa não encontrada.");

        return new PersonDtoResponse(person.Id, person.Name, person.Email);
    }
}
