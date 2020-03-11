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
    public partial class UrunlerForm : Form
    {
        KafeContext db;
        BindingList<Urun> blUrunler;
        public UrunlerForm(KafeContext kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            blUrunler = new BindingList<Urun>(db.Urunler);
            dgvUrunler.DataSource = blUrunler;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text.Trim();

            if (urunAd == "")
            {
                MessageBox.Show("Lütfen bir ürün adı giriniz.");
                return;
            }

            blUrunler.Add(new Urun
            {
                UrunAd = urunAd,
                BirimFiyat = nudBirimFiyat.Value
            });
            db.Urunler.Sort();
        }

        // https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/handling-errors-that-occur-during-data-entry-in-the-datagrid#creating-the-form
        private void dgvUrunler_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Geçersiz bir değer girdiniz.");
        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // UrunAd'ı düzenliyorsa
            if (e.ColumnIndex == 0)
            {
                if (e.FormattedValue.ToString().Trim() == "")
                {
                    dgvUrunler.Rows[e.RowIndex].ErrorText = "Ürün ad boş geçilemez";
                    e.Cancel = true;
                }
                else
                {
                    dgvUrunler.Rows[e.RowIndex].ErrorText = "";
                }
            }
        }
    }
}
