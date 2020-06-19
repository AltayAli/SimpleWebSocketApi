using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketrealize.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public ICollection<Platform> Platforms { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public User()
        {
            Platforms = new HashSet<Platform>();
        }
    }
}
