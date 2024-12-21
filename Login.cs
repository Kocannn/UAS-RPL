using System;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UAS_RPL
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
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

        private void metroLabel2_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            registerForm.Show();
            this.Hide();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            string nama = inputNama.Text;
            string password = inputPassword.Text;
            string role = radioUser.Checked ? "user" : radioAdmin.Checked ? "admin" : "";

            if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    DatabaseHelper.OpenConnection(conn);
                    string query = "SELECT id FROM pengguna WHERE nama = @nama AND kata_sandi = @password AND peran = @role";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nama", nama);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);
                    cmd.Parameters.AddWithValue("@role", role);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int userId = Convert.ToInt32(result);
                        MessageBox.Show("Login successful!");

                        // Navigasi ke form utama sesuai role
                        this.Hide();
                        if (role == "admin")
                        {
                            AdminMain adminForm = new AdminMain();
                            adminForm.FormClosed += (s, args) => this.Show(); // Show the Login form again when AdminMain form is closed
                            adminForm.Show();
                        }
                        else
                        {
                            UserMain userMain = new UserMain(userId);
                            userMain.FormClosed += (s, args) => this.Show(); // Show the Login form again when UserMain form is closed
                            userMain.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid credentials or role!");
                    }

                    DatabaseHelper.CloseConnection(conn);
                }
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
    }
}