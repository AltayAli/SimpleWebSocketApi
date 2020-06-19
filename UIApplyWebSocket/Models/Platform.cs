using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketrealize.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public byte Status { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}
