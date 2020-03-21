using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rental_kendaraan_DBFirst_KelasC.Models
{
    public partial class Peminjaman
    {

        [Required(ErrorMessage = "IdPeminjaman Tidak Boleh Kosong!")]
        public int IdPeminjaman { get; set; }

        [Required(ErrorMessage = "TglPeminjaman Tidak Boleh Kosong!")]
        public DateTime? TglPeminjaman { get; set; }

        [Required(ErrorMessage = "IdKendaraan Tidak Boleh Kosong!")]
        public int? IdKendaraan { get; set; }

        [Required(ErrorMessage = "IdCustomer Tidak Boleh Kosong!")]
        public int? IdCustomer { get; set; }

        [Required(ErrorMessage = "IdJaminan Tidak Boleh Kosong!")]
        public int? IdJaminan { get; set; }

        [Required(ErrorMessage = " Biaya Tidak Boleh Kosong!")]
        public int? Biaya { get; set; }

        public Customer IdCustomerNavigation { get; set; }
        public Jaminan IdJaminanNavigation { get; set; }
        public Kendaraan IdKendaraanNavigation { get; set; }
    }
}
