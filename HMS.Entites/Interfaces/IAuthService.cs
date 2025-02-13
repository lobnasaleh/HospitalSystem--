using HMS.Entites.Contracts;
using HMS.Entites.Models;
using HMS.Entites.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterPatient(RegisterRequest registerRequest);//mapped from dtos to avoid Entities referencing to  
        Task<AuthResponse> RegisterStaff(RegisterStaffRequest registerRequest);//mapped from dtos to avoid Entities referencing to  

        public Task<AuthResponse> UpdateStaffProfile(string id, RegisterStaffRequestVM registerRequest);



        Task<AuthResponse> Login(LoginRequest loginRequest);
        // Task<AuthResponse> Logout();
        Task<ApplicationUser> getUserById(string userId);
    }
}
