using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rental_kendaraan_DBFirst_KelasC.Models
{
    public partial class Jaminan
    {
        public Jaminan()
        {
            Peminjaman = new HashSet<Peminjaman>();
        }

        public int IdJaminan { get; set; }

        [Required(ErrorMessage = "NamaJaminan Tidak Boleh Kosong!")]
        public string NamaJaminan { get; set; }

        public ICollection<Peminjaman> Peminjaman { get; set; }
    }
}
