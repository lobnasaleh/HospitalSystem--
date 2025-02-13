using HMS.Entites.Contracts;
using HMS.Entites.Enums;
using HMS.Entites.Interfaces;
using HMS.Entites.Models;
using HMS.Entites.ViewModel;
using HMS.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DataAccess.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _config;


        public AuthService(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> roleManager, IConfiguration _config)
        {
            this._userManager = _userManager;
            this.roleManager = roleManager;
            this._config = _config;

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

            //generate jwt token
            //ba strore fel token hagat zy el id bta3 el user ,roles,name 3shan keda ba3at el user lel generatejwt
            JwtSecurityToken jwt = await GenerateJWTTokenAsync(user);

            AuthResponse res = new AuthResponse()
            {
                //kol dool baraga3hom lel front l2eno momkn yehtaghom
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                isAuthenticated = true,
                Message = "Logged In Successfully !",
                Email = loginRequest.Email,
                Roles = userRoles,//agarb asheel .tolist
                ExpiresOn = DateTime.Now.AddDays(30),
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


            JwtSecurityToken jwt = await GenerateJWTTokenAsync(patient);

            //create Token



            return new AuthResponse
            {
                Email = patient.Email,
                ExpiresOn = DateTime.Now.AddDays(30),
                isAuthenticated = true,
                Message = $"Welcome {patient.UserName}",
                Roles = new List<string> { "Patient" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Username = patient.UserName
            };
        }

        public async Task<AuthResponse>UpdateStaffProfile(string id, RegisterStaffRequestVM registerRequest)
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
            //RegisterStaffRequestVM-->Staff
            st.Address = registerRequest.Address;
            st.FullName = registerRequest.FullName;
            st.UserName = registerRequest.UserName;
            st.PasswordHash = registerRequest.PasswordHash;
            st.Position = (Position)registerRequest.Position;
            st.Email = registerRequest.Email;
            st.DepartmentId = registerRequest.DepartmentId;
            st.Qualification = registerRequest.Qualification;

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


            JwtSecurityToken jwt = await GenerateJWTTokenAsync(st);

            //create Token



            return new AuthResponse
            {
                Email = st.Email,
                ExpiresOn = DateTime.Now.AddDays(30),
                isAuthenticated = true,
                Message = $"Welcome {st.UserName}",
                Roles = new List<string> { "Staff" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Username = st.UserName
            };
        }
        public async Task<JwtSecurityToken> GenerateJWTTokenAsync(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            List<Claim> userclaims = new List<Claim>();//to store Id,name,roles and make jwt unique every time

            foreach (var role in roles)
            {
                userclaims.Add(new Claim(ClaimTypes.Role, role));
            }
            userclaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            userclaims.Add(new Claim(ClaimTypes.Email, user.Email));
            userclaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            userclaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:secretkey"]));
            var SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken mytoken = new JwtSecurityToken(
                claims: userclaims,
                audience: _config["jwt:audience"],
                issuer: _config["jwt:issuer"],
                expires: DateTime.Now.AddDays(30),
                signingCredentials: SigningCredentials


                );


            foreach (var claim in claims)//for testing
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            return mytoken;
        }
    }
}

