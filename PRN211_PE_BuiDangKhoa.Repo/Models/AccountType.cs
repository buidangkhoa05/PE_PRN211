using System;
using System.Collections.Generic;

namespace PRN211PE_SU22_BuiDangKhoa.Repo.Models
{
    public partial class AccountType
    {
        public AccountType()
        {
            BankAccounts = new HashSet<BankAccount>();
        }

        public string TypeId { get; set; } = null!;
        public string TypeName { get; set; } = null!;
        public string? TypeDesc { get; set; }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}
