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
using System.Text.RegularExpressions;
namespace Hostel_DataBase
{
    public partial class Payment : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Andrey\Documents\Hostel_DataBase\Hostel.mdf;Integrated Security=True;Connect Timeout=30");
        public Payment()
        {
            InitializeComponent();
            Refill_Combobox();
           // MessageBox.Show(dataGridView1.Columns[], "Помилка"); 
        }
        DataTable Info = new DataTable();
        DataTable Room = new DataTable();
        void Fill_Combobox(int row, ComboBox c)
        {
            c.Items.Clear();
            sqlCon.Open();
            SqlCommand cm = new SqlCommand("Search_Student", sqlCon);
            cm.CommandType = CommandType.StoredProcedure;
            cm.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "N", Value = label3.Text.Trim() });
            cm.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "SN", Value = label4.Text.Trim() });
            cm.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "O", Value = label5.Text.Trim() });
            cm.Parameters.Add(new SqlParameter { SqlDbType = SqlDbType.NVarChar, ParameterName = "Group", Value = "" });
            SqlDataReader myReader;
            try
            {
                myReader = cm.ExecuteReader();
                while (myReader.Read())
                {
                    c.Items.Add(myReader.GetString(row));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error_Combo");
            }
            c.Refresh();
            sqlCon.Close();
        }

        void Refill_Combobox()
        {
            Fill_Combobox(2, comboBox1); Fill_Combobox(1, comboBox2); Fill_Combobox(3, comboBox3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox1.Text = ""; comboBox2.Text = ""; comboBox3.Text = "";
            label3.Text = ""; label4.Text = ""; label5.Text = ""; textBox6.Text = "";
            Refill_Combobox();
            if (groupBox2.Visible) Slide_Animation(1, groupBox2);
        }

        private void comboBox1_TabIndexChanged(object sender, EventArgs e)
        {
            Refill_Combobox();
        }

        private void comboBox1_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            Refill_Combobox();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label3.Text = comboBox1.Text.Trim();
            Refill_Combobox();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = comboBox2.Text.Trim();
            Refill_Combobox();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            label5.Text = comboBox3.Text.Trim();
            Refill_Combobox();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        void Fiil_form()
        {
            textBox1.Text = Info.Rows[0]["Дата в'їзду"].ToString();
            textBox2.Text = Info.Rows[0]["Дата виїзду"].ToString();
            textBox3.Text = Info.Rows[0]["Місяців прожито"].ToString();
            textBox4.Text = Info.Rows[0]["Місяців оплачено"].ToString();
            textBox5.Text = Info.Rows[0]["Поточний борг"].ToString();
        }
        void Room_Info()
        {
            try
            {
                sqlCon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("Search_Rooms_ByID", sqlCon);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("@ID", Convert.ToInt32(Info.Rows[0]["Код студента"]));
                Room.Clear();
                sqlData.Fill(Room);
                dataGridView1.Visible = false;
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = Room;
                DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn()
                {
                    Name = "Телевізор"
                };
                dataGridView1.Columns.Insert(5, col);
                dataGridView1.Rows[0].Cells["Телевізор"].Value = Convert.ToBoolean(Room.Rows[0]["Телевізор"]);
                dataGridView1.Columns[6].Visible = false ;
                dataGridView1.Visible = true;
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error_Room");
            }
           
        }
        void Slide_Animation(int d, GroupBox g)
        {
            if (d == -1)
            {
                g.Visible = true;
                this.Refresh();
            }
            for (int i = 0; i < 656; i++)
            {
                g.Location = new Point(g.Location.X - (1 * d), g.Location.Y);
            }
            if (d == 1)
            {
                g.Visible = false;
                this.Refresh();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (label3.Text != "" && label4.Text != "" && label5.Text != "")
            {
                sqlCon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("Payment_Info", sqlCon);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("@N", label3.Text.Trim());
                sqlData.SelectCommand.Parameters.AddWithValue("@SN", label4.Text.Trim());
                sqlData.SelectCommand.Parameters.AddWithValue("@O", label5.Text.Trim());
                Info.Clear();
                sqlData.Fill(Info);
                sqlCon.Close();
                Fiil_form();
                Room_Info();
                textBox6.Text = "";
                if(!groupBox2.Visible)Slide_Animation(-1, groupBox2);
            }
            else MessageBox.Show("Заповніть всі поля", "Пусті поля!");
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((Convert.ToInt32(textBox6.Text.Trim())+Convert.ToInt32(textBox4.Text.Trim()))>Convert.ToInt32(textBox3.Text.Trim()))
            {
                MessageBox.Show("Оплата наперед не передбачена!", "Помилка"); return;
            }
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("Payment_Income", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(Info.Rows[0]["Код студента"]));
            sqlCmd.Parameters.AddWithValue("@O", Convert.ToInt32(textBox6.Text.Trim()));
            sqlCmd.Parameters.AddWithValue("@P", Convert.ToInt32(textBox3.Text.Trim()));
            sqlCmd.Parameters.AddWithValue("@B", Convert.ToInt32(textBox5.Text.Trim()) - Convert.ToInt32(Room.Rows[0]["Ціна за місяць"]) * Convert.ToInt32(textBox6.Text.Trim()));
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
            button1.PerformClick();
        }

        private void Payment_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
