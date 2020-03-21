using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rental_kendaraan_DBFirst_KelasC.Models
{
    public partial class KondisiKendaraan
    {
        public KondisiKendaraan()
        {
            Pengembalian = new HashSet<Pengembalian>();
        }

        public int IdKondisi { get; set; }

        [Required(ErrorMessage = "Kondisi Tidak Boleh Kosong!")]
        public string NamaKondisi { get; set; }

        public ICollection<Pengembalian> Pengembalian { get; set; }
    }
}
