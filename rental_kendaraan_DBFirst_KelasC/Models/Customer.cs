using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rental_kendaraan_DBFirst_KelasC.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Peminjaman = new HashSet<Peminjaman>();
        }

        public int IdCustomer { get; set; }

        [Required(ErrorMessage ="Nama Customer Tidak Boleh Kosong!")]
        public string NamaCustomer { get; set; }

        [Required(ErrorMessage = "NIK Tidak Boleh Kosong!")]
        [MinLength(16, ErrorMessage ="NIK Kurang dari 16 Angka")]
        [MaxLength(16,ErrorMessage ="NIK Tidak Boleh Lebih dari 16 Angka")]
        [RegularExpression("^[0-9]*$", ErrorMessage ="NIK Hanya Boleh Diisi dengan Angka")]
        public string Nik { get; set; }

        [Required(ErrorMessage = "Alamat Tidak Boleh Kosong!")]
        public string Alamat { get; set; }

        [Required(ErrorMessage = "No.Hp Tidak Boleh Kosong!")]
        [MinLength(11, ErrorMessage = "Nomor Hp Kurang dari 11 Angka")]
        [MaxLength(13, ErrorMessage = "Nomor Hp Tidak Boleh Lebih dari 13 Angka")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Nomor Hp Hanya Boleh Diisi dengan Angka")]
        public string NoHp { get; set; }

        [Required(ErrorMessage = "Id Gender Tidak Boleh Kosong!")]
        public int? IdGender { get; set; }

        public Gender IdGenderNavigation { get; set; }
        public ICollection<Peminjaman> Peminjaman { get; set; }
    }
}
