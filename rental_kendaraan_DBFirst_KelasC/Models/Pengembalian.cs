using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rental_kendaraan_DBFirst_KelasC.Models
{
    public partial class Pengembalian
    {

        [Required(ErrorMessage = "IdPengembalian Tidak Boleh Kosong!")]
        public int IdPengembalian { get; set; }

        [Required(ErrorMessage = "TglPengembalian Tidak Boleh Kosong!")]
        public DateTime? TglPengembalian { get; set; }

        [Required(ErrorMessage = "IdPeminjaman Tidak Boleh Kosong!")]
        public int? IdPeminjaman { get; set; }

        [Required(ErrorMessage = "IdKondisi Tidak Boleh Kosong!")]
        public int? IdKondisi { get; set; }

        [Required(ErrorMessage = "Denda Tidak Boleh Kosong!")]
        public int? Denda { get; set; }

        public KondisiKendaraan IdKondisiNavigation { get; set; }
    }
}
