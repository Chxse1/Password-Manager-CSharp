using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manager
{
    public partial class Add : Form
    {
        List<PasswordClass> PasswordList = new List<PasswordClass>();
        public Add()
        {
            InitializeComponent();
        }
        Point lastPoint;

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
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

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            //PasswordClass p = (PasswordClass)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            SqlConnection conn = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Directory.GetCurrentDirectory()}\\Database1.mdf;Integrated Security=True");
            conn.Open();

            DateTime timestamp = DateTime.Now;
            byte[] key = SimpleFernet.Base64UrlDecode("M9qFqrS3_tiBDsxb4hDQwMS1MknmkxiJgYbOIb4bcx8=");
            byte[] data = Encoding.ASCII.GetBytes(maskedTextBox1.Text);
            var password = SimpleFernet.EncryptFernet(key, data, timestamp);

            SqlCommand command = new SqlCommand("INSERT INTO Passwords (Site, Phone, Email, Username, Password) VALUES (@Site, @Phone, @Email, @Username, @Password)", conn);
            command.Parameters.AddWithValue("@Site", textBox1.Text);
            command.Parameters.AddWithValue("@Phone", textBox2.Text);
            command.Parameters.AddWithValue("@Email", textBox3.Text);
            command.Parameters.AddWithValue("@Username", textBox4.Text);
            command.Parameters.AddWithValue("@Password", password);
            command.ExecuteNonQuery();

            conn.Close();
            MessageBox.Show("Password Added!", "Password Manager!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}