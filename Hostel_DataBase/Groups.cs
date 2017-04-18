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
    public partial class Groups : Form
    {
        public Groups()
        {
            InitializeComponent();
            Fill_Combobox();
            //Load_info();
        }
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Andrey\Documents\Hostel_DataBase\Hostel.mdf;Integrated Security=True;Connect Timeout=30");
        DataTable groups = new DataTable();
        void Fill_Combobox()
        {
            sqlCon.Open();
            SqlCommand cm = new SqlCommand("Select * from dbo.Groups", sqlCon);
            SqlDataReader myReader;
                myReader = cm.ExecuteReader();
                while (myReader.Read())
                {
                    comboBox2.Items.Add(myReader.GetString(0));
                }
            sqlCon.Close();
        }
        void Load_info()
        {
            sqlCon.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter(
            "SELECT Stud.[Назва групи],Stud.[Спеціальність],Groups.[По батькові Куратора],Groups.[Прізвище Куратора],Groups.[Ім_я Куратора],"+
            "Groups.[Прізвище Старости],Groups.[Ім_я Старости],Groups.[По батькові Старости],Groups.[Курс]"+
            "FROM Groups INNER JOIN Stud ON Groups.[Назва групи]=Stud.[Назва групи] WHERE Groups.[Назва групи] LIKE '%'+@G+'%'", sqlCon);
            sqlData.SelectCommand.CommandType = CommandType.Text;
            sqlData.SelectCommand.Parameters.AddWithValue("@G",comboBox2.Text.Trim());
            groups.Clear();
            sqlData.Fill(groups); 
            sqlCon.Close();
            Info_to_Form();
        }
       /* void Dell_g()
        {
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("DeleteGroup");
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@ID",comboBox2.Text.Trim());
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
        }*/
        void Add_g()
        {
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("Add_Groups", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@NG", textBox1.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@K", textBox9.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@PK", textBox2.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@IK", textBox3.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@OK", textBox4.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@PS", textBox5.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@IS", textBox6.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@OS", textBox7.Text.Trim());
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
        }
        void Info_to_Form()
        {
            try
            {
                textBox2.Text = groups.Rows[0]["Прізвище куратора"].ToString();
                textBox3.Text = groups.Rows[0]["Ім_я куратора"].ToString();
                textBox4.Text = groups.Rows[0]["По батькові куратора"].ToString();
                textBox5.Text = groups.Rows[0]["Прізвище старости"].ToString();
                textBox6.Text = groups.Rows[0]["Ім_я старости"].ToString();
                textBox7.Text = groups.Rows[0]["По батькові старости"].ToString();
                textBox1.Text = groups.Rows[0]["Назва групи"].ToString();
                textBox8.Text = groups.Rows[0]["Спеціальність"].ToString();
                textBox8.Text = groups.Rows[0]["Спеціальність"].ToString();
                textBox9.Text = groups.Rows[0]["Курс"].ToString();
            }
            catch { MessageBox.Show("В групі немає студентів","Додайте студента!"); }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_g();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_info();  //button5.Visible = true; 
            button1.Visible = true; groupBox1.Visible = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
