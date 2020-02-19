using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeKod.Data
{
    public class Urun : IComparable
    {
        public string UrunAd { get; set; }
        public decimal BirimFiyat { get; set; }

        public int CompareTo(object obj)
        {
            return UrunAd.CompareTo(((Urun)obj).UrunAd);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1:0.00}₺", UrunAd, BirimFiyat);
        }
    }
}
