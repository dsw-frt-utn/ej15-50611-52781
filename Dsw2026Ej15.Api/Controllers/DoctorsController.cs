using Dsw2026Ej15.Api.Dtos;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dsw2026Ej15.Api.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private readonly IPersistence _persistence;

        public DoctorsController(IPersistence persistence)
        {
            _persistence = persistence;
        }

        [HttpPost]
        public IActionResult AddDoctor(
            [FromBody] CreateDoctorDto doctorRequest)
        {
            if (string.IsNullOrWhiteSpace(doctorRequest.Name))
            {
                return BadRequest(
                    "El Nombre del médico es obligatorio"
                );
            }

            if (string.IsNullOrWhiteSpace(
                doctorRequest.LicenseNumber))
            {
                return BadRequest(
                    "El Número de matrícula del médico es obligatorio"
                );
            }

            Speciality? selectedSpeciality =
                _persistence.GetSpecialityById(
                    doctorRequest.SpecialityId
                );

            if (selectedSpeciality == null)
            {
                return BadRequest(
                    "La especialidad indicada no existe"
                );
            }

            Doctor newDoctor = new Doctor(
                doctorRequest.Name.Trim(),
                doctorRequest.LicenseNumber.Trim(),
                selectedSpeciality
            );

            _persistence.AddDoctor(newDoctor);

            DoctorResponseDto doctorResponse =
                CreateDoctorResponse(newDoctor);

            return CreatedAtAction(
                nameof(GetDoctorById),
                new { doctorId = newDoctor.Id },
                doctorResponse
            );
        }

        [HttpGet]
        public IActionResult GetDoctors()
        {
            List<Doctor> activeDoctors =
                _persistence.GetActiveDoctors();

            List<DoctorResponseDto> doctorResponses =
                activeDoctors
                    .Select(doctor =>
                        CreateDoctorResponse(doctor))
                    .ToList();

            return Ok(doctorResponses);
        }

        [HttpGet("{doctorId:guid}")]
        public IActionResult GetDoctorById(Guid doctorId)
        {
            Doctor? selectedDoctor =
                _persistence.GetActiveDoctorById(doctorId);

            if (selectedDoctor == null)
            {
                return NotFound();
            }

            DoctorResponseDto doctorResponse =
                CreateDoctorResponse(selectedDoctor);

            return Ok(doctorResponse);
        }

        [HttpDelete("{doctorId:guid}")]
        public IActionResult DeleteDoctor(Guid doctorId)
        {
            bool doctorWasDeactivated =
                _persistence.DeactivateDoctor(doctorId);

            if (!doctorWasDeactivated)
            {
                return NotFound();
            }

            return NoContent();
        }

        private static DoctorResponseDto CreateDoctorResponse(
            Doctor doctor)
        {
            return new DoctorResponseDto
            {
                Id = doctor.Id,
                Name = doctor.Name,
                LicenseNumber = doctor.LicenseNumber,
                SpecialityName =
                    doctor.Speciality?.Name ?? string.Empty
            };
        }
    }
}