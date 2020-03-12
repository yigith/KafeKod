using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeKod.Data
{
   [Table("Urunler")]
    public class Urun
    {
        public Urun()
        {
            SiparisDetaylar = new List<SiparisDetay>();
        }

        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string UrunAd { get; set; }

        public decimal BirimFiyat { get; set; }

        public bool StoktaYok { get; set; }


        public virtual List<SiparisDetay> SiparisDetaylar { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1:0.00}₺", UrunAd, BirimFiyat);
        }
    }
}
