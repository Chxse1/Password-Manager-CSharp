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
using System.IO;

namespace Manager
{
    public partial class Delete : Form
    {
        List<PasswordClass> PasswordList = new List<PasswordClass>();
        public Delete()
        {
            InitializeComponent();
        }
        Point lastPoint;

        private void button2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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

        private void Delete_Load(object sender, EventArgs e)
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
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            PasswordClass p = new PasswordClass();
            p = (PasswordClass)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            SqlConnection conn = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Directory.GetCurrentDirectory()}\\Database1.mdf;Integrated Security=True");
            conn.Open();
            SqlCommand command = new SqlCommand("DELETE FROM [Passwords] WHERE ID = @ID", conn);
            command.Parameters.AddWithValue("@ID", p.ID);
            command.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Password Deleted!", "Password Manager!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
