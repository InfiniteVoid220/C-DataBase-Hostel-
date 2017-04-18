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
    public partial class Rooms_Form : Form
    {
        public Rooms_Form()
        {
            InitializeComponent();
        }
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Andrey\Documents\Hostel_DataBase\Hostel.mdf;Integrated Security=True;Connect Timeout=30");
        DataTable Room = new DataTable();
        DataTable Stud = new DataTable();
        void Fill_Combobox()
        {
            sqlCon.Open();
            SqlCommand cm = new SqlCommand("Select dbo.Rooms.[Номер кімнати]  from dbo.Rooms", sqlCon);
            SqlDataReader myReader;
            try
            {
                myReader = cm.ExecuteReader();
                while (myReader.Read())
                {
                    comboBox2.Items.Add(myReader.GetInt32(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error_Combo");
            }
            sqlCon.Close();
        }
        void Load_info()
        {
            sqlCon.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("Find_Room", sqlCon);
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlData.SelectCommand.Parameters.AddWithValue("@N", Convert.ToInt32(comboBox2.Text.Trim()));
            Room.Clear();
            sqlData.Fill(Room);
            dataGridView1.DataSource=Room;
            Info_to_Form();
            sqlCon.Close();
        }
        void Load_Students()
        {
            sqlCon.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("Students_in_Room", sqlCon);
            sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlData.SelectCommand.Parameters.AddWithValue("@N", Convert.ToInt32(comboBox2.Text.Trim()));
            Stud.Clear();
            sqlData.Fill(Stud);
            dataGridView1.DataSource = Stud;
            dataGridView1.Columns[5].Visible = false; dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[4].Visible = false; dataGridView1.Columns[0].Visible = false;
            sqlCon.Close();
        }
        void Edit_Rooms()
        {
            sqlCon.Open();
            try
            {
                SqlCommand sqlCmd = new SqlCommand("Edit_Roms_ByID", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                MessageBox.Show(Convert.ToInt32(comboBox2.Text.Trim()).ToString(), " ");
                sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(comboBox2.Text.Trim()));
                sqlCmd.Parameters.AddWithValue("@KM", Convert.ToInt32(textBox2.Text.Trim()));
                sqlCmd.Parameters.AddWithValue("@VM", Convert.ToInt32(textBox3.Text.Trim()));
                sqlCmd.Parameters.AddWithValue("@S", Convert.ToInt32(textBox4.Text.Trim()));
                sqlCmd.Parameters.AddWithValue("@H", Convert.ToInt32(textBox5.Text.Trim()));
                sqlCmd.Parameters.AddWithValue("@T", checkBox1.Checked == true ? 1 : 0);
                sqlCmd.Parameters.AddWithValue("@MONEY", Convert.ToInt32(textBox6.Text.Trim()));
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error_Combo");
            } 
            sqlCon.Close();
            Load_info(); Load_Students();
        }
        void Info_to_Form()
        {
            textBox1.Text = Room.Rows[0]["Номер кімнати"].ToString();
            textBox2.Text = Room.Rows[0]["Кількість місць"].ToString();
            textBox3.Text = Room.Rows[0]["Вільних місць"].ToString();
            textBox4.Text = Room.Rows[0]["Столів"].ToString();
            textBox5.Text = Room.Rows[0]["Шаф"].ToString();
            textBox6.Text = Room.Rows[0]["Ціна за місяць"].ToString();
            checkBox1.Checked = Convert.ToInt32(Room.Rows[0]["Телевізор"]) == 1 ? true : false;
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Rooms_Form_Load(object sender, EventArgs e)
        {
            Fill_Combobox();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_info(); Load_Students();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Edit_Rooms();
        }
    }
}
