using Microsoft.Extensions.Logging;
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
    private readonly IAdminAuthService _adminAuthService;
    private readonly ILogger<PersonService> _logger;

    public PersonService(
        ICurrentUserService currentUser,
        IPersonRepository personRepository,
        IAdminAuthService adminAuthService,
        ILogger<PersonService> logger)
    {
        _currentUser = currentUser;
        _personRepository = personRepository;
        _adminAuthService = adminAuthService;
        _logger = logger;
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

        try
        {
            string responsMessage;

            if (person is null)
            {
                _logger.LogInformation("Creating new Person for SupabaseUserId={SupabaseUserId}", supabaseUserId);

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
                _logger.LogInformation("Updating existing Person (Id={PersonId}) for SupabaseUserId={SupabaseUserId}", person.Id, supabaseUserId);

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

            _logger.LogInformation("Person saved successfully for SupabaseUserId={SupabaseUserId}", supabaseUserId);

            var data = new PersonResponseDTO(person.Id, person.Name, person.Email);
            return BaseResponse<PersonResponseDTO>.Ok(responsMessage, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating/updating Person for SupabaseUserId={SupabaseUserId}. Executing rollback...", supabaseUserId);

            var deleteResponse = await _adminAuthService.DeleteUserAsync(supabaseUserId);

            if (deleteResponse.Success)
            {
                _logger.LogWarning("Rollback successful: Supabase user deleted. SupabaseUserId={SupabaseUserId}", supabaseUserId);
            }
            else
            {
                _logger.LogError("Rollback attempted but failed to delete Supabase user. SupabaseUserId={SupabaseUserId}", supabaseUserId);
            }

            return BaseResponse<PersonResponseDTO>.Fail("Failed to create/update person.");
        }
    }

    public async Task<BaseResponse<PersonResponseDTO>> GetCurrentPerson()
    {
        var supabaseUserId = _currentUser.SupabaseUserId
            ?? throw new UnauthorizedAccessException("Invalid or expired token");

        var person = await _personRepository.GetBySupabaseUserIdAsync(supabaseUserId)
            ?? throw new InvalidOperationException("Person not Found");

        var data = new PersonResponseDTO(person.Id, person.Name, person.Email);
        return BaseResponse<PersonResponseDTO>.Ok("Person fetched successfully", data);
    }
}
