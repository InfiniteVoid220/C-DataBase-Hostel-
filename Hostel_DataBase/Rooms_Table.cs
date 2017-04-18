using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Hostel_DataBase
{
    public partial class Rooms_Table : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Andrey\Documents\Hostel_DataBase\Hostel.mdf;Integrated Security=True;Connect Timeout=30");
        public Rooms_Table()
        {
            InitializeComponent();


        }
        private void Rooms_Table_Load_1(object sender, EventArgs e)
        {
            sqlCon.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("Free_Rooms", sqlCon);
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable Rooms = new DataTable();
            sqlData.Fill(Rooms);
            dataGridView1.DataSource = Rooms;
            sqlCon.Close();
            int i = 0;
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                i += c.Width;

            }
            dataGridView1.Width = i+50;
            dataGridView1.Height = dataGridView1.Rows[0].Height * (dataGridView1.Rows.Count+1);
            this.Size = new Size(dataGridView1.Width+10, dataGridView1.Height);
        }
    }
}
