using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeKod.Data
{
    [Table("SiparisDetaylar")]
    public class SiparisDetay
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string UrunAd { get; set; }

        public decimal BirimFiyat { get; set; }

        public int Adet { get; set; }
        
        public int UrunId { get; set; }

        public int SiparisId { get; set; }


        public virtual Urun Urun { get; set; }
        public virtual Siparis Siparis { get; set; }
    }
}
