using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rental_kendaraan_DBFirst_KelasC.Models
{
    public partial class JenisKendaraan
    {
        public JenisKendaraan()
        {
            Kendaraan = new HashSet<Kendaraan>();
        }

        public int IdJenisKendaraan { get; set; }

        [Required(ErrorMessage = "JenisKendaraan Tidak Boleh Kosong!")]
        public string NamaJenisKendaraan { get; set; }

        public ICollection<Kendaraan> Kendaraan { get; set; }
    }
}
