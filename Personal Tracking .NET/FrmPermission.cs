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
using DAL;

namespace Personal_Tracking.NET
{
    public partial class FrmPermission : Form
    {
        public FrmPermission()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private TimeSpan PermissionDay;
        private void FrmPermission_Load(object sender, EventArgs e)
        {
            txtUserNo.Text = UserStatic.UserNo.ToString();
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            PermissionDay = dtpEnd.Value.Date - dtpStart.Value.Date;
            txtDayAmount.Text = PermissionDay.TotalDays.ToString();
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            PermissionDay = dtpEnd.Value.Date - dtpStart.Value.Date;
            txtDayAmount.Text = PermissionDay.TotalDays.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtDayAmount.Text.Trim() == "")
                MessageBox.Show("Hãy thay đổi ngày kết thúc hoặc ngày bắt đầu!");
            else if (Convert.ToInt32(txtDayAmount.Text) <= 0)
                MessageBox.Show("Tổng số ngày phải lớn hơn 0!");
            else if (txtExplanation.Text.Trim() == "")
                MessageBox.Show("Chú giải không được để trống!");
            else
            {
                PERMISSION permission = new PERMISSION();
                permission.EmployeeID = UserStatic.EmployeeID;
                permission.PermissionState = 1;
                permission.PermissionStartDate = dtpStart.Value.Date;
                permission.PermissionEndDate = dtpEnd.Value.Date;
                permission.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
                permission.PermissionExplanation = txtExplanation.Text;
                PermissionBLL.AddPermission(permission);
                MessageBox.Show("Quyền đã được thêm");
                permission = new PERMISSION();
                dtpStart.Value = DateTime.Today;
                dtpEnd.Value = DateTime.Today;
                txtDayAmount.Clear();
                txtExplanation.Clear();
            }
        }
    }
}
