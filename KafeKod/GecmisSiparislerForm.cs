using KafeKod.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KafeKod
{
    public partial class GecmisSiparislerForm : Form
    {
        KafeContext db;
        public GecmisSiparislerForm(KafeContext kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();

            dgvSiparisler.DataSource = db.GecmisSiparisler;
        }

        private void dgvSiparisler_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSiparisler.SelectedRows.Count > 0)
            {
                DataGridViewRow satir = dgvSiparisler.SelectedRows[0];
                Siparis siparis = (Siparis)satir.DataBoundItem;
                dgvSiparisDetaylari.DataSource = siparis.SiparisDetaylar;
            }
        }
    }
}
