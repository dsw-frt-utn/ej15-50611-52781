using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using System.Text.Json;

namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
    private List<Speciality> _specialities = [];
    private readonly List<Doctor> _doctors = [];

    public PersistenceInMemory()
    {
        LoadSpecialities();
    }

    public void AddDoctor(Doctor doctor)
    {
        _doctors.Add(doctor);
    }

    public List<Doctor> GetActiveDoctors()
    {
        return _doctors
            .Where(doctor => doctor.IsActive)
            .ToList();
    }

    public Doctor? GetActiveDoctorById(Guid doctorId)
    {
        return _doctors.FirstOrDefault(
            doctor => doctor.Id == doctorId && doctor.IsActive
        );
    }

    public Speciality? GetSpecialityById(Guid specialityId)
    {
        return _specialities.FirstOrDefault(
            speciality => speciality.Id == specialityId
        );
    }

    public bool DeactivateDoctor(Guid doctorId)
    {
        Doctor? doctor = GetActiveDoctorById(doctorId);

        if (doctor == null)
        {
            return false;
        }

        doctor.IsActive = false;

        return true;
    }

    private void LoadSpecialities()
    {
        string jsonPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Sources",
            "specialities.json"
        );

        string jsonContent = File.ReadAllText(jsonPath);

        List<SpecialityDto> specialitiesFromJson =
            JsonSerializer.Deserialize<List<SpecialityDto>>(
                jsonContent,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? [];

        _specialities = specialitiesFromJson
            .Select(specialityDto => new Speciality(
                specialityDto.Name,
                specialityDto.Description,
                specialityDto.Id
            ))
            .ToList();
    }
}