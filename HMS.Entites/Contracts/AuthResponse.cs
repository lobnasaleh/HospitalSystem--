using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Entites.Contracts
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public bool isAuthenticated { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }//new for update
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
