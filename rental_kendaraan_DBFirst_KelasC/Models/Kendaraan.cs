using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rental_kendaraan_DBFirst_KelasC.Models
{
    public partial class Kendaraan
    {
        public Kendaraan()
        {
            Peminjaman = new HashSet<Peminjaman>();
        }

        public int IdKendaraan { get; set; }

        [Required(ErrorMessage = "NamaKendaraan Tidak Boleh Kosong!")]
        public string NamaKendaraan { get; set; }

        [Required(ErrorMessage = "NoPolisi Tidak Boleh Kosong!")]
        public string NoPolisi { get; set; }

        [Required(ErrorMessage = "NoStnk Tidak Boleh Kosong!")]
        [MinLength(15, ErrorMessage = "NoSTNK Kurang dari 20 Angka")]
        [MaxLength(15, ErrorMessage = "NoSTNK Tidak Boleh Lebih dari 20 Angka")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "No STNK Hanya Boleh Diisi dengan Angka")]
        public string NoStnk { get; set; }

        [Required(ErrorMessage = "IdJenisKendaraan Tidak Boleh Kosong!")]
        public int? IdJenisKendaraan { get; set; }

        [Required(ErrorMessage = "Ketersediaan Tidak Boleh Kosong!")]
        public string Ketersediaan { get; set; }

        public JenisKendaraan IdJenisKendaraanNavigation { get; set; }
        public ICollection<Peminjaman> Peminjaman { get; set; }
    }
}
