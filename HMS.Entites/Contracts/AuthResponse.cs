﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Contracts
{
    public class AuthResponse
    {
        
        public bool isAuthenticated { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }//update
        public string Message { get; set; }
        public bool IsSuccess { get; set; }//new for update
        public string Username { get; set; }
        public string Email { get; set; }
       
    }
}
