using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using DAL.DTO;
using DAL;

namespace Personal_Tracking.NET
{
    public partial class FrmSalary : Form
    {
        SalaryDTO dto = new SalaryDTO();
        private bool combofull;
        SALARY salary = new SALARY();
        public SalaryDetailDTO detail = new SalaryDetailDTO();
        public bool isUpdate = false;
        int oldSalary = 0;

        public FrmSalary()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void FrmSalary_Load(object sender, EventArgs e)
        {
            dto = SalaryBLL.GetAll();
            if (!isUpdate)
            {
                dataGridView1.DataSource = dto.Employees;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Mã số nhân viên";
                dataGridView1.Columns[2].HeaderText = "Họ";
                dataGridView1.Columns[3].HeaderText = "Tên";
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                combofull = false;
                cmbDepartment.DataSource = dto.Departments;
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "ID";
                cmbPosition.DataSource = dto.Positions;
                cmbPosition.DisplayMember = "PositionName";
                cmbPosition.ValueMember = "ID";
                cmbDepartment.SelectedIndex = -1;
                cmbPosition.SelectedIndex = -1;
                if (dto.Departments.Count > 0)
                    combofull = true;
            }
            cmbMonth.DataSource = dto.Months;
            cmbMonth.DisplayMember = "MonthName";
            cmbMonth.ValueMember = "ID";
            cmbMonth.SelectedIndex = -1;
            if (isUpdate)
            {
                panel1.Hide();
                txtSurname.Text = detail.Surname;
                txtName.Text = detail.Name;
                txtSalary.Text = detail.SalaryAmount.ToString();
                txtYear.Text = detail.SalaryYear.ToString();
                cmbMonth.SelectedValue = detail.MonthID;
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtUserNo.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtSurname.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtYear.Text = DateTime.Today.Year.ToString();
            txtSalary.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            salary.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            oldSalary = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtYear.Text.Trim() == "")
                MessageBox.Show("Hãy nhập mục năm!");
            else if (cmbMonth.SelectedIndex == -1)
                MessageBox.Show("Hãy lựa chọn một tháng!");
            else if (txtSalary.Text.Trim() == "")
                MessageBox.Show("Hãy nhập mục lương!");
            else
            {
                bool control = false;
                if (!isUpdate)
                {
                    if (salary.EmployeeID == 0)
                        MessageBox.Show("Hãy lựa chọn một nhân viên từ bảng");
                    else
                    {
                        salary.Year = Convert.ToInt32(txtYear.Text);
                        salary.MonthID = Convert.ToInt32(cmbMonth.SelectedValue);
                        salary.Amount = Convert.ToInt32(txtSalary.Text);
                        if (salary.Amount > oldSalary)
                            control = true;
                        SalaryBLL.AddSalary(salary, control);
                        MessageBox.Show("Lương đã được thêm");
                        cmbMonth.SelectedIndex = -1;
                        salary = new SALARY();
                    }
                }
                else
                {
                    DialogResult rs = MessageBox.Show("Bạn có muốn cập nhật?", "Cảnh báo!", MessageBoxButtons.YesNo);
                    if (DialogResult.Yes == rs)
                    {
                        SALARY salary = new SALARY();
                        salary.ID = detail.SalaryID;
                        salary.EmployeeID = detail.EmployeeID;
                        salary.Year = Convert.ToInt32(txtYear.Text);
                        salary.MonthID = Convert.ToInt32(cmbMonth.SelectedValue);
                        salary.Amount = Convert.ToInt32(txtSalary.Text);
                        if (salary.Amount > detail.OldSalary)
                            control = true;
                        SalaryBLL.UpdateSalary(salary, control);
                        MessageBox.Show("Cập nhật thành công!");
                        Close();
                    }
                }
            }

        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
                List<EmployeeDetailDTO> list = dto.Employees;
                dataGridView1.DataSource =
                    list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
        }

        private void cmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                List<EmployeeDetailDTO> list = dto.Employees;
                dataGridView1.DataSource =
                    list.Where(x => x.PositionID == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            }
        }
    }
}
