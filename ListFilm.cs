using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace UAS_RPL
{
    public partial class ListFilm : Form
    {
        private FlowLayoutPanel moviePanel;
        private int userId; // Ganti dengan id_pengguna yang sesuai

        public ListFilm(int Id)
        {
            InitializeComponent();
            InitializeMoviePanel();
            this.userId = Id;
        }

        private void InitializeMoviePanel()
        {
            // Buat panel utama untuk menampung semua film
            moviePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.White
            };
            Controls.Add(moviePanel);
        }

        private void ListFilm_Load(object sender, EventArgs e)
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

                    // Tambahkan setiap film ke panel
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string title = row["nama"].ToString();
                        string price = row["harga"].ToString();
                        string imageUrl = row["gambar"].ToString();
                        string description = row["deskripsi"].ToString();
                        int filmId = Convert.ToInt32(row["id"]);
                        AddMovie(title, price, imageUrl, description, filmId);
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

        private void AddMovie(string title, string price, string imagePath, string description, int filmId)
        {
            // Panel untuk satu film
            Panel movieItem = new Panel
            {
                Size = new Size(200, 300),
                Margin = new Padding(10),
                BackColor = Color.WhiteSmoke,
                Tag = new MovieInfo { Title = title, Price = price, ImagePath = imagePath, Description = description, FilmId = filmId }
            };

            // PictureBox untuk gambar film
            PictureBox pb = new PictureBox
            {
                Size = new Size(180, 200),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            try
            {
                pb.Image = Image.FromFile(imagePath);
            }
            catch
            {
                // Gunakan gambar placeholder jika gambar tidak ditemukan
                pb.BackColor = Color.Gray;
            }

            // Label untuk judul film
            Label titleLbl = new Label
            {
                Text = title,
                Location = new Point(10, 220),
                Size = new Size(180, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Label untuk harga
            Label priceLbl = new Label
            {
                Text = $"Rp {price}",
                Location = new Point(10, 250),
                Size = new Size(180, 20),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Tambahkan event handler untuk klik panel dan semua kontrol di dalamnya
            movieItem.Click += MovieItem_Click;
            pb.Click += MovieItem_Click;
            titleLbl.Click += MovieItem_Click;
            priceLbl.Click += MovieItem_Click;

            // Tambahkan semua kontrol ke panel film
            movieItem.Controls.Add(pb);
            movieItem.Controls.Add(titleLbl);
            movieItem.Controls.Add(priceLbl);

            // Tambahkan panel film ke panel utama
            moviePanel.Controls.Add(movieItem);
        }

        private void MovieItem_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                Panel movieItem = control.Parent as Panel;
                if (movieItem != null)
                {
                    MovieInfo movieInfo = movieItem.Tag as MovieInfo;
                    if (movieInfo != null)
                    {
                        ShowMovieDetails(movieInfo);
                    }
                }
            }
        }

        private void ShowMovieDetails(MovieInfo movieInfo)
        {
            Form detailsForm = new Form
            {
                Text = movieInfo.Title,
                Size = new Size(400, 500)
            };

            // PictureBox untuk gambar film
            PictureBox pb = new PictureBox
            {
                Size = new Size(360, 240),
                Location = new Point(20, 20),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            try
            {
                pb.Image = Image.FromFile(movieInfo.ImagePath);
            }
            catch
            {
                pb.BackColor = Color.Gray;
            }

            // Label untuk deskripsi film
            Label descriptionLbl = new Label
            {
                Text = movieInfo.Description,
                Location = new Point(20, 270),
                Size = new Size(360, 100),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.TopLeft
            };

            // Label untuk jumlah tiket
            Label quantityLbl = new Label
            {
                Text = "Jumlah Tiket:",
                Location = new Point(20, 380),
                Size = new Size(100, 20),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // NumericUpDown untuk memilih jumlah tiket
            NumericUpDown quantityUpDown = new NumericUpDown
            {
                Location = new Point(130, 380),
                Size = new Size(50, 20),
                Minimum = 1,
                Maximum = 10,
                Value = 1
            };

            // Tombol untuk memesan film
            Button orderButton = new Button
            {
                Text = "Pesan Film",
                Location = new Point(150, 420),
                Size = new Size(100, 30)
            };
            orderButton.Click += (s, e) => OrderMovie(movieInfo, (int)quantityUpDown.Value);

            // Tambahkan semua kontrol ke form detail
            detailsForm.Controls.Add(pb);
            detailsForm.Controls.Add(descriptionLbl);
            detailsForm.Controls.Add(quantityLbl);
            detailsForm.Controls.Add(quantityUpDown);
            detailsForm.Controls.Add(orderButton);

            // Tampilkan form detail
            detailsForm.ShowDialog();
        }

        private void OrderMovie(MovieInfo movieInfo, int quantity)
        {
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    DatabaseHelper.OpenConnection(conn);
                    string query = "INSERT INTO transaksi (id_pengguna, id_film, jumlah_tiket, total_biaya, waktu_pemesanan) VALUES (@id_pengguna, @id_film, @jumlah_tiket, @total_biaya, @waktu_pemesanan)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_pengguna", userId);
                    cmd.Parameters.AddWithValue("@id_film", movieInfo.FilmId);
                    cmd.Parameters.AddWithValue("@jumlah_tiket", quantity);
                    cmd.Parameters.AddWithValue("@total_biaya", quantity * Convert.ToDecimal(movieInfo.Price));
                    cmd.Parameters.AddWithValue("@waktu_pemesanan", DateTime.Now);

                    cmd.ExecuteNonQuery();
                    DatabaseHelper.CloseConnection(conn);

                    MessageBox.Show("Pemesanan berhasil!");
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

        private class MovieInfo
        {
            public string Title { get; set; }
            public string Price { get; set; }
            public string ImagePath { get; set; }
            public string Description { get; set; }
            public int FilmId { get; set; }
        }
    }
}