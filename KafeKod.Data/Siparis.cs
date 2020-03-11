using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeKod.Data
{
    public enum SiparisDurum { Aktif, Odendi, Iptal }

    [Table("Siparisler")]
    public class Siparis
    {
        public Siparis()
        {
            SiparisDetaylar = new List<SiparisDetay>();
        }

        public int Id { get; set; }

        public int MasaNo { get; set; }

        public DateTime? AcilisZamani { get; set; }

        public DateTime? KapanisZamani { get; set; }

        public SiparisDurum Durum { get; set; }

        public decimal OdenenTutar { get; set; }  
        

        public virtual List<SiparisDetay> SiparisDetaylar { get; set; }
    }
}
