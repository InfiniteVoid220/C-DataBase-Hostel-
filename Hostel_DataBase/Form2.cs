using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Hostel_DataBase
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DataSet2.DataTable1' table. You can move, or remove it, as needed.
            
            this.DataTable1TableAdapter.Fill(this.DataSet2.DataTable1);
            this.reportViewer1.RefreshReport();
            reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            /*System.Drawing.Printing.PageSettings AlmostA4 = new System.Drawing.Printing.PageSettings();
            AlmostA4.PaperSize = new System.Drawing.Printing.PaperSize("CustomType", 17, 12);
            reportViewer1.SetPageSettings(AlmostA4);*/
        }
    }
}
