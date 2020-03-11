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
    public partial class SiparisForm : Form
    {
        public event EventHandler<MasaTasimaEventArgs> MasaTasiniyor;

        KafeContext db;
        Siparis siparis;
        BindingList<SiparisDetay> blSiparisDetaylar;
        public SiparisForm(KafeContext kafeVeri, Siparis siparis)
        {
            db = kafeVeri;
            this.siparis = siparis;
            blSiparisDetaylar
                = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            InitializeComponent();
            MasaNolariYukle();
            MasaNoGuncelle();
            TutarGuncelle();
            cboUrun.DataSource = db.Urunler;
            // cboUrun.SelectedItem = null;
            dgvSiparisDetaylari.DataSource = blSiparisDetaylar;
        }

        private void MasaNolariYukle()
        {
            cboMasaNo.Items.Clear();
            for (int i = 1; i <= db.MasaAdet; i++)
            {
                if (!db.AktifSiparisler.Any(x => x.MasaNo == i))
                {
                    cboMasaNo.Items.Add(i);
                }
            }
        }

        private void TutarGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
        }

        private void MasaNoGuncelle()
        {
            Text = "Masa " + siparis.MasaNo;

            lblMasaNo.Text = siparis.MasaNo.ToString("00");
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (cboUrun.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir ürün seçiniz!");
                return;
            }

            Urun seciliUrun = (Urun)cboUrun.SelectedItem;

            var sd = new SiparisDetay
            {
                UrunAd = seciliUrun.UrunAd,
                BirimFiyat = seciliUrun.BirimFiyat,
                Adet = (int)nudAdet.Value
            };
            blSiparisDetaylar.Add(sd);
            cboUrun.SelectedIndex = 0;
            nudAdet.Value = 1;
            TutarGuncelle();
        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show(
                "Sipariş iptal edilecektir. Onaylıyor musunuz?",
                "Sipariş İptal Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                siparis.Durum = SiparisDurum.Iptal;
                siparis.KapanisZamani = DateTime.Now;
                Close();
            }
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show(
                "Ödeme alındıysa masanın hesabı kapatılacaktır. Onaylıyor musunuz?",
                "Masa Kapatma Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                siparis.Durum = SiparisDurum.Odendi;
                siparis.KapanisZamani = DateTime.Now;
                siparis.OdenenTutar = siparis.ToplamTutar();
                Close();
            }
        }

        private void dgvSiparisDetaylari_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int rowIndex = dgvSiparisDetaylari.HitTest(e.X, e.Y).RowIndex;

                if (rowIndex > -1)
                {
                    dgvSiparisDetaylari.ClearSelection();
                    dgvSiparisDetaylari.Rows[rowIndex].Selected = true;
                    cmsSiparisDetay.Show(MousePosition);
                }

            }
        }

        private void tsmiSiparisDetaySil_Click(object sender, EventArgs e)
        {
            // seçili elemanı sildir
            if (dgvSiparisDetaylari.SelectedRows.Count > 0)
            {
                var seciliSatir = dgvSiparisDetaylari.SelectedRows[0];
                var sipDetay = (SiparisDetay)seciliSatir.DataBoundItem;
                blSiparisDetaylar.Remove(sipDetay);
            }

            TutarGuncelle();
        }

        private void btnTasi_Click(object sender, EventArgs e)
        {
            if (cboMasaNo.SelectedItem == null)
            {
                MessageBox.Show("Lütfen hedef masa noyu seçiniz.");
                return;
            }

            int eskiMasaNo = siparis.MasaNo;
            int hedefMasaNo = (int)cboMasaNo.SelectedItem;

            if (MasaTasiniyor != null)
            {
                var args = new MasaTasimaEventArgs
                {
                    TasinanSiparis = siparis,
                    EskiMasaNo = eskiMasaNo,
                    YeniMasaNo = hedefMasaNo
                };
                MasaTasiniyor(this, args);
            }

            siparis.MasaNo = hedefMasaNo;
            MasaNoGuncelle();
            MasaNolariYukle();
        }
    
    }

    public class MasaTasimaEventArgs : EventArgs
    {
        public Siparis TasinanSiparis { get; set; }
        public int EskiMasaNo { get; set; }
        public int YeniMasaNo { get; set; }
    }
}
