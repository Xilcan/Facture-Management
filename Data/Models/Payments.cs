using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    internal class Payments
    {
        public long Id { get; set; }

        public string Method { get; set; } = "Cash";

        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 9999999999999999.99)]
        public decimal Amount { get; set; }

        public DateTime PaymentDeadline { get; set; }

        public DateTime? PaymantDate { get; set; }

        public long FactureId { get; set; }

        public virtual Factures Facture { get; set; } = new Factures();
    }
}
