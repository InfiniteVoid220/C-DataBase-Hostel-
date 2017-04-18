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
    public partial class Account_Settlement : Form
    {
        //Приєднання бази данних
        SqlConnection sqlCon = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\Andrey\Documents\Hostel_DataBase\Hostel.mdf;Integrated Security=True;Connect Timeout=30");
        //Таблиці де будуть розміщуватись результати запитів.
        DataTable Student=new DataTable();
        DataTable Room = new DataTable();
        DataTable Deal = new DataTable();
        int StudRow= 0;
        public Account_Settlement()
        {
            InitializeComponent();
            Fill_Student(); Student_to_Form();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            Fill_Combobox();
        }
        void Hide_and_Revial_elements(bool x)
        {
            checkBox1.Checked = x;
            panel1.Visible = x;
            groupBox2.Visible = x;
            dataGridView1.Visible = x;
            label11.Visible = x;
            this.Refresh();
        }
        void Clear_Search()
        {
            textBox12.Text = ""; textBox13.Text = ""; textBox14.Text = ""; comboBox1.Text ="";
        }
        int Get_Previous_room()
        {
            sqlCon.Open();
            SqlCommand cm = new SqlCommand("Select * from dbo.Deal WHERE [Код студента]=" + Deal.Rows[0]["Код студента"], sqlCon);
            SqlDataReader myReader;
            myReader = cm.ExecuteReader();
            int rez = 0;
            while (myReader.Read())
            {
                rez = myReader.GetInt32(3);
            }
            sqlCon.Close();
            return rez;
        }
        bool Free_Room(int n)
        {
            sqlCon.Open();
            List<int> rez = new List<int>();
            SqlCommand cm = new SqlCommand("Select * from dbo.Rooms WHERE [Вільних місць]>0", sqlCon);
            SqlDataReader myReader;
                myReader = cm.ExecuteReader();
                while (myReader.Read())
                {
                    rez.Add(myReader.GetInt32(0));
                }
            sqlCon.Close();
                for (int i = 0; i < rez.Count; i++)
                    if (rez[i] == n)
                        return true;   
            return false;
        }
        void Fill_Combobox() 
        {
            sqlCon.Open();
            SqlCommand cm = new SqlCommand("Select * from dbo.Groups", sqlCon);
            SqlDataReader myReader;
            try
            {
                myReader = cm.ExecuteReader();
                while (myReader.Read())
                {
                    comboBox1.Items.Add(myReader.GetString(0));
                    comboBox2.Items.Add(myReader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error_Combo");
            }
            sqlCon.Close();
        }
        void Deal_to_DataGrid()
        {
            try
            {
            dataGridView1.Visible = false;
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Deal;
            dataGridView1.Refresh();
            dataGridView1.Columns[0].Visible = false; dataGridView1.Columns[1].Visible = false;
            dataGridView1.Visible = true;
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error_Deal!!");
            }
        }
        void Fiil_Deal(int StudID)
        {
                sqlCon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("Search_Deal_ByID", sqlCon);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("@ID",StudID);
                Deal.Clear();
                sqlData.Fill(Deal);
                Deal_to_DataGrid();
                sqlCon.Close();

        }
        void Room_to_DataGrid()
        {
            dataGridView1.Visible = false;
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource = Room;
            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn()
            {
                Name = "Телевізор"
            };
            dataGridView1.Columns.Insert(5, col);
            dataGridView1.Rows[0].Cells["Телевізор"].Value = Convert.ToBoolean(Room.Rows[0]["Телевізор"]);
            dataGridView1.Columns.RemoveAt(6);
            dataGridView1.Visible = true;
        }
        void Fiil_Room()
        {
            try
            {
                sqlCon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("Search_Rooms_ByID", sqlCon);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("@ID", Convert.ToInt32(Deal.Rows[0]["Номер кімнати"]));
                Room.Clear();
                sqlData.Fill(Room);
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error_Room");
            }
        }
        void Change_Room_size(int x,int number)
        {
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("Edit_Room");
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@number",number);
            sqlCmd.Parameters.AddWithValue("@x",x);
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
        }
        //Заповнення форми даними про студента
        void Fill_Student()
        {

                sqlCon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("Search_Student", sqlCon);
                sqlData.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlData.SelectCommand.Parameters.AddWithValue("@Group", comboBox1.Text.Trim());
                sqlData.SelectCommand.Parameters.AddWithValue("@SN", textBox12.Text.Trim());
                sqlData.SelectCommand.Parameters.AddWithValue("@N", textBox13.Text.Trim());
                sqlData.SelectCommand.Parameters.AddWithValue("@O", textBox14.Text.Trim());
                Student.Clear();
                sqlData.Fill(Student);
                sqlCon.Close();
        }
        void Student_to_Form()
        {
            try
            {
                sqlCon.Open();
                textBox1.Text = (Student.Rows[StudRow]["Прізвище"]).ToString().Trim();
                textBox2.Text = (Student.Rows[StudRow]["Ім_я"]).ToString().Trim();
                textBox3.Text = (Student.Rows[StudRow]["По батькові"]).ToString().Trim();
                textBox4.Text = (Student.Rows[StudRow]["Дата народження"]).ToString().Substring(0, 10);
                textBox5.Text = (Student.Rows[StudRow]["Номер Паспорта"]).ToString().Trim();
                textBox6.Text = (Student.Rows[StudRow]["Телефон"]).ToString().Trim();
                comboBox2.Text = (Student.Rows[StudRow]["Назва групи"]).ToString().Trim();
                textBox7.Text = (Student.Rows[StudRow]["Спеціальність"]).ToString().Trim();
                textBox8.Text = (Student.Rows[StudRow]["Прописка"]).ToString().Trim();
                checkBox1.Checked = Convert.ToInt32(Student.Rows[StudRow]["Стипендія"]) == 1 ? true : false;
                sqlCon.Close();
                Fiil_Deal(Convert.ToInt32(Student.Rows[StudRow]["Код студента"]));
                Fiil_Room();
                label17.Text = (StudRow + 1) + "/" + Student.Rows.Count;
                label11.Text = "Угода";
            }
            catch 
            { 
                MessageBox.Show("Нічого не знайдено", "Error");
                sqlCon.Close(); 
            }
        }
        void Slide_Animation(int d,GroupBox g)
        {
            if (d == 1)
            {
                g.Visible = true;
                this.Refresh();
            }
                for (int i = 0; i < 520; i++)
                    g.Location = new Point(g.Location.X-(1*d), g.Location.Y);
                if (d == -1)
                {
                    g.Visible =false;
                    this.Refresh();
                }
        }
        private void Account_Settlement_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (label11.Text!="Кімната")
            {
                Room_to_DataGrid();
                label11.Text = "Кімната";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label11.Text != "Угода")
            {
                Deal_to_DataGrid();
                label11.Text = "Угода";
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            StudRow = 0;
            Fill_Student();Student_to_Form();
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Rooms_Table f = new Rooms_Table();
            f.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (groupBox1.Visible == true)
            {
                Slide_Animation(groupBox1.Visible == true ? -1 : 1, groupBox1);
                Slide_Animation(groupBox2.Visible == true ? -1 : 1, groupBox2);
                button7.Text = "Пошук";
            }
            else
            {
                Slide_Animation(groupBox2.Visible == true ? -1 : 1, groupBox2);
                Slide_Animation(groupBox1.Visible == true ? -1 : 1, groupBox1);
                button7.Text = "Інше";
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (StudRow < Student.Rows.Count - 1)
            {
                StudRow++;
            }
            else StudRow = 0;
            Student_to_Form();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (StudRow>0)
            {
                StudRow--;
            }
            else StudRow = Student.Rows.Count-1;
            Student_to_Form();
        }
        bool Check_Deal_Form()
        {
            bool show=true;
            for (int i = 9; i <=11; i++)
                ((TextBox)Controls.Find("textBox" + i, true)[0]).BackColor = Color.White;
            comboBox2.BackColor = Color.White;
            for (int i = 9; i <=11; i++)
            {
                TextBox t = ((TextBox)Controls.Find("textBox" + i, true)[0]);
                if (t.Text.Trim() == "")
                {
                    if (show)
                    {
                        MessageBox.Show("Заповніть всі поля!");
                        show = false;
                    }
                    t.BackColor = Color.DarkRed;
                }
            }
            if (!show) return false;
            if (!Regex.IsMatch(textBox10.Text.Trim(), @"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"))
            {
                MessageBox.Show("Введіть дату в вигляді dd-mm-yyyy (12.05.2016)", "Помилка при наборі");
                textBox10.BackColor = Color.DarkRed; return false;
            }
            if (!Regex.IsMatch(textBox11.Text.Trim(), @"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"))
            {
                MessageBox.Show("Введіть дату в вигляді dd-mm-yyyy (12.05.2016)", "Помилка при наборі");
                textBox11.BackColor = Color.DarkRed; return false;
            }
            if (!Regex.IsMatch(textBox9.Text.Trim(), @"^[0-9]+$") || !Free_Room(Convert.ToInt32(textBox9.Text.Trim())))
            {
                MessageBox.Show("Номер кімнати не число, або У кімнаті не має вільних місць", "Помилка при наборі");
                textBox9.BackColor = Color.DarkRed; return false;
            }
            return true;
        }
        bool Check_Form()
        {
            bool show = true;
            for (int i = 1; i <= 8; i++)
                ((TextBox)Controls.Find("textBox" + i, true)[0]).BackColor = Color.White;
            comboBox2.BackColor = Color.White;
            for (int i = 1; i <= 8; i++)
            {
                TextBox t = ((TextBox)Controls.Find("textBox" + i, true)[0]);
                if (t.Text.Trim() == "")
                {
                    if (show)
                    {
                        MessageBox.Show("Заповніть всі поля!");
                        show = false;
                    }
                    t.BackColor = Color.DarkRed;
                }
            }
            if (comboBox2.Text.Trim() == "") { show = false; comboBox2.BackColor = Color.DarkRed; }
            if (!show) return false;
            for (int i = 1; i <= 3; i++)
            {
                TextBox t = ((TextBox)Controls.Find("textBox" + i, true)[0]);
                if (!Regex.IsMatch(t.Text.Trim(), @"^([a0zA-Z]|[а-яА-Я]|[іїє])+$"))
                {
                    if (show)
                    {
                        MessageBox.Show("В Імені, Фамілії та імені по батькові дозволено використовувати лише ЛІТЕРИ", "Помилка при наборі");
                        show = false;
                    }
                    t.BackColor = Color.DarkRed;
                }
            }
            if (!show) return false;;
            if (!Regex.IsMatch(textBox4.Text.Trim(), @"^(0?[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$"))
            {
                MessageBox.Show("Введіть дату в вигляді dd-mm-yyyy (12.05.2016)", "Помилка при наборі");
                textBox4.BackColor = Color.DarkRed; return false;
            }
            return true;
        }
        void Add_deal(string mode="Add")
        {
            sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("Deal_Add_Edit");
            sqlCmd.Connection = sqlCon;
            string Today = "";
            if (DateTime.Today.Day < 9) Today += ("0" + DateTime.Today.Day + ".");
            else Today += (DateTime.Today.Day + ".");
            if (DateTime.Today.Month < 9) Today += ("0"+DateTime.Today.Month + ".");
            else Today += (DateTime.Today.Month + ".");
            Today+=DateTime.Today.Year.ToString();
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@mode", mode);
            if (mode=="Add")
            {
                sqlCmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(Student.Rows[Student.Rows.Count-1]["Код студента"]));
                sqlCmd.Parameters.AddWithValue("@Date_Deal", Today);
                sqlCmd.Parameters.AddWithValue("@Room", textBox9.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Date_Start", textBox10.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Date_Finish", textBox11.Text.Trim());
            }
            else
            {
                sqlCmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(Student.Rows[StudRow]["Код студента"]));
                sqlCmd.Parameters.AddWithValue("@Date_Deal",(dataGridView1.Rows[0].Cells[2].Value.ToString()).Trim());
                sqlCmd.Parameters.AddWithValue("@Room", Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value.ToString().Trim()));
                sqlCmd.Parameters.AddWithValue("@Date_Start", (dataGridView1.Rows[0].Cells[4].Value.ToString()).Trim());
                sqlCmd.Parameters.AddWithValue("@Date_Finish",(dataGridView1.Rows[0].Cells[5].Value.ToString()).Trim());
            }
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
        }
        void Add_and_Edit_Student(string mode="Add",int ID=0)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("StudentsAdd_Edit");
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@mode", mode);
            sqlCmd.Parameters.AddWithValue("@StudentID", ID);
            sqlCmd.Parameters.AddWithValue("@N", textBox2.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@SN", textBox1.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@O", textBox3.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@t", textBox6.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@g", comboBox2.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@s", checkBox1.Checked == true ? 1 : 0);
            sqlCmd.Parameters.AddWithValue("@d", textBox4.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@spec", textBox7.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@Adress", textBox8.Text.Trim());
            sqlCmd.Parameters.AddWithValue("@p", textBox5.Text.Trim());
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
        }
        void Add_and_Edit_Accounting(string mode = "Add", int ID = 0)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("Accounting_Add_and_Edit");
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@mode", mode);
            sqlCmd.Parameters.AddWithValue("@ID", ID);
            sqlCmd.Parameters.AddWithValue("@P", 1);
            sqlCmd.Parameters.AddWithValue("@O",1);
            sqlCmd.Parameters.AddWithValue("@Pay",0);
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (Check_Form() && Check_Deal_Form())
            try
            {
                Clear_Search();
                Fill_Student();
                Add_and_Edit_Student("Add");
                button11.PerformClick();
                Fill_Student(); Student_to_Form();
                label17.Text = (StudRow + 1) + "/" + Student.Rows.Count;
                Add_deal();
                Change_Room_size(-1, Convert.ToInt32(textBox9.Text.Trim()));
                Add_and_Edit_Accounting("Add", Convert.ToInt32(Student.Rows[Student.Rows.Count - 1]["Код студента"]));
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message,"Ошибка!");
            }
        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            for (int i = 1; i <= 8; i++)
                ((TextBox)Controls.Find("textBox" + i, true)[0]).Text = "";
            comboBox2.Text = "";
            Hide_and_Revial_elements(false);
            groupBox3.Visible=true;
            this.Refresh();
            Slide_Animation(-1, groupBox3);
            groupBox3.Visible = true;  
        }

        private void button11_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 11; i++)
                ((TextBox)Controls.Find("textBox" + i, true)[0]).BackColor = Color.White;
            Slide_Animation(1, groupBox3);
            groupBox3.Visible = false;
            Hide_and_Revial_elements(true);
            Student_to_Form();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (label11.Text == "Угода")
            {
                if (Get_Previous_room().ToString().Trim() != dataGridView1.Rows[0].Cells[3].Value.ToString().Trim())
                {
                    if (!Regex.IsMatch(dataGridView1.Rows[0].Cells[3].Value.ToString().Trim(), @"^[0-9]+$") || !Free_Room(Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value.ToString().Trim())))
                    {
                        MessageBox.Show("Номер кімнати не число, або У кімнаті не має вільних місць", "Помилка при наборі");
                         return;
                    }
                }
                Change_Room_size(1, Get_Previous_room());
                Add_deal("Edit");
                Fill_Student(); Student_to_Form();
                Change_Room_size(-1, Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value));
            }
            if (Check_Form())
            {
                Add_and_Edit_Student("Edit", Convert.ToInt32(Student.Rows[StudRow]["Код студента"]));
                Fill_Student(); Student_to_Form();
            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
            SqlCommand sqlCmd = new SqlCommand("DeleteStudent_And_Deal");
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@StudentID", Convert.ToInt32(Student.Rows[StudRow]["Код студента"]));
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
            button2.PerformClick();
            Change_Room_size(1,Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value));
            StudRow=0;
            Fill_Student(); Student_to_Form();

        }

        private void dataGridView1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        }
}
