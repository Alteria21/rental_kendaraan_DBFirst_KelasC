using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace rental_kendaraan_DBFirst_KelasC.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Customer = new HashSet<Customer>();
        }


        public int IdGender { get; set; }

        [Required(ErrorMessage = "NamaGender Tidak Boleh Kosong!")]
        public string NamaGender { get; set; }

        public ICollection<Customer> Customer { get; set; }
    }
}
