using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personal_Tracking.NET
{
    public partial class FrmPosition : Form
    {
        public FrmPosition()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private List<DEPARTMENT> departmentList = new List<DEPARTMENT>();
        private void FrmPosition_Load(object sender, EventArgs e)
        {
            departmentList = DepartmentBLL.GetDepartments();
            cmbDepartment.DataSource = departmentList;
            cmbDepartment.DisplayMember = "DepartmentName";
            cmbDepartment.ValueMember = "ID";
            cmbDepartment.SelectedIndex = -1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtPosition.Text.Trim() == "")
                MessageBox.Show("Mục chức vụ không được để trống!");
            else if (cmbDepartment.SelectedIndex == -1)
                MessageBox.Show("Hãy lựa chọn một phòng ban!");
            else
            {
                POSITION position = new POSITION();
                position.PositionName = txtPosition.Text;
                position.DepartmentID = Convert.ToInt32(cmbDepartment.SelectedValue);
                PositionBLL.AddPosition(position);
                MessageBox.Show("Lưu thành công!");
                txtPosition.Clear();
                cmbDepartment.SelectedIndex = -1;
            }
        }
    }
}
