using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZerdaLiveApi.Models;

namespace ZerdaLiveApi.Helpper
{
    public class UserRole
    {
        int IntRole = 0;
        private readonly ZerdaLiveContext _context;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>")]
        public static List<ApiKey> ApiList;

        public UserRole(ZerdaLiveContext context)
        {
            _context = context;
            ApiList = new List<ApiKey>();
            ApiList = _context.ApiKeys.ToList();
        }
       
        public int CheckUserRolee(string UserName, string Password)
        {
            string UserNm = UserName;
            string pass = Password;
            string Role = "";
            using var HR = _context;
            var table = HR.Users;
            var searchresult = table.Where(c => c.UserName.Equals(UserNm) && c.Password.Equals(pass));
            if (!searchresult.Any())
            {
                return 0;
            }
            else
            {
                foreach (var ss in searchresult)
                {
                    Role = ss.Role;
                    switch (Role)
                    {
                        case "Admin":
                            IntRole = 1;
                            return IntRole;
                        case "Channels":
                            IntRole = 2;
                            return IntRole;
                        case "Series":
                            IntRole = 3;
                            return IntRole;
                        case "Movies":
                            IntRole = 4;
                            return IntRole;
                        default:
                            IntRole = 0;
                            return IntRole;
                    }

                }
                return IntRole;
            }

        }
    }
}
