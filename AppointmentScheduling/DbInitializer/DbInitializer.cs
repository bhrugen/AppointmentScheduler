using AppointmentScheduling.Models;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentScheduling.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initalize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Roles.Any(x => x.Name == Utility.Helper.Admin)) return;


            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Patient)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Admin Spark"

            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult();
            
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "doctor@gmail.com",
                Email = "doctor@gmail.com",
                EmailConfirmed = true,
                Name = "Doctor Spark"

            }, "Doctor123*").GetAwaiter().GetResult();
            
            ApplicationUser doctor = _db.Users.FirstOrDefault(u => u.Email == "doctor@gmail.com");
            _userManager.AddToRoleAsync(doctor, Helper.Doctor).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "patient@gmail.com",
                Email = "patient@gmail.com",
                EmailConfirmed = true,
                Name = "Patient Spark"

            }, "Patient123*").GetAwaiter().GetResult();
            
            ApplicationUser patient = _db.Users.FirstOrDefault(u => u.Email == "patient@gmail.com");
            _userManager.AddToRoleAsync(patient, Helper.Patient).GetAwaiter().GetResult();
        }
    }
}
