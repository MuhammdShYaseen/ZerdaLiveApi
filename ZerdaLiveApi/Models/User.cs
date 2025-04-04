using System;
using System.Collections.Generic;

#nullable disable

namespace ZerdaLiveApi.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
