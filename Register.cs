using System;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UAS_RPL
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string nama = inputNama.Text;
            string email = inputEmail.Text;
            string password = inputPassword.Text;
            string confirmPassword = inputConfirmPassword.Text;
            string role = radioUser.Checked ? "user" : radioAdmin.Checked ? "admin" : "";

            if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Password and confirm password do not match!");
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    DatabaseHelper.OpenConnection(conn);
                    string query = "INSERT INTO pengguna (nama, email, kata_sandi, peran) VALUES (@nama, @email, @password, @role)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@role", role);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Registration successful!");
                    DatabaseHelper.CloseConnection(conn);
                }

                this.Hide();
                Login loginForm = new Login();
                loginForm.FormClosed += (s, args) => this.Close(); 
                loginForm.Show();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

      
    }
}
