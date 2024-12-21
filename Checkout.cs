using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRCoder;

namespace UAS_RPL
{
    public partial class Checkout : Form
    {
        private int userId;
        private FlowLayoutPanel tiketPanel;

        public Checkout(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            InitializeTiketPanel();
            LoadTiket();
        }

        private void InitializeTiketPanel()
        {
            // Buat panel utama untuk menampung semua tiket
            tiketPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.White
            };
            Controls.Add(tiketPanel);
        }

        private void LoadTiket()
        {
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    DatabaseHelper.OpenConnection(conn);
                    string query = @"
                        SELECT t.id, f.nama AS film_title, t.jumlah_tiket, t.total_biaya, t.waktu_pemesanan, t.status
                        FROM transaksi t
                        JOIN film f ON t.id_film = f.id
                        WHERE t.id_pengguna = @userId";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@userId", userId);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Tambahkan setiap tiket ke panel
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int transaksiId = Convert.ToInt32(row["id"]);
                        string filmTitle = row["film_title"].ToString();
                        int jumlahTiket = Convert.ToInt32(row["jumlah_tiket"]);
                        decimal totalBiaya = Convert.ToDecimal(row["total_biaya"]);
                        DateTime waktuPemesanan = Convert.ToDateTime(row["waktu_pemesanan"]);
                        bool status = Convert.ToBoolean(row["status"]);
                        AddTiket(transaksiId, filmTitle, jumlahTiket, totalBiaya, waktuPemesanan, status);
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

        private void AddTiket(int transaksiId, string filmTitle, int jumlahTiket, decimal totalBiaya, DateTime waktuPemesanan, bool status)
        {
            // Panel untuk satu tiket
            Panel tiketItem = new Panel
            {
                Size = new Size(300, 150),
                Margin = new Padding(10),
                BackColor = Color.WhiteSmoke,
                Tag = new TiketInfo { TransaksiId = transaksiId, FilmTitle = filmTitle, JumlahTiket = jumlahTiket, TotalBiaya = totalBiaya, Status = status }
            };

            // Label untuk judul film
            Label titleLbl = new Label
            {
                Text = $"Film: {filmTitle}",
                Location = new Point(10, 10),
                Size = new Size(280, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Label untuk jumlah tiket
            Label jumlahLbl = new Label
            {
                Text = $"Jumlah Tiket: {jumlahTiket}",
                Location = new Point(10, 40),
                Size = new Size(280, 20),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Label untuk total biaya
            Label biayaLbl = new Label
            {
                Text = $"Total Biaya: Rp {totalBiaya}",
                Location = new Point(10, 70),
                Size = new Size(280, 20),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Label untuk waktu pemesanan
            Label waktuLbl = new Label
            {
                Text = $"Waktu Pemesanan: {waktuPemesanan}",
                Location = new Point(10, 100),
                Size = new Size(280, 20),
                Font = new Font("Arial", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Tambahkan event handler untuk klik panel dan semua kontrol di dalamnya
            tiketItem.Click += TiketItem_Click;
            titleLbl.Click += TiketItem_Click;
            jumlahLbl.Click += TiketItem_Click;
            biayaLbl.Click += TiketItem_Click;
            waktuLbl.Click += TiketItem_Click;

            // Tambahkan semua kontrol ke panel tiket
            tiketItem.Controls.Add(titleLbl);
            tiketItem.Controls.Add(jumlahLbl);
            tiketItem.Controls.Add(biayaLbl);
            tiketItem.Controls.Add(waktuLbl);

            // Tambahkan panel tiket ke panel utama
            tiketPanel.Controls.Add(tiketItem);
        }

        private void TiketItem_Click(object sender, EventArgs e)
        {
            Control control = sender as Control;
            Panel tiketItem = null;

            // Pastikan event click berasal dari panel atau kontrol di dalam panel
            if (control is Panel)
            {
                tiketItem = control as Panel;
            }
            else
            {
                tiketItem = control.Parent as Panel;
            }

            if (tiketItem != null)
            {
                TiketInfo tiketInfo = tiketItem.Tag as TiketInfo;
                if (tiketInfo != null)
                {
                    // Periksa status tiket dari database
                    bool status = CheckTiketStatus(tiketInfo.TransaksiId);
                    if (status)
                    {
                        // Jika tiket sudah dibayar, tampilkan QR code
                        ShowQRCode(tiketInfo.FilmTitle, tiketInfo.JumlahTiket);
                    }
                    else
                    {
                        // Jika tiket belum dibayar, tampilkan dialog pembayaran
                        ShowPaymentDialog(tiketInfo.TransaksiId, tiketInfo.TotalBiaya);
                    }
                }
            }
        }

        private bool CheckTiketStatus(int transaksiId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    DatabaseHelper.OpenConnection(conn);
                    string query = "SELECT status FROM transaksi WHERE id = @transaksiId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@transaksiId", transaksiId);
                    bool status = Convert.ToBoolean(cmd.ExecuteScalar());
                    DatabaseHelper.CloseConnection(conn);
                    return status;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("MySQL Error: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private void ShowPaymentDialog(int transaksiId, decimal totalBiaya)
        {
            Form paymentForm = new Form
            {
                Text = "Pembayaran",
                Size = new Size(300, 200)
            };

            Label paymentLbl = new Label
            {
                Text = $"Lakukan pembayaran sebesar Rp {totalBiaya} untuk menyelesaikan transaksi.",
                Location = new Point(20, 20),
                Size = new Size(260, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Button payButton = new Button
            {
                Text = "Bayar",
                Location = new Point(100, 100),
                Size = new Size(100, 30)
            };
            payButton.Click += (s, e) => CompletePayment(transaksiId, totalBiaya, paymentForm);

            paymentForm.Controls.Add(paymentLbl);
            paymentForm.Controls.Add(payButton);

            paymentForm.ShowDialog();
        }

        private void CompletePayment(int transaksiId, decimal totalBiaya, Form paymentForm)
        {
            try
            {
                using (MySqlConnection conn = DatabaseHelper.GetConnection())
                {
                    DatabaseHelper.OpenConnection(conn);
                    string query = "UPDATE transaksi SET status = true WHERE id = @transaksiId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@transaksiId", transaksiId);
                    cmd.ExecuteNonQuery();
                    DatabaseHelper.CloseConnection(conn);

                    MessageBox.Show("Pembayaran berhasil!");

                    paymentForm.Close();

                    // Ambil informasi film dan jumlah tiket untuk QR code
                    string filmTitle;
                    int jumlahTiket;
                    using (MySqlConnection conn2 = DatabaseHelper.GetConnection())
                    {
                        DatabaseHelper.OpenConnection(conn2);
                        string query2 = "SELECT f.nama AS film_title, t.jumlah_tiket FROM transaksi t JOIN film f ON t.id_film = f.id WHERE t.id = @transaksiId";
                        MySqlCommand cmd2 = new MySqlCommand(query2, conn2);
                        cmd2.Parameters.AddWithValue("@transaksiId", transaksiId);
                        using (MySqlDataReader reader = cmd2.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                filmTitle = reader["film_title"].ToString();
                                jumlahTiket = Convert.ToInt32(reader["jumlah_tiket"]);
                            }
                            else
                            {
                                throw new Exception("Data tidak ditemukan.");
                            }
                        }
                        DatabaseHelper.CloseConnection(conn2);
                    }

                    // Tampilkan QR code setelah pembayaran berhasil
                    ShowQRCode(filmTitle, jumlahTiket);
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

        private void ShowQRCode(string filmTitle, int jumlahTiket)
        {
            Form qrForm = new Form
            {
                Text = "QR Code",
                Size = new Size(300, 300)
            };

            string qrContent = $"Film: {filmTitle}\nJumlah Tiket: {jumlahTiket}";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            PictureBox qrPictureBox = new PictureBox
            {
                Image = qrCodeImage,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            qrForm.Controls.Add(qrPictureBox);
            qrForm.ShowDialog();
        }

        private class TiketInfo
        {
            public int TransaksiId { get; set; }
            public string FilmTitle { get; set; }
            public int JumlahTiket { get; set; }
            public decimal TotalBiaya { get; set; }
            public bool Status { get; set; }
        }
    }
}