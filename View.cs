using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Manager
{
    public partial class View : Form
    {
        List<PasswordClass> PasswordList = new List<PasswordClass>();
        public View()
        {
            InitializeComponent();
        }
        Point lastPoint;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void View_Load(object sender, EventArgs e)
        {
            
            SqlConnection conn = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Directory.GetCurrentDirectory()}\\Database1.mdf;Integrated Security=True");
            conn.Open();

            SqlCommand command = new SqlCommand("Select * from [Passwords]", conn);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateTime timestamp;
                    byte[] key = SimpleFernet.Base64UrlDecode("M9qFqrS3_tiBDsxb4hDQwMS1MknmkxiJgYbOIb4bcx8=");
                    var password = SimpleFernet.DecryptFernet(key, (string)reader.GetValue(5), out timestamp);
                    PasswordClass p = new PasswordClass();
                    p.ID = (int)reader.GetValue(0);
                    p.Site = (string)reader.GetValue(1);
                    p.Phone = (string)reader.GetValue(2);
                    p.Email = (string)reader.GetValue(3);
                    p.Username = (string)reader.GetValue(4);
                    p.Password = System.Text.Encoding.Default.GetString(password);
                    PasswordList.Add(p);
                }

            }

            conn.Close();
            dataGridView1.DataSource = PasswordList;
            dataGridView1.Columns[0].ReadOnly = true;

            // TODO: This line of code loads data into the 'database1DataSet.Passwords' table. You can move, or remove it, as needed.
            //this.passwordsTableAdapter.Fill(this.database1DataSet.Passwords);

            //
            //// FastColoredTextBox
            //

            //FileInfo file = new FileInfo("passwords.txt");
            //if (file.Length == 0)
            //    fastColoredTextBox1.Text = "No Passwords Yet!";
            //byte[] key = SimpleFernet.Base64UrlDecode("M9qFqrS3_tiBDsxb4hDQwMS1MknmkxiJgYbOIb4bcx8=");
            //DateTime timestamp;
            //foreach (var line in (File.ReadAllLines("passwords.txt")))
            //    fastColoredTextBox1.Text += System.Text.Encoding.Default.GetString(SimpleFernet.DecryptFernet(key, line + "\r\n", out timestamp));
            
            //
            //// FastColoredTextBox
            //
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            
            //PasswordClass p = (PasswordClass)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            //SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Desktop\\Net_Manager\\Database1.mdf;Integrated Security=True");
            //conn.Open();

            //SqlCommand command = new SqlCommand("UPDATE [Passwords] SET Site = @Site, Phone = @Phone, Email = @Email, Username = @Username, Password = @Password WHERE ID = @ID" , conn);
            //command.Parameters.AddWithValue("@Site", dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            //command.Parameters.AddWithValue("@Phone", dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            //command.Parameters.AddWithValue("@Email", dataGridView1.Rows[e.RowIndex].Cells[3].Value);
            //command.Parameters.AddWithValue("@Username", dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            //command.Parameters.AddWithValue("@Password", dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            //command.Parameters.AddWithValue("@ID", dataGridView1.Rows[e.RowIndex].Cells[0].Value);

            //command.ExecuteNonQuery();
            //Console.WriteLine((string)dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + " was updated");

            //conn.Close();
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            PasswordClass p = (PasswordClass)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            Update frmAdd = new Update();
            frmAdd.Tag = p;
            this.Close();
            frmAdd.ShowDialog();
        }
    }
}