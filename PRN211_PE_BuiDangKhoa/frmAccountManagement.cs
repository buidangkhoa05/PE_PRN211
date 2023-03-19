using Infrastructure.Repository;
using PRN211PE_SU22_BuiDangKhoa.Repo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRN211PE_SU22_BuiDangKhoa
{
    public partial class frmAccountManagement : Form
    {

        private BindingSource _source;

        private GenericRepository<AccountType> _accountTypeRepository;
        private GenericRepository<BankAccount> _bankAccountRepository;

        public frmAccountManagement(GenericRepository<AccountType> accountTypeRepository, GenericRepository<BankAccount> bankAccountRepository)
        {
            InitializeComponent();
            _accountTypeRepository = accountTypeRepository;
            _bankAccountRepository = bankAccountRepository;
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var accountID = txtAccountID.Text;
            var accountName = txtAccountName.Text;
            var opendate = dtpOpendate.Value;
            var branchName = txtBranchName.Text;
            var typeName = cboTypeName.SelectedItem.ToString();

            try
            {
                var account = new BankAccount()
                {
                    AccountId = accountID,
                    AccountName = accountName,
                    OpenDate = opendate,
                    BranchName = branchName,
                    TypeId = _accountTypeRepository.Getby(x => x.TypeName == typeName).TypeId
                };

                if(IsValidCreate(account) == false)
                {
                    MessageBox.Show("Create account data is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                _bankAccountRepository.Create(account);

                MessageBox.Show("Create account successfully");
                LoadAccount();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Create account fail");
            }


        }

        private bool IsValidCreate(BankAccount account)
        {

            bool isValid = false;

            if(account.AccountId.Trim().Equals(""))
            {
                return isValid;
            }

            var isExist = _bankAccountRepository
                .Getby(x => x.AccountId.Equals(account.AccountId)) != null;

            if(isExist)
            {
                MessageBox.Show("Account ID is exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return isValid;
            }

            if(account.AccountName.Trim().Equals(""))
            {
                MessageBox.Show("Account Name is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return isValid;
            }

            var minDate = new DateTime(2000, 1, 1);
            var maxDate = new DateTime(2022, 12, 31);

            if(account.OpenDate < minDate || account.OpenDate > maxDate)
            {
                MessageBox.Show("Open date is between 2000 and 2022", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return isValid;
            }

            var branchName = account.BranchName.Trim();

            if(branchName.Length <= 5 || !IsCapitalLetter(branchName))
            {
                MessageBox.Show("Branch Name is greater than 5 characters and each word of the Branch Name must begin with the capital letter", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return isValid;
            }

            return isValid = true;
        }

        private bool IsCapitalLetter(string stringValue)
        {
            bool isValid = false;

            stringValue = stringValue.Trim();

            var stringArray = stringValue.Split(" ");

            for(int i = 0; i < stringArray.Length; i++)
            {
                string actualValue = stringArray[i];
                char firstChar = actualValue[0];

                if(!char.IsUpper(firstChar))
                {
                    return isValid;
                }
            }

            return isValid = true;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void frmAccountManagement_Load(object sender, EventArgs e)
        {
            var typeName = _accountTypeRepository.GetAll().Select(x => x.TypeName).ToList();

            cboTypeName.Items.AddRange(typeName.ToArray());

            cboTypeName.SelectedIndex = 0;

            LoadAccount();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadAccount()
        {
            var accounts = _bankAccountRepository.GetAll(navigationProperties: "Type").Select(x => new
            {
                AccountID = x.AccountId,
                AccountName = x.AccountName,
                OpenDate = x.OpenDate,
                BranchName = x.BranchName,
                TypeName = x.Type.TypeName
            });

            if(_source == null)
            {
                _source = new BindingSource();
            }

            _source.DataSource = accounts;

            dgvAccount.DataSource = null;

            dgvAccount.DataSource = _source;
        }

        private void dgvAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadField(string cellID)
        {
            var typeNames = _accountTypeRepository.GetAll().Select(x => x.TypeName).ToList();
            var account = _bankAccountRepository.Getby(a => a.AccountId.Equals(cellID));
            var type = _accountTypeRepository.Getby(i => i.TypeId.Equals(account.TypeId));

            txtAccountID.Text = account.AccountId;
            txtAccountName.Text = account.AccountName;
            dtpOpendate.Value = account.OpenDate ?? DateTime.MinValue;
            txtBranchName.Text = account.BranchName;

            cboTypeName.SelectedIndex = typeNames.IndexOf(type.TypeName);
        }

        private void dgvAccount_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string cellID = dgvAccount.Rows[e.RowIndex].Cells[0].Value.ToString();

                LoadField(cellID);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var accountID = txtAccountID.Text;
            var accountName = txtAccountName.Text;
            var opendate = dtpOpendate.Value;
            var branchName = txtBranchName.Text;
            var typeName = cboTypeName.SelectedItem.ToString();

            var account = _bankAccountRepository.Getby(a => a.AccountId.Equals(accountID));

            if(account == null)
            {
                MessageBox.Show("Account is not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }


            //account.AccountId = accountID;
            account.AccountName = accountName;
            account.OpenDate = opendate;
            account.BranchName = branchName;
            account.TypeId = _accountTypeRepository.Getby(x => x.TypeName == typeName).TypeId;

            _bankAccountRepository.Update(account);

            MessageBox.Show("Update account successfully");

            LoadAccount();
        }

        //detele account by id
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"Do you want to delete with Id: \"{txtAccountID.Text}\" ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                _bankAccountRepository.Delete(a => a.AccountId.Equals(txtAccountID.Text));

                MessageBox.Show("Delete account successfully");

                LoadAccount();
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchValue = txtSearchValue.Text.Trim();

            var accounts = _bankAccountRepository.GetAll(a => a.BranchName.Contains(searchValue), "Type").Select(x => new
            {
                AccountID = x.AccountId,
                AccountName = x.AccountName,
                OpenDate = x.OpenDate,
                BranchName = x.BranchName,
                TypeName = x.Type.TypeName
            });

            if(_source == null)
            {
                _source = new BindingSource();
            }

            _source.DataSource = accounts;

            dgvAccount.DataSource = null;

            dgvAccount.DataSource = _source;
        }
    }
}
