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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Check_date();
        }
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Andrey\Documents\Hostel_DataBase\Hostel.mdf;Integrated Security=True;Connect Timeout=30");
        DataTable CHECK = new DataTable();
        //Елементи, необхідні для переміщення вікна--------------------------------------------------------------
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //-------------------------------------------------------------------------------------------------------
        //Перехід по пунктам меню--------------------------------------------------------------------------------
        int Previous_Tab = 2;
        public void Change_Tab(int r, int g, int b, Form frm, int tab_number)
        {
                ((Panel)Controls.Find("panel" + Previous_Tab, true)[0]).BackColor = Color.FromArgb(60, 58, 69);
                ((Label)Controls.Find("label" + Previous_Tab, true)[0]).ForeColor = Color.FromArgb(110, 110, 110);
                ((Panel)Controls.Find("panel" + tab_number, true)[0]).BackColor = Color.FromArgb(r,g,b);
                ((Label)Controls.Find("label" + tab_number, true)[0]).ForeColor = Color.White;
               // SASDASFADSGSFGH GHJ JKLKL;''
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.WindowState = FormWindowState.Maximized;
                panel1.Visible = false;
                label1.Visible = true;
                this.Refresh();
                panel1.Controls.Add(frm);
                frm.Show();
                System.Threading.Thread.Sleep(300);
                label1.Visible = false;
                panel1.Visible = true;
                Previous_Tab = tab_number;
        }
        //------------------------------------------------------------------------------------------------------- 
        private void button1_Click(object sender, EventArgs e)
        {
            Account_Settlement f = new Account_Settlement();
            f.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        string Get_Date()
        {
            sqlCon.Open();
            SqlCommand cm = new SqlCommand("Select * from dbo.PreviousData", sqlCon);
            SqlDataReader myReader;
            string date="";
            myReader = cm.ExecuteReader();
                while (myReader.Read())
                {
                    date = myReader.GetString(0);
                }
            sqlCon.Close();
            MessageBox.Show("" + date, "date");
            return date;
        }
        void Check_date()
        {
            sqlCon.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter(
            "SELECT Accounting.[Код угоди],Accounting.[Місяців прожито],"+ 
            "Accounting.[Поточний борг],Deal.[Номер кімнати],Deal.[Дата виїзду],Rooms.[Ціна за місяць]"+
            "FROM Accounting INNER JOIN Deal ON Accounting.[Код угоди]=Deal.[Код студента]"+ 
            "INNER JOIN Rooms ON Deal.[Номер кімнати]=Rooms.[Номер кімнати]", sqlCon);
            sqlData.SelectCommand.CommandType = CommandType.Text;
            CHECK.Clear();
            sqlData.Fill(CHECK);
            sqlCon.Close();
        }
        void Set_Live_Month(int difference,int i)
        {
            try
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("Live_Month", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@P", difference);
                sqlCmd.Parameters.AddWithValue("@B", difference * Convert.ToInt32(CHECK.Rows[i]["Ціна за місяць"]));
                sqlCmd.Parameters.AddWithValue("@ID",  Convert.ToInt32(CHECK.Rows[i]["Код угоди"]));
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }
        void Set_New_Date(string day, string month, string years)
        {
            try
            {
                sqlCon.Open();
                if (day.Length == 1) day = "0"+ day;
                if (month.Length == 1) day = "0"+month;
                SqlCommand sqlCmd = new SqlCommand("New_PreviousDate", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                MessageBox.Show(day + "." + month + "." + years, "lolo");
                sqlCmd.Parameters.AddWithValue("@data",day + "." + month + "." + years);
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime date1 = DateTime.Today;                
                DateTime date2 = DateTime.ParseExact(Get_Date(), "dd.MM.yyyy", System.Globalization.CultureInfo.CurrentCulture);
                int difference = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
                MessageBox.Show(difference.ToString(), "Новий місяць");
                if (difference > 0)
                {
                    DateTime DATE = new DateTime();
                    for (int i = 0; i<CHECK.Rows.Count;i++)
                    {
                        int diff = difference;
                        DATE = DateTime.ParseExact(CHECK.Rows[i]["Дата виїзду"].ToString().Trim(), "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (DateTime.Today<DATE)
                        {
                            Set_Live_Month(difference, i);
                        }
                    }
                    MessageBox.Show("Застаріла інформація була оновлена", "Новий місяць");
                    Set_New_Date(DateTime.Today.Day.ToString(), DateTime.Today.Month.ToString(), DateTime.Today.Year.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!");
            }
            Check_date();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Account_Settlement f = new Account_Settlement();
            Change_Tab(44, 152, 224,f,2);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Payment f = new Payment();
            Change_Tab(254,208, 114, f, 4);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            Change_Tab(242,29, 1, f, 6);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Rooms_Form f = new Rooms_Form();
            Change_Tab(90,190,12, f, 8);
        }

        private void panel1_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //Переміщення вікна програми по робочому столу
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Groups f = new Groups();
            Change_Tab(174, 22, 197, f, 10);
        }
    }
}
