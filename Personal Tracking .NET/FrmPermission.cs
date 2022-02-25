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
using DAL.DTO;

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

        TimeSpan PermissionDay;
        public bool isUpdate = false;
        public PermissionDetailDTO detail = new PermissionDetailDTO();
        private void FrmPermission_Load(object sender, EventArgs e)
        {
            txtUserNo.Text = UserStatic.UserNo.ToString();
            if (isUpdate)
            {
                dtpStart.Value = detail.StartDate;
                dtpEnd.Value = detail.EndDate;
                txtDayAmount.Text = detail.PermissionDayAmount.ToString();
                txtExplanation.Text = detail.Explanation;
                txtUserNo.Text = detail.UserNo.ToString();
            }
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
                if (!isUpdate)
                {
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
                else if (isUpdate)
                {
                    DialogResult rs = MessageBox.Show("Bạn có chắc muốn cập nhật chứ?", "Cảnh báo!", MessageBoxButtons.YesNo);
                    if (rs == DialogResult.Yes)
                    {
                        permission.ID = detail.PermissionID;
                        permission.PermissionExplanation = txtExplanation.Text;
                        permission.PermissionStartDate = dtpStart.Value;
                        permission.PermissionEndDate = dtpEnd.Value;
                        permission.PermissionDay = Convert.ToInt32(txtDayAmount.Text);
                        PermissionBLL.UpdatePermission(permission);
                        MessageBox.Show("Cập nhật thành công!");
                        Close();
                    }
                }
            }
        }
    }
}
