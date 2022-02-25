using BLL;
using DAL;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personal_Tracking.NET
{
    public partial class FrmEmployee : Form
    {
        EmployeeDTO dto = new EmployeeDTO();
        bool combofull = false;

        public FrmEmployee()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }


        private void FrmEmployee_Load(object sender, EventArgs e)
        {
            dto = EmployeeBLL.GetAll();
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
        }


        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                int departmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == departmentID).ToList();
            }
        }

        string fileName = "";

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbImage.Load(openFileDialog1.FileName);
                txtImagePath.Text = openFileDialog1.FileName;
                string Unique = Guid.NewGuid().ToString();
                fileName += Unique + openFileDialog1.SafeFileName;
            }
        }

        bool isUnique = false;
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("Mã số nhân viên không được để trống!");
            else
            {
                isUnique = EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text));
                if (!isUnique)
                    MessageBox.Show("Mã số nhân viên đã được sử dụng. Hãy thay đổi mã số khác!");
                else
                    MessageBox.Show("Mã số nhân viên có thể sử dụng!");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserNo.Text.Trim() == "")
                MessageBox.Show("Mã số nhân viên không được để trống!");
            else if (!EmployeeBLL.isUnique(Convert.ToInt32(txtUserNo.Text)))
                MessageBox.Show("Mã số nhân viên đã được sử dụng. Hãy thay đổi mã số khác!");
            else if (txtPassword.Text.Trim() == "")
                MessageBox.Show("Mục mật khẩu không được để trống!");
            else if (txtSurname.Text.Trim() == "")
                MessageBox.Show("Mục họ không được để trống!");
            else if (txtName.Text.Trim() == "")
                MessageBox.Show("Mục tên không được để trống!");
            else if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Mục lương không được để trống!");
            else if (cmbDepartment.SelectedIndex == -1)
                MessageBox.Show("Hãy lựa chọn một phòng ban!");
            else if (cmbPosition.SelectedIndex == -1)
                MessageBox.Show("Hãy lựa chọn một chức vụ!");
            else
            {
                EMPLOYEE employee = new EMPLOYEE();
                employee.UserNo = Convert.ToInt32(txtUserNo.Text);
                employee.Password = txtPassword.Text;
                employee.isAdmin = chAdmin.Checked;
                employee.Surname = txtSurname.Text;
                employee.Name = txtName.Text;
                employee.Salary = Convert.ToInt32(txtSalary.Text);
                employee.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                employee.PositionID = Convert.ToInt32(cmbPosition.SelectedValue);
                employee.Address = txtAddress.Text;
                employee.BirthDay = dtpBirthday.Value;
                employee.ImagePath = fileName;
                EmployeeBLL.AddEmployee(employee);
                File.Copy(txtImagePath.Text, @"images\\" + fileName);
                MessageBox.Show("Thông tin nhân viên đã được lưu!");
                txtUserNo.Clear();
                txtPassword.Clear();
                chAdmin.Checked = false;
                txtSurname.Clear();
                txtName.Clear();
                txtSalary.Clear();
                txtAddress.Clear();
                txtImagePath.Clear();
                pbImage.Image = null;
                combofull = false;
                cmbDepartment.SelectedIndex = -1;
                cmbPosition.DataSource = dto.Positions;
                cmbPosition.SelectedIndex = -1;
                combofull = true;
                dtpBirthday.Value = DateTime.Today;
            }
        }
    }
}