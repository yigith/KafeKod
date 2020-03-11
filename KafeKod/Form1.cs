using KafeKod.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KafeKod
{
    public partial class Form1 : Form
    {
        KafeContext db;

        public Form1()
        {
            VerileriOku();
            InitializeComponent();
            MasalariOlustur();
        }

        private void VerileriOku()
        {
            try
            {
                string json = File.ReadAllText("veri.json");
                db = JsonConvert.DeserializeObject<KafeContext>(json);
            }
            catch (Exception)
            {
                db = new KafeContext();
            }
        }

        private void OrnekVerileriYukle()
        {
            db.Urunler = new List<Urun>
            {
                new Urun { UrunAd = "Kola", BirimFiyat = 6.99m },
                new Urun { UrunAd = "Çay", BirimFiyat = 3.99m }
            };
            db.Urunler.Sort();
        }

        private void MasalariOlustur()
        {
            #region ListView Imajlarının Hazırlanması
            ImageList il = new ImageList();
            il.Images.Add("bos", Properties.Resources.masabos);
            il.Images.Add("dolu", Properties.Resources.masadolu);
            il.ImageSize = new Size(64, 64);
            lvwMasalar.LargeImageList = il;
            #endregion

            ListViewItem lvi;
            for (int i = 1; i <= db.MasaAdet; i++)
            {
                lvi = new ListViewItem("Masa " + i);

                // i masa no değeriyle kayıtlı bir sipariş var mı?
                Siparis sip = db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == i);

                if (sip == null)
                {
                    lvi.Tag = i;
                    lvi.ImageKey = "bos";
                }
                else
                {
                    lvi.Tag = sip;
                    lvi.ImageKey = "dolu";
                }

                lvwMasalar.Items.Add(lvi);
            }
        }

        private void lvwMasalar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var lvi = lvwMasalar.SelectedItems[0];
                lvi.ImageKey = "dolu";

                Siparis sip;
                // masa doluysa olanı al, boşsa yeni oluştur
                if (lvi.Tag is Siparis)
                {
                    sip = (Siparis)lvi.Tag;
                }
                else
                {
                    sip = new Siparis();
                    sip.MasaNo = (int)lvi.Tag;
                    sip.AcilisZamani = DateTime.Now;
                    lvi.Tag = sip;
                    db.AktifSiparisler.Add(sip);
                }

                SiparisForm frmSiparis = new SiparisForm(db, sip);
                frmSiparis.MasaTasiniyor += FrmSiparis_MasaTasindi;
                frmSiparis.ShowDialog();


                if (sip.Durum == SiparisDurum.Odendi 
                    || sip.Durum == SiparisDurum.Iptal)
                {
                    lvi.Tag = sip.MasaNo;
                    lvi.ImageKey = "bos";
                    db.AktifSiparisler.Remove(sip);
                    db.GecmisSiparisler.Add(sip);
                }
            }
        }

        private void FrmSiparis_MasaTasindi(object sender, MasaTasimaEventArgs e)
        {
            // adım 1: eski masayı boşalt
            ListViewItem lviEskiMasa = MasaBul(e.EskiMasaNo);
            lviEskiMasa.Tag = e.EskiMasaNo;
            lviEskiMasa.ImageKey = "bos";


            // adım 2: yeni masaya siparişi koy
            ListViewItem lviYeniMasa = MasaBul(e.YeniMasaNo);
            lviYeniMasa.Tag = e.TasinanSiparis;
            lviYeniMasa.ImageKey = "dolu";
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            var frm = new GecmisSiparislerForm(db);
            frm.ShowDialog();
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            var frm = new UrunlerForm(db);
            frm.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string json = JsonConvert.SerializeObject(db);
            File.WriteAllText("veri.json", json);
        }

        private ListViewItem MasaBul(int masaNo)
        {
            foreach (ListViewItem item in lvwMasalar.Items)
            {
                if (item.Tag is int && (int)item.Tag == masaNo)
                {
                    return item;
                }
                else if (item.Tag is Siparis && ((Siparis)item.Tag).MasaNo == masaNo)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
