using Infrastructure.Repository;
using PRN211PE_SU22_BuiDangKhoa.Repo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRN211PE_SU22_BuiDangKhoa
{
    public partial class frmLogin : Form
    {
        private GenericRepository<User> _userRepository;

        public frmLogin(GenericRepository<User> userRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;

            if(IsValidUserForLogin(username, password))
            {
                MessageBox.Show("Login successfully");
                this.Hide();

                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                DialogResult = DialogResult.Continue;
                MessageBox.Show("You are not allowed to access this function!");
            }
        }

        private bool IsValidUserForLogin(string username, string password)
        {
            var user = _userRepository.Getby(u => u.UserId == username
                                            && u.Password == password);

            if(user != null && user.UserRole == 1)
            {
                return true;
            }

            return false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
