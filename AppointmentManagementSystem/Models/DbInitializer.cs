﻿using AppointmentManagementSystem.Areas.Identity.Data;
using AppointmentManagementSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace AppointmentManagementSystem.Models;


public static class DbInitializer
{
    public static async void Seed(IApplicationBuilder applicationBuilder)
    {
        AppointmentDbContext AppointmentContext = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppointmentDbContext>();

        if (!AppointmentContext.appointments.Any())
        {
            AppointmentContext.appointments.AddRange
            (
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 29), AppointmentTime = new DateTime(2024, 1, 1, 9, 0, 0), UserEmail = "test@test.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 29), AppointmentTime = new DateTime(2024, 1, 1, 10, 30, 0), UserEmail = "test@test.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 28), AppointmentTime = new DateTime(2024, 1, 1, 14, 15, 0), UserEmail = "test@test.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 27), AppointmentTime = new DateTime(2024, 1, 1, 16, 45, 0), UserEmail = "test@test.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 23), AppointmentTime = new DateTime(2024, 1, 1, 11, 0, 0), UserEmail = "test@test.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 26), AppointmentTime = new DateTime(2024, 1, 1, 13, 45, 0), UserEmail = "email@email.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 26), AppointmentTime = new DateTime(2024, 1, 1, 15, 30, 0), UserEmail = "email@email.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 25), AppointmentTime = new DateTime(2024, 1, 1, 17, 15, 0), UserEmail = "email@email.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 25), AppointmentTime = new DateTime(2024, 1, 1, 9, 45, 0), UserEmail = "email@email.com" },
                new UpcomingAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2024, 4, 26), AppointmentTime = new DateTime(2024, 1, 1, 12, 0, 0), UserEmail = "email@email.com" }
            );
        }

        if (!AppointmentContext.archivedAppointments.Any())
        {
            AppointmentContext.archivedAppointments.AddRange
            (
                new ArchivedAppointment { AppointmentSubject = "Meeting", AppointmentDate = new DateTime(2023, 4, 13), AppointmentTime = new DateTime(2024, 1, 1, 9, 0, 0), UserEmail = "test@test.com" },
                new ArchivedAppointment { AppointmentSubject = "Conference", AppointmentDate = new DateTime(2022, 7, 20), AppointmentTime = new DateTime(2024, 1, 1, 14, 30, 0), UserEmail = "test@test.com" },
                new ArchivedAppointment { AppointmentSubject = "Training", AppointmentDate = new DateTime(2021, 9, 5), AppointmentTime = new DateTime(2024, 1, 1, 11, 0, 0), UserEmail = "email@email.com" },
                new ArchivedAppointment { AppointmentSubject = "Seminar", AppointmentDate = new DateTime(2020, 12, 12), AppointmentTime = new DateTime(2024, 1, 1, 9, 30, 0), UserEmail = "email@email.com" },
                new ArchivedAppointment { AppointmentSubject = "Workshop", AppointmentDate = new DateTime(2019, 6, 25), AppointmentTime = new DateTime(2024, 1, 1, 12, 0, 0), UserEmail = "email@email.com" }
            );
        }

        AppointmentContext.SaveChanges();

        //Should seed the data for 3 base accounts
        //1 admin and 2 users
        //this should make use of a users class to hold stuff to store
        //like phone, email, password, Role

        ////seeding data
        using (var scope = applicationBuilder.ApplicationServices.CreateScope())
        {
            //seeding initial sata into application
            // - roles
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); //can grab an instance of this using dependency injection
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                //if no roles provided create roles
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using (var scope = applicationBuilder.ApplicationServices.CreateScope())
        {
            //seeding initial sata into application
            // - accounts
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>(); //can grab an instance of this using dependency injection

            DbUserInfo[] dbUsers = new DbUserInfo[]
            {
                new DbUserInfo
                {
                    FirstName = "Admin",
                    LastName = "User",
                    PhoneNumber = "1234567890",
                    Role = "Admin",
                    Email = "admin@admin.com",
                    Password = "h493yz96x5XyxYTfAOZdey/rL0Qe2fmESwmldH9Ph9g="
                },
                new DbUserInfo
                {
                    FirstName = "Test",
                    LastName = "User",
                    PhoneNumber = "9876543210",
                    Role = "User",
                    Email = "test@test.com",
                    Password = "9NgIcEyeC6DRUQwjgD2NEJ4lRV6N3rkMVpndW9u0VOE="
                },
                new DbUserInfo
                {
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "2012345678",
                    Role = "User",
                    Email = "email@email.com",
                    Password = "CYAW1j7zrwejgW47ldd36rgyOmUWHUJuwPRoOWvV5MM="
                }
            };

            foreach (var newDbUser in dbUsers)
            {
                if (await userManager.FindByEmailAsync(newDbUser.Email) == null)
                {
                    var user = new AppUser
                    {

                        UserName = newDbUser.Email,
                        Email = newDbUser.Email,
                        EmailConfirmed = true,
                        FirstName = newDbUser.FirstName,
                        LastName = newDbUser.LastName,
                        PhoneNumber = newDbUser.PhoneNumber
                    };

                    var result = await userManager.CreateAsync(user, newDbUser.Password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, newDbUser.Role);
                    } else
                    {
                        // Handle error
                        throw new Exception($"Error creating seeded user data");
                    }
                }
            }
        }
    }
}

public class DbUserInfo
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

}