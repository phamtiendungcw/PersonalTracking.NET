using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.DTO;
using DAL;
using BLL;

namespace Personal_Tracking.NET
{
    public partial class FrmPermissionList : Form
    {
        public FrmPermissionList()
        {
            InitializeComponent();
        }

        private void txtUserNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void txtDayAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = General.isNumber(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FrmPermission frm = new FrmPermission();
            this.Hide();
            frm.ShowDialog();
            this.Visible = true;
            FillAllData();
            CleanFilters();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (detail.PermissionID == 0)
                MessageBox.Show("Hãy lựa chọn một quyền từ bảng!");
            else if (detail.State == PermissionStates.Approved || detail.State == PermissionStates.Disapproved)
                MessageBox.Show("Bạn không thể cập nhật khi đã phê duyệt hoặc không chấp thuận quyền này!");
            else
            {
                FrmPermission frm = new FrmPermission();
                frm.isUpdate = true;
                frm.detail = detail;
                this.Hide();
                frm.ShowDialog();
                this.Visible = true;
                FillAllData();
                CleanFilters();
            }
        }

        PermissionDTO dto = new PermissionDTO();
        private bool combofull = false;

        void FillAllData()
        {
            dto = PermissionBLL.GetAll();
            if (!UserStatic.isAdmin)
                dto.Permissions = dto.Permissions.Where(x => x.EmployeeID == UserStatic.EmployeeID).ToList();
            dataGridView1.DataSource = dto.Permissions;
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
            cmbState.DataSource = dto.States;
            cmbState.DisplayMember = "StateName";
            cmbState.ValueMember = "ID";
            cmbState.SelectedIndex = -1;
        }

        private void FrmPermissionList_Load(object sender, EventArgs e)
        {
            FillAllData();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Mã số nhân viên";
            dataGridView1.Columns[2].HeaderText = "Họ";
            dataGridView1.Columns[3].HeaderText = "Tên";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].HeaderText = "Ngày bắt đầu";
            dataGridView1.Columns[9].HeaderText = "Ngày kết thúc";
            dataGridView1.Columns[10].HeaderText = "Tổng số ngày";
            dataGridView1.Columns[11].HeaderText = "Trạng thái";
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;
            if (!UserStatic.isAdmin)
            {
                pnlForAdmin.Visible = false;
                btnApprove.Hide();
                btnDisaprove.Hide();
                btnDelete.Hide();
                btnClose.Location = new Point(539, 36);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<PermissionDetailDTO> list = dto.Permissions;
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
                    x.StartDate < Convert.ToDateTime(dtpEnd.Value) &&
                    x.StartDate > Convert.ToDateTime(dtpStart.Value)).ToList();
            else if (rbEndDate.Checked)
                list = list.Where(x =>
                    x.EndDate < Convert.ToDateTime(dtpEnd.Value) &&
                    x.EndDate > Convert.ToDateTime(dtpStart.Value)).ToList();
            if (cmbState.SelectedIndex != -1)
                list = list.Where(x => x.State == Convert.ToInt32(cmbState.SelectedValue)).ToList();
            if (txtDayAmount.Text.Trim() != "")
                list = list.Where(x => x.PermissionDayAmount == Convert.ToInt32(txtDayAmount.Text)).ToList();
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
            rbStartDate.Checked = false;
            rbEndDate.Checked = false;
            cmbState.SelectedIndex = -1;
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;
            txtDayAmount.Clear();
            dataGridView1.DataSource = dto.Permissions;
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combofull)
            {
                cmbPosition.DataSource = dto.Positions.Where(x => x.DepartmentID == Convert.ToInt32(cmbDepartment.SelectedValue)).ToList();
            }
        }

        PermissionDetailDTO detail = new PermissionDetailDTO();
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detail.UserNo = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            detail.StartDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[8].Value);
            detail.EndDate = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells[9].Value);
            detail.PermissionDayAmount = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[10].Value);
            detail.State = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[12].Value);
            detail.Explanation = dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString();
            detail.PermissionID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[14].Value);
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(detail.PermissionID, PermissionStates.Approved);
            MessageBox.Show("Cập nhật thành công!");
            FillAllData();
            CleanFilters();
        }

        private void btnDisapprove_Click(object sender, EventArgs e)
        {
            PermissionBLL.UpdatePermission(detail.PermissionID, PermissionStates.Disapproved);
            MessageBox.Show("Cập nhật thành công!");
            FillAllData();
            CleanFilters();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có chắc muốn xóa quyền này chứ?", "Cảnh báo!!!",
                MessageBoxButtons.YesNo);
            if (DialogResult.Yes == rs)
            {
                if (detail.State == PermissionStates.Approved || detail.State == PermissionStates.Disapproved)
                    MessageBox.Show("Bạn không thể xóa quyền đã được phê duyệt hoặc không chấp thuận!");
                else
                {
                    PermissionBLL.DeletePermission(detail.PermissionID);
                    MessageBox.Show("Xóa thành công!");
                    FillAllData();
                    CleanFilters();
                }
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel.ExcelExport(dataGridView1);
        }
    }
}
