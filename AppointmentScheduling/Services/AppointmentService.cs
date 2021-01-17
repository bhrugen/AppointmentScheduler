using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> AddUpdate(AppointmentVM model)
        {
            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duriation));

            if (model != null && model.Id > 0)
            {
                //update
                return 1;
            }
            else
            {
                //create
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duriation = model.Duriation,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    IsDoctorApproved = false,
                    AdminId = model.AdminId
                };

                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();
                return 2;
            }

        }

        public List<DoctorVM> GetDoctorList()
        {
            var doctors = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Helper.Doctor) on userRoles.RoleId equals roles.Id
                           select new DoctorVM
                           {
                               Id = user.Id,
                               Name = user.Name
                           }
                           ).ToList();

            return doctors;
        }

        public List<PatientVM> GetPatientList()
        {
            var patients = (from user in _db.Users
                            join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                            join roles in _db.Roles.Where(x => x.Name == Helper.Patient) on userRoles.RoleId equals roles.Id
                            select new PatientVM
                            {
                                Id = user.Id,
                                Name = user.Name
                            }
                           ).ToList();

            return patients;
        }
    }
}
