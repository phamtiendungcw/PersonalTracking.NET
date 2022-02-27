using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personal_Tracking.NET
{
    public class ExportToExcel
    {
        public static void ExcelExport(DataGridView dataGridView)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            //worksheet = workbook.Sheets["Trang_tính1"];     // Language = vn
            worksheet = workbook.Sheets["Sheet1"];          // Language = en
            worksheet = workbook.ActiveSheet;
            for (int i = 1; i < dataGridView.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dataGridView.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView.Rows[i].Cells[j].Value.ToString();
                }
            }

            //workbook.SaveAs("D:\\List.xlsx");
            //workbook.Close();
        }
    }
}
