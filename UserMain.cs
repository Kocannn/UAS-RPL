using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UAS_RPL
{
    public partial class UserMain : Form
    {
        private int Id;
        public UserMain(int Id)
        {
            InitializeComponent();
            this.Id = Id;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            ListFilm listFilm = new ListFilm(Id);
            listFilm.Show();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Checkout checkout = new Checkout(Id);
            checkout.Show();
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
