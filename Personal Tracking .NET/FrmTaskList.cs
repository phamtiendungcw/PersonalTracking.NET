﻿using System;
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
    public partial class FrmTaskList : Form
    {
        public FrmTaskList()
        {
            InitializeComponent();
        }

        TaskDTO dto = new TaskDTO();
        private bool combofull = false;

        void FillAllData()
        {
            dto = TaskBLL.GetAll();
            if (!UserStatic.isAdmin)
                dto.Tasks = dto.Tasks.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();
            dataGridView1.DataSource = dto.Tasks;
            combofull = false;
            cmbDepartment.DataSource = dto.Departments;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.DisplayMember = "PositionName";
            cmbPosition.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            cmbTaskState.DataSource = dto.TaskStates;
            cmbTaskState.DisplayMember = "StateName";
            cmbTaskState.ValueMember = "ID";
            cmbTaskState.SelectedIndex = -1;

        }

        TaskDetailDTO detail = new TaskDetailDTO();
        private void FrmTaskList_Load(object sender, EventArgs e)
        {
            FillAllData();
            dataGridView1.Columns[0].HeaderText = "Tiêu đề nhiệm vụ";
            dataGridView1.Columns[1].HeaderText = "Mã số nhân viên";
            dataGridView1.Columns[2].HeaderText = "Họ";
            dataGridView1.Columns[3].HeaderText = "Tên";
            dataGridView1.Columns[4].HeaderText = "Ngày bắt đầu";
            dataGridView1.Columns[5].HeaderText = "Ngày bàn giao";
            dataGridView1.Columns[6].HeaderText = "Trạng thái nhiệm vụ";
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;
            if (!UserStatic.isAdmin)
            {
                btnNew.Visible = false;
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
                btnClose.Location = new Point(476, 16);
                btnApprove.Location = new Point(358, 16);
                pnlForAdmin.Hide();
                btnApprove.Text = "Bàn giao";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmTask frm = new FrmTask();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.TaskID == 0)
                MessageBox.Show("Hãy lựa chọn một nhiệm vụ từ bảng!");
            else
            {
                FrmTask frm = new FrmTask();
                frm.isUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();
            }
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<TaskDetailDTO> list = dto.Tasks;
            if (txtUserNo.Text.Trim() != "")
                list = list.Where(x => x.UserNo == Convert.ToInt32(txtUserNo.Text)).ToList();
            if (txtName.Text.Trim() != "")
                list = list.Where(x => x.Name.Contains(txtName.Text)).ToList();
            if (txtSurname.Text.Trim() != "")
                list = list.Where(x => x.Surname.Contains(txtSurname.Text)).ToList();
            if (cmbDepartment.SelectedIndex != -1)
                list = list.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            if (cmbPosition.SelectedIndex != -1)
                list = list.Where(x => x.PositionID == Convert.ToInt32(cmbPosition.SelectedValue)).ToList();
            if (rbStartDate.Checked)
                list = list.Where(x =>
                    x.TaskStartDate > Convert.ToDateTime(dtpStart.Value) &&
                    x.TaskStartDate < Convert.ToDateTime(dtpEnd.Value)).ToList();
            if (rbDeliveryDate.Checked)
                list = list.Where(x =>
                    x.TaskDeliveryDate > Convert.ToDateTime(dtpStart.Value) &&
                    x.TaskDeliveryDate < Convert.ToDateTime(dtpEnd.Value)).ToList();
            if (cmbTaskState.SelectedIndex != -1)
                list = list.Where(x => x.TaskStateID == Convert.ToInt32(cmbTaskState.SelectedValue)).ToList();
            dataGridView1.DataSource = list;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CleanFilters();
        }

        private void CleanFilters()
        {
            txtUserNo.Clear();
            txtName.Clear();
            txtSurname.Clear();
            combofull = false;
            cmbDepartment.SelectedIndex = -1;
            cmbPosition.DataSource = dto.Positions;
            cmbPosition.SelectedIndex = -1;
            combofull = true;
            rbDeliveryDate.Checked = false;
            rbStartDate.Checked = false;
            cmbTaskState.SelectedIndex = -1;
            dataGridView1.DataSource = dto.Tasks;
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;

        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.Title = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            detail.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.Surname = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            detail.Name = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            detail.TaskStartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            detail.TaskDeliveryDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            detail.EmployeeID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
            detail.TaskID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[11].Value);
            detail.Content = dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();
            detail.TaskStateID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[14].Value);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn xóa nhiệm vụ này?", "Cảnh báo!!!",
                MessageBoxButtons.YesNo);
            if (rs == DialogResult.Yes)
            {
                if (detail.TaskStateID == 2 || detail.TaskStateID == 3)
                    MessageBox.Show("Bạn không thể xóa nhiệm vụ đã được bàn giao hoặc đã phê duyệt!");
                else
                {
                    TaskBLL.DeleteTask(detail.TaskID);
                    MessageBox.Show("Nhiệm vụ này đã được xóa!");
                    FillAllData();
                    CleanFilters();
                }
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (UserStatic.isAdmin && detail.TaskStateID == TaskState.OnEmployee && detail.EmployeeID != UserStatic.EmployeeID)
                MessageBox.Show("Trước khi phê duyệt nhiệm vụ thì nhân viên phải bàn giao nhiệm vụ!");
            else if (UserStatic.isAdmin && detail.TaskStateID == TaskState.Approved)
                MessageBox.Show("Nhiệm vụ này đã được phê duyệt!");
            else if (!UserStatic.isAdmin && detail.TaskStateID == TaskState.Delivered)
                MessageBox.Show("Nhiệm vụ đã được bàn giao!");
            else if (!UserStatic.isAdmin && detail.TaskStateID == TaskState.Approved)
                MessageBox.Show("Nhiệm vụ này đã được phê duyệt!");
            else
            {
                TaskBLL.ApproveTask(detail.TaskID, UserStatic.isAdmin);
                MessageBox.Show("Cập nhật thành công!");
                FillAllData();
                CleanFilters();
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel.ExcelExport(dataGridView1);
        }
    }
}
