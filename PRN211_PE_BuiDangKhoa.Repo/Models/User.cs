using System;
using System.Collections.Generic;

namespace PRN211PE_SU22_BuiDangKhoa.Repo.Models
{
    public partial class User
    {
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? UserName { get; set; }
        public int? UserRole { get; set; }
    }
}
