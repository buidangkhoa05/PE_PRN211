using Infrastructure.Repository;
using PRN211PE_SU22_BuiDangKhoa.Repo.Models;

namespace PRN211PE_SU22_BuiDangKhoa
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            BankAccountTypeContext context = new BankAccountTypeContext();

            GenericRepository<AccountType> AccountTypeRepository = new GenericRepository<AccountType>(context);
            GenericRepository<BankAccount> BankAccountRepository = new GenericRepository<BankAccount>(context);
            GenericRepository<User> UserRepository = new GenericRepository<User>(context);

            var frmLogin = new frmLogin(UserRepository);

            Application.Run(frmLogin);

            if(frmLogin.DialogResult == DialogResult.OK)
            {
                var frmAccountManagement = new frmAccountManagement(AccountTypeRepository, BankAccountRepository);

                Application.Run(frmAccountManagement);
            }
        }
    }
}