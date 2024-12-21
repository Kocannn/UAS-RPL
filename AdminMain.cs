using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UAS_RPL
{
    public partial class AdminMain : Form
    {
        private string imagePath;
        private int Id;

        public AdminMain()
        {
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {

        }

        private void AddBTN_Click(object sender, EventArgs e)
        {

        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    DatabaseHelper.OpenConnection(conn);
                    string query = "SELECT * FROM film";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    metroGrid1.DataSource = dataTable;
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

        private void LoadTransaksiData()
        {
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    DatabaseHelper.OpenConnection(conn);
                    string query = @"
                        SELECT t.id, p.nama AS pengguna_nama, f.nama AS film_title, t.jumlah_tiket, t.total_biaya, t.waktu_pemesanan, t.status
                        FROM transaksi t
                        JOIN film f ON t.id_film = f.id
                        JOIN pengguna p ON t.id_pengguna = p.id";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    metroGrid1.DataSource = dataTable;
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

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imagePath = ofd.FileName;
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(inputHarga.Text, out decimal harga))
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    MessageBox.Show("Please select an image.");
                    return;
                }

                try
                {
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        DatabaseHelper.OpenConnection(conn);
                        string query = "INSERT INTO film (nama, deskripsi, harga, gambar) VALUES (@nama, @deskripsi, @harga, @gambar)";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@nama", inputJudul.Text);
                        cmd.Parameters.AddWithValue("@deskripsi", inputSinopsis.Text);
                        cmd.Parameters.AddWithValue("@harga", harga);
                        cmd.Parameters.AddWithValue("@gambar", imagePath);
                        cmd.ExecuteScalar();
                        DatabaseHelper.CloseConnection(conn);
                    }

                    MessageBox.Show("Data berhasil disimpan!");
                    LoadData();
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
            else
            {
                MessageBox.Show("Harga tiket harus berupa angka yang valid.");
            }
        }

        private void AdminMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = metroGrid1.Rows[e.RowIndex];
                inputJudul.Text = row.Cells["nama"].Value.ToString();
                inputSinopsis.Text = row.Cells["deskripsi"].Value.ToString();
                inputHarga.Text = row.Cells["harga"].Value.ToString();
                imagePath = row.Cells["gambar"].Value.ToString();
                pictureBox1.Image = Image.FromFile(imagePath);
                Id = Convert.ToInt32(row.Cells["id"].Value);
            }
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(inputHarga.Text, out decimal harga))
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    MessageBox.Show("Please select an image.");
                    return;
                }

                try
                {
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        DatabaseHelper.OpenConnection(conn);
                        string query = "UPDATE film SET nama = @nama, deskripsi = @deskripsi, harga = @harga, gambar = @gambar WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@nama", inputJudul.Text);
                        cmd.Parameters.AddWithValue("@deskripsi", inputSinopsis.Text);
                        cmd.Parameters.AddWithValue("@harga", harga);
                        cmd.Parameters.AddWithValue("@gambar", imagePath);
                        cmd.Parameters.AddWithValue("@id", Id); // Assumes you have a variable to store the selected film's ID
                        cmd.ExecuteNonQuery();
                        DatabaseHelper.CloseConnection(conn);
                    }

                    MessageBox.Show("Data berhasil diperbarui!");
                    LoadData();
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
            else
            {
                MessageBox.Show("Harga tiket harus berupa angka yang valid.");
            }
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            if (Id == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            var confirmResult = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        DatabaseHelper.OpenConnection(conn);
                        string query = "DELETE FROM film WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", Id);
                        cmd.ExecuteNonQuery();
                        DatabaseHelper.CloseConnection(conn);
                    }

                    MessageBox.Show("Data berhasil dihapus!");
                    LoadData();
                    ClearInputs();
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

        private void ClearInputs()
        {
            inputJudul.Text = string.Empty;
            inputSinopsis.Text = string.Empty;
            inputHarga.Text = string.Empty;
            imagePath = string.Empty;
            pictureBox1.Image = null;
            Id = 0;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            LoadTransaksiData();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}