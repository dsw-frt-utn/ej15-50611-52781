using Dsw2026Ej15.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dsw2026Ej15.Domain.Interfaces
{
    public interface IPersistence
    {
        void AddDoctor(Doctor doctor);

        List<Doctor> GetActiveDoctors();

        Doctor? GetActiveDoctorById(Guid doctorId);

        Speciality? GetSpecialityById(Guid specialityId);

        bool DeactivateDoctor(Guid doctorId);
    }
}
