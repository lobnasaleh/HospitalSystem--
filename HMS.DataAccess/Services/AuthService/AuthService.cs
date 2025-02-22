using HMS.Entites.Contracts;
using HMS.Entites.Enums;
using HMS.Entites.Interfaces;
using HMS.Entites.Models;
using HMS.Entites.ViewModel;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace HMS.DataAccess.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;


        public AuthService(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration _config, IHttpContextAccessor httpContextAccessor)
        {
            this._userManager = _userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this._config = _config;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApplicationUser> getUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IEnumerable<ApplicationUser>> getAllUsers()
        {
            return await _userManager.GetUsersInRoleAsync("Staff");
        }

        public async Task<AuthResponse> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                AuthResponse response = new AuthResponse() { isAuthenticated = false, Message = "Email Or Password is Incorrect" };
                return response;
            }

            //get roles of user 
            var userRoles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
               
            }

            /// Create a claims identity
            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the user
          await _httpContextAccessor.HttpContext.SignInAsync(
              IdentityConstants.ApplicationScheme, 
             claimsPrincipal
                  );
            AuthResponse res = new AuthResponse()
            {
               
                isAuthenticated = true,
                Message = "Logged In Successfully !",
                Email = loginRequest.Email,
                Roles = userRoles,
                Username = user.UserName
            };

            return res;

        }
        public async Task<AuthResponse> RegisterPatient(RegisterRequest registerRequest)
        {
            if (await _userManager.FindByEmailAsync(registerRequest.Email) is not null)//we found a user with this email
            {
                return new AuthResponse() { isAuthenticated = false, Message = "This Email is already Registered" };
            }
            if (await _userManager.FindByNameAsync(registerRequest.UserName) is not null)//we found a user with this username
            {
                return new AuthResponse() { isAuthenticated = false, Message = "This Username is already Registered" };
            }
            //map RegisterRequest to ApplicationUser
            Patient patient = new Patient()
            {

                Email = registerRequest.Email,
                FullName = registerRequest.FullName,
                UserName = registerRequest.UserName,
                Address = registerRequest.Address,
                DOB=registerRequest.DOB,
                InsuranceNumber = registerRequest.InsuranceNumber,
                InsuranceProvider = registerRequest.InsuranceProvider,
                EmergencyContact = registerRequest.EmergencyContact
                //el password ha ahotha ma3 el createasync
            };
            var errorslist = new List<string>();
            var res = await _userManager.CreateAsync(patient, registerRequest.PasswordHash);
            if (!res.Succeeded)
            {
                foreach (var err in res.Errors)
                {
                    errorslist.Add(err.Description);
                }

                string errormessages = string.Join(", ", errorslist);

                return new AuthResponse()
                {
                    isAuthenticated = false,
                    Message = errormessages
                };
            }
            //assign role
            await _userManager.AddToRoleAsync(patient, "Patient");

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, patient.Id),
                new Claim(ClaimTypes.Name, patient.UserName),
                new Claim(ClaimTypes.Email, patient.Email)
            };

            claims.Add(new Claim(ClaimTypes.Role, "Patient"));

            // await signInManager.SignInWithClaimsAsync(user, false, claims);
            // Create a claims identity
            /// Create a claims identity
            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Sign in the user
            await _httpContextAccessor.HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
               claimsPrincipal
                    );
            return new AuthResponse
            {
                Email = patient.Email,
                isAuthenticated = true,
                Message = $"Welcome {patient.UserName}",
                Roles = new List<string> { "Patient" },
                Username = patient.UserName
            };
        }

        public async Task<AuthResponse>UpdateStaffProfile(string id, UpateStaffRequest UpdateRequest)
        {
            AuthResponse auth = new AuthResponse();
            //registerRequest.
            Staff st= await _userManager.FindByIdAsync(id) as Staff;
            if (st == null)
            {
                auth.Message = "No Staff With This ID is found";
                auth.IsSuccess = false;
                   return auth; 
            }
            //updateStaffRequestVM-->Staff
            st.Address = UpdateRequest.Address;
            st.FullName = UpdateRequest.FullName;
            st.Position = (Position)UpdateRequest.Position;
            st.DepartmentId = UpdateRequest.DepartmentId;
            st.Qualification = UpdateRequest.Qualification;

            var result=  await _userManager.UpdateAsync(st);

            if (result.Succeeded)
            {
                auth.Message = "Staff Info Updated Successfully";
                auth.IsSuccess = true;
            }
            else
            {
                auth.Message = "Failed to Update Staff Info";
                auth.IsSuccess = false;
            }
            return auth;
        }


        public async Task<AuthResponse> UpdateProfile(string id, UpdateRequest UpdateRequest)
        {
            AuthResponse auth = new AuthResponse();
            //registerRequest.
            Patient p = await _userManager.FindByIdAsync(id) as Patient;
            if (p == null)
            {
                auth.Message = "No Patient With This ID is found";
                auth.IsSuccess = false;
                return auth;
            }
            //updateRequestVM-->Patient
            p.Address = UpdateRequest.Address;
            p.FullName = UpdateRequest.FullName;
            p.InsuranceProvider = UpdateRequest.InsuranceProvider;
            p.InsuranceNumber = UpdateRequest.InsuranceNumber;
            p.DOB = UpdateRequest.DOB;
            p.EmergencyContact = UpdateRequest.EmergencyContact;

            var result = await _userManager.UpdateAsync(p);

            if (result.Succeeded)
            {
                auth.Message = "Staff Info Updated Successfully";
                auth.IsSuccess = true;
            }
            else
            {    auth.Errors=result.Errors.ToList();
                
                auth.Message = "Failed to Update Staff Info";
                auth.IsSuccess = false;
            }
            return auth;
        }

        public async Task<AuthResponse> RegisterStaff(RegisterStaffRequest registerRequest)
        {
            if (await _userManager.FindByEmailAsync(registerRequest.Email) is not null)//we found a user with this email
            {
                return new AuthResponse() { isAuthenticated = false, Message = "This Email is already Registered" };
            }
            if (await _userManager.FindByNameAsync(registerRequest.UserName) is not null)//we found a user with this username
            {
                return new AuthResponse() { isAuthenticated = false, Message = "This Username is already Registered" };
            }
            //map RegisterRequest to staff
            Staff st=new Staff()
            {

                Email = registerRequest.Email,
                FullName = registerRequest.FullName,
                UserName = registerRequest.UserName,
                Address = registerRequest.Address,
                Position=(Position)registerRequest.Position,
                Qualification=registerRequest.Qualification,
                DepartmentId=registerRequest.DepartmentId
                //el password ha ahotha ma3 el createasync
            };
            var errorslist = new List<string>();
            var res = await _userManager.CreateAsync(st, registerRequest.PasswordHash);
            if (!res.Succeeded)
            {
                foreach (var err in res.Errors)
                {
                    errorslist.Add(err.Description);
                }

                string errormessages = string.Join(", ", errorslist);

                return new AuthResponse()
                {
                    isAuthenticated = false,
                    Message = errormessages
                };
            }
            //assign role
            await _userManager.AddToRoleAsync(st, "Staff");

         
            return new AuthResponse
            {
                Email = st.Email,
                isAuthenticated = true,
                Roles = new List<string> { "Staff" },
                Username = st.UserName
            };
        }
        public async Task<AuthResponse> AddRole(string name)
        {
            IdentityRole role=new IdentityRole();
            role.Name = name;
            var res=await roleManager.CreateAsync(role);
            if (res.Succeeded) { 
            return new AuthResponse() { IsSuccess = true };
            }
            return new AuthResponse() { IsSuccess = false };
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
              var res= await roleManager.Roles.ToListAsync();
            return res;
        }

        public async Task<AuthResponse> Logout()
        {
          await signInManager.SignOutAsync();
          return new AuthResponse() { IsSuccess = true };
        }
    }
}

