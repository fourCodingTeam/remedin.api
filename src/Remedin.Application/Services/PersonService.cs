using Remedin.Application.DTOs;
using Remedin.Application.Interfaces;
using Remedin.Domain.Entities;
using Remedin.Domain.Interfaces;

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

    public async Task<BaseResponse<PersonResponseDTO>> GetOrCreateByUserAsync(RegisterPessoaDto request)
    {
        var supabaseUserId = _currentUser.SupabaseUserId
            ?? throw new UnauthorizedAccessException("Invalid or expired token.");

        var emailFromToken = _currentUser.Email;
        var effectiveEmail = string.IsNullOrWhiteSpace(emailFromToken)
            ? request.Email
            : emailFromToken;

        var person = await _personRepository.GetBySupabaseUserIdAsync(supabaseUserId);
        string responsMessage;

        if (person is null)
        {
            person = new Person(
                supabaseUserId: supabaseUserId,
                email: effectiveEmail,
                name: request.Name,
                username: request.UserName,
                birthDate: request.BirthDate,
                phone: request.Phone,
                weightKg: request.WeightKg,
                heightCm: request.HeightCm
            );
            await _personRepository.AddAsync(person);
            responsMessage = "Person created successfully";
        }
        else
        {
           person.UpdateProfile(
                email: effectiveEmail,
                name: request.Name,
                username: request.UserName,
                birthDate: request.BirthDate,
                phone: request.Phone,
                weightKg: request.WeightKg,
                heightCm: request.HeightCm
           );
            responsMessage = "Person updated successfully";
        }

        await _personRepository.SaveChangesAsync();

        var data = new PersonResponseDTO(person.Id, person.Name, person.Email);
        return BaseResponse<PersonResponseDTO>.Ok(data, responsMessage);
    }

    public async Task<BaseResponse<PersonResponseDTO>> GetCurrentPerson()
    {
        var supabaseUserId = _currentUser.SupabaseUserId
            ?? throw new UnauthorizedAccessException("Invalid or expired token");

        var person = await _personRepository.GetBySupabaseUserIdAsync(supabaseUserId)
            ?? throw new InvalidOperationException("Person not Found");

        var data = new PersonResponseDTO(person.Id, person.Name, person.Email);
        return BaseResponse<PersonResponseDTO>.Ok(data, "Person fetched successfully");
    }
}
