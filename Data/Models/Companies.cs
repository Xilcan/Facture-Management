using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    internal class Companies
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Nazwa firmy jest wymagana")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nip Firmy jest wymagany")]
        public long NIP { get; set; }

        public virtual ICollection<Factures>? Factures { get; set; }
        public virtual ICollection<CompanyAddress>? Addresses { get; set; }
    }
}
